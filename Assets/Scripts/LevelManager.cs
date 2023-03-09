using ICKT;
using ICKT.Services;
using ICKT.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IRegisterable
{
    [SerializeField] private PlayerData _PlayerData;
    [SerializeField] private ItemSpawnSettings _SpawnerData;
    [SerializeField] private ItemSpawner[] _Spawners;
    [SerializeField] private PickUpItem _Item;

    private Coroutine _SpawnCoroutine;
    private float _CurrentSpawnRate;
    private float _CurrentItemSpeed;
    public float ElapsedTime { get; private set; }
    public int ElapsedTimePlusOne;

    public delegate void OneSecondPassedEvent(int totalTime);
    public event OneSecondPassedEvent OnOneSecondPassed;

    public bool IsPersistent() => false;

    private void Awake()
    {
        ServiceLocator.Register(this, true);
    }

    private void Start()
    {
        StartLevel();
    }

    private void OnDisable()
    {
        if (ServiceLocator.IsRegistered<LevelManager>())
        {
            ServiceLocator.Unregister(this);
        }
        OnOneSecondPassed = null;
    }

    private void OnDestroy()
    {
        if (ServiceLocator.IsRegistered<LevelManager>())
        {
            ServiceLocator.Unregister(this);
        }
        OnOneSecondPassed = null;
    }

    private void Update()
    {
        ElapsedTime += Time.deltaTime;

        if (ElapsedTime > ElapsedTimePlusOne)
        {
            OnOneSecondPassed?.Invoke(ElapsedTimePlusOne);
            ElapsedTimePlusOne++;
        }

        if (ElapsedTime > _SpawnerData.TimeUntilMaxSpawnRate)
        {
            _CurrentSpawnRate = _SpawnerData.FastestSpawnRate;
        }
        else
        {
            _CurrentSpawnRate = Mathf.Lerp(_SpawnerData.StartingSpawnRate, _SpawnerData.FastestSpawnRate, ElapsedTime / _SpawnerData.TimeUntilMaxSpawnRate);
        }

        if (ElapsedTime > _SpawnerData.TimeUntilMaxItemSpeed)
        {
            _CurrentItemSpeed = _SpawnerData.FinalItemSpeed;
        }
        else
        {
            _CurrentItemSpeed = Mathf.Lerp(_SpawnerData.StartingItemSpeed, _SpawnerData.FinalItemSpeed, ElapsedTime / _SpawnerData.TimeUntilMaxItemSpeed);
        }
    }

    public void DamagePlayer(int amount)
    {
        _PlayerData.ModifyHealth(-amount);
    }

    public void ChangePlayerSpeed(float amountToAdd, float duration)
    {
        _PlayerData.ModifySpeed(amountToAdd);
        if (duration > 0)
        {
            StartCoroutine(RemoveSpeedChange(amountToAdd, duration));
        }
    }

    private void StartLevel()
    {
        _PlayerData.ResetData();
        _PlayerData.OnPlayerDied += GameOver;
        ElapsedTime = 0f;
        ElapsedTimePlusOne = 1;
        UIManager.Show<UIBattle>();
        StartSpawning();
    }

    private void StartSpawning()
    {
        _CurrentSpawnRate = _SpawnerData.StartingSpawnRate;
        _CurrentItemSpeed = _SpawnerData.StartingItemSpeed;

        _SpawnCoroutine = StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            var delay = Mathf.Max(0f, _CurrentSpawnRate - _SpawnerData.SpawnRateVariability / 2f) + UnityEngine.Random.value * _SpawnerData.SpawnRateVariability;
            yield return new WaitForSeconds(delay);

            SpawnRandom();
        }
    }

    private void SpawnRandom()
    {
        var itemIndex = FunctionLibrary.GetRandomNumber(0, _SpawnerData._ItemDatas.Length - 1);
        var spawnerIndex = FunctionLibrary.GetRandomNumber(0, _Spawners.Length - 1);
        var enumLength = Enum.GetValues(typeof(ItemIntensity)).Length;
        var intensity = FunctionLibrary.GetRandomNumber(0, enumLength - 1);

		var itemData = Instantiate(_SpawnerData._ItemDatas[itemIndex]);
		itemData.Intensity = (ItemIntensity)intensity;
        var speed = Mathf.Max(0f, _CurrentItemSpeed - _SpawnerData.ItemSpeedVariability / 2f) + (UnityEngine.Random.value * _SpawnerData.ItemSpeedVariability) * itemData.ItemSpeed;

        _Spawners[spawnerIndex].SpawnItem(_Item, itemData, speed);
    }

    private IEnumerator RemoveSpeedChange(float amountToRemove, float duration)
    {
        yield return new WaitForSeconds(duration);
        _PlayerData.ModifySpeed(-amountToRemove);
    }

    private void GameOver()
    {
        StopCoroutine(_SpawnCoroutine);
        GameInstance.PauseGame();
        UIManager.Show<UIGameOver>();
    }
}
