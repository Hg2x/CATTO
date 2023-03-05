using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpawnSettings", menuName = "ScriptableObject/ItemSpawnSettings", order = 0)]
public class ItemSpawnSettings : ScriptableObject
{
    [Range(0f, 1f)] public float HorizontalSpawnPercentage = 0.5f;
    [Min(0f)] public float SpawnRateVariability = 2f;
    [Min(0f)] public float StartingSpawnRate = 5f;
    [Min(0f)] public float FastestSpawnRate = 1f;
    [Min(0f)] public float TimeUntilMaxSpawnRate = 120f;
    [Min(0f)] public float ItemSpeedVariability = 1.5f;
    [Min(0f)] public float StartingItemSpeed = 2f;
    [Min(0f)] public float FinalItemSpeed = 5f;
    [Min(0f)] public float TimeUntilMaxItemSpeed = 120f;
    public ItemData[] _ItemDatas;
}
