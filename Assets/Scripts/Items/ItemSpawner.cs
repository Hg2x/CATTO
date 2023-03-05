using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Vector2 _DirectionToSpawn;

    public void SpawnItem(PickUpItem item, ItemData data, float itemSpeed = 0)
    {
        Vector2 randomPosition = new(Random.Range(transform.position.x - transform.localScale.x / 2, transform.position.x + transform.localScale.x / 2),
                                     Random.Range(transform.position.y - transform.localScale.y / 2, transform.position.y + transform.localScale.y / 2));
        var spawnedItem = Instantiate(item, randomPosition, Quaternion.identity);
        spawnedItem.Initialize(data, _DirectionToSpawn * itemSpeed);
    }  
}
