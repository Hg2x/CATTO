using ICKT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [HideInInspector] public ItemIntensity Intensity;
    public float ItemSpeed;
    [Min(0.1f)] public float ItemSize = 1;
    [HideInInspector] public Color ItemColor;
    public Sprite ItemSprite;

    public virtual void Initialize()
    {
        Color color = Color.white;
        switch (Intensity)
        {
            case ItemIntensity.Weak:
                if (FunctionLibrary.GetRandomBool())
                {
                    color = Const.LightCreme;
                }
                else
                {
                    color = Const.MediumCreme;
                }
                break;
            case ItemIntensity.Medium:
                if (FunctionLibrary.GetRandomBool())
                {
                    color = Const.LightOrange;
                }
                else
                {
                    color = Const.MediumOrange;
                }
                break;
            case ItemIntensity.Strong:
                color = Const.DarkPurple;
                break;

            default:
                break;
        }
        ItemColor = color;
    }

    public abstract void PickUpEffect();
}
