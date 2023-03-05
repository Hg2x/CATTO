using ICKT;
using ICKT.Services;
using ICKT.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattle : UIBase, IRegisterable
{
    [SerializeField] private PlayerData _PlayerData;
    [SerializeField] private Image _VisionBlocker;
    [SerializeField] private TextMeshProUGUI _TimerText;
    [SerializeField] private Transform _HealthImages;
    [SerializeField] private Image _HeartReference;
    private readonly List<GameObject> _Hearts = new();
    private float _VisionBlockAmount;

    public bool IsPersistent() => false;

    private void Awake()
    {
        _VisionBlockAmount = _VisionBlocker.color.a;
        ServiceLocator.Register(this, true);
    }

    private void OnDisable()
    {
        if (ServiceLocator.IsRegistered<UIBattle>())
        {
            ServiceLocator.Unregister(this);
        }
        if (ServiceLocator.IsRegistered<LevelManager>())
        {
            ServiceLocator.Get<LevelManager>().OnOneSecondPassed -= UIBattle_OnOneSecondPassed;
        }
        _PlayerData.OnHealthChanged -= ChangeHeartAmount;
    }

    private void OnDestroy()
    {
        if (ServiceLocator.IsRegistered<UIBattle>())
        {
            ServiceLocator.Unregister(this);
        }
        if (ServiceLocator.IsRegistered<LevelManager>())
        {
            ServiceLocator.Get<LevelManager>().OnOneSecondPassed -= UIBattle_OnOneSecondPassed;
        }
        _PlayerData.OnHealthChanged -= ChangeHeartAmount;
    }

    private void Start()
    {
        Initialize();
        if (ServiceLocator.IsRegistered<LevelManager>())
        {
            ServiceLocator.Get<LevelManager>().OnOneSecondPassed += UIBattle_OnOneSecondPassed;
        }
        _PlayerData.OnHealthChanged += ChangeHeartAmount;
    }

    private void UIBattle_OnOneSecondPassed(int totalTime)
    {
        ChangeTimerText(totalTime.ToString());
    }

    private void Initialize()
    {
        var heartAmount = _PlayerData.Health;
        for(int i = 0; i < heartAmount; i++)
        {
            var heart = Instantiate(_HeartReference, _HealthImages);
            heart.gameObject.SetActive(true);
            _Hearts.Add(heart.gameObject);
        }
        ChangeTimerText("0");
    }

    public void ChangeTimerText(string text)
    {
        _TimerText.text = text;
    }

    public void OnPauseButtonClicked()
    {
        GameInstance.PauseGame();
        UIManager.Show<UIPauseMenu>();
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_UI_CLICK_EVENT);
    }

    public void ModifyVisionBlockerAlpha(float amountToAdd, float duration)
    {
        if (duration <= 0.01f)
        {
            duration = 1f;
        }
        ChangeVisionBlockerAlpha(amountToAdd);
        StartCoroutine(DelayedChangeVisionBlockerAlpha(-amountToAdd, duration));
    }

    private void ChangeHeartAmount(int healthAmountChanged)
    {
        if (healthAmountChanged < 0)
        {
            RemoveHeart(-healthAmountChanged);
        }
    }

    private void RemoveHeart(int amountToRemove)
    {
        for (int i = _Hearts.Count - 1; i >= 0; i--)
        {
            if (amountToRemove < 1)
            {
                return;
            }
            if (_Hearts[i].activeSelf)
            {
                _Hearts[i].SetActive(false);
                amountToRemove--;
            }
        }
    }

    private void ChangeVisionBlockerAlpha(float amount)
    {
        _VisionBlockAmount += amount;
        if (_VisionBlockAmount < 0)
        {
            _VisionBlockAmount = 0;
        }
        if (_VisionBlockAmount > 1)
        {
            _VisionBlockAmount = 1;
        }

        Color newColor = _VisionBlocker.color;
        newColor.a = _VisionBlockAmount;
        _VisionBlocker.color = newColor;
    }

    private IEnumerator DelayedChangeVisionBlockerAlpha(float amountToRemove, float duration)
    {
        yield return new WaitForSeconds(duration);
        ChangeVisionBlockerAlpha(amountToRemove);
    }
}
