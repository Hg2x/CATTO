using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerData _Data;
    private Rigidbody2D _Rigidbody;
    private Animator _Animator;
    private SpriteRenderer _SpriteRenderer;
    private GameObject _Arrow;

    private bool _FacingRight;
    private string _CurrentAnimation;

    private const string IDLE_1 = "Idle1";
    private const string JUMP_UP = "JumpUp";
    private const string JUMP_DOWN = "JumpDown";

    public void TakeDamage(int amount)
    {
        _Data.ModifyHealth(amount);
    }

    private void Start()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        if (_Arrow == null)
        {
            _Arrow = Instantiate(_Data.Arrow);
            _Arrow.transform.SetParent(transform, false);
        }
        _Arrow.SetActive(false);
        _Data.LaunchDirection = new Vector2(0, 0);
    }

    private void Update()
    {
        if (_Data.UpdateArrow)
        {
            _Data.MouseReleasePosition = Input.mousePosition;
            _Data.LaunchDirection = ((Vector2)_Data.MouseReleasePosition - (Vector2)_Data.MouseStartPosition).normalized;
            if (_Data.LaunchDirection != new Vector2(0, 0))
            {
                _Arrow.SetActive(true);
            }

            _Arrow.transform.position = (Vector2)gameObject.transform.position + _Data.LaunchDirection * _Data.ArrowDistance;
            float angle = Mathf.Atan2(_Data.LaunchDirection.y, _Data.LaunchDirection.x) * Mathf.Rad2Deg;
            _Arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        UpdateAnimation();
    }

    private void OnEnable()
    {
        _Data.PlayerControls ??= new PlayerControls();
        _Data.PlayerControls.Player.Enable();
        _Data.PlayerControls.Player.LeftClick.started += LeftClick_started;
        _Data.PlayerControls.Player.LeftClick.canceled += LeftClick_canceled;
    }

    private void OnDisable()
    {
        _Data.PlayerControls.Player.Disable();
        _Data.PlayerControls.Player.LeftClick.started -= LeftClick_started;
        _Data.PlayerControls.Player.LeftClick.canceled -= LeftClick_canceled;
    }

    private void LeftClick_started(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        _Data.MouseStartPosition = Input.mousePosition;
        _Data.UpdateArrow = true;
    }

    private void LeftClick_canceled(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        _Data.UpdateArrow = false;
        _Arrow.SetActive(false);
        Vector2 launchForce = _Data.LaunchDirection * _Data.LaunchSpeed;
        _Rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
        if (launchForce != Vector2.zero)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_CAT_JUMP_EVENT);
        }
        _Data.LaunchDirection = new Vector2(0, 0);
    }

    private void UpdateAnimation()
    {
        var velocity = _Rigidbody.velocity;
        if (Mathf.Abs(velocity.x) > _Data.LeftRightToleraence)
        {
            _FacingRight = velocity.x > 0;
        }

        if (_FacingRight)
        {
            _SpriteRenderer.flipX = false;
        }
        else
        {
            _SpriteRenderer.flipX = true;
        }
        
        if (Mathf.Abs(velocity.y) < _Data.OnGroundTolerance)
        {
            ChangeAnimationState(IDLE_1);
        }
        else
        {
            if (velocity.y > _Data.OnGroundTolerance)
            {
                ChangeAnimationState(JUMP_UP);
            }
            else if (velocity.y < -_Data.OnGroundTolerance)
            {
                ChangeAnimationState(JUMP_DOWN);
            }
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (_CurrentAnimation == newAnimation)
        {
            return;
        }

        _Animator.Play(newAnimation);
        _CurrentAnimation = newAnimation;
    }
}
