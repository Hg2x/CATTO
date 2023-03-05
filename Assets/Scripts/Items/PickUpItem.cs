using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PickUpItem : MonoBehaviour, IPickUppable
{
    private ItemData _Data;
    private SpriteRenderer _SpriteRenderer;
    private Rigidbody2D _Rigidbody;

    protected void Awake()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _Rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnItemPickUp()
    {
        _Data.PickUpEffect();
    }

    public void Initialize(ItemData data, Vector2? optionalVector = null)
    {
        if (data == null)
        {
            throw new InvalidOperationException($"ItemData is null: {data}");
        }
        _Data = data;
        _Data.Initialize();
        _SpriteRenderer.sprite = _Data.ItemSprite;
        _SpriteRenderer.color = _Data.ItemColor;
        gameObject.transform.localScale = Vector3.one * _Data.ItemSize;
        if (optionalVector != null)
        {
            _Rigidbody.velocity = optionalVector.Value.normalized * _Data.ItemSpeed;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            OnItemPickUp();
            Destroy(gameObject);
        }
    }
}
