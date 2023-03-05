using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObject/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Header("Stat Values to tweak")]
    [Min(1)] public int MaxHealth = 9;
    public float BaseLaunchSpeed = 10f;
    public PlayerControls PlayerControls;
    public GameObject Arrow;
    public float ArrowDistance;

    [Header("For Monitoring purposes only")]
    public int Health;
    public float LaunchSpeed;

    [Header("Animation Values")]
    public float OnGroundTolerance = 0.01f;
    public float LeftRightToleraence = 0.01f;

    [HideInInspector] public bool UpdateArrow;
    [HideInInspector] public Vector3 MouseStartPosition;
    [HideInInspector] public Vector3 MouseReleasePosition;
    [HideInInspector] public Vector2 LaunchDirection;

    public delegate void HealthChangedEvent(int amountChanged);
    public event HealthChangedEvent OnHealthChanged;
    public delegate void PlayerDiedEvent();
    public event PlayerDiedEvent OnPlayerDied;

    public void ResetData()
    {
        Health = MaxHealth;
        LaunchSpeed = BaseLaunchSpeed;
        OnHealthChanged = null;
        OnPlayerDied = null;
    }

    public void ModifyHealth(int amount)
    {
        Health = Mathf.Clamp(Health + amount, 0, MaxHealth);
        OnHealthChanged?.Invoke(amount);
        if (Health <= 0)
        {
            Health = 0;
            OnPlayerDied?.Invoke();
        }
    }

    public void ModifySpeed(float amount)
    {
        LaunchSpeed = Mathf.Clamp(LaunchSpeed + amount, 0, 100);
    }
}
