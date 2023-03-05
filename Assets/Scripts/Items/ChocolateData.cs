using ICKT.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChocolateData", menuName = "ScriptableObject/ItemData/ChocolateData", order = 0)]
public class ChocolateData : ItemData
{
    [Header("Chocolate Specific")]
    [Min(1)] public int WeakDamageAmount;
    [Min(1)] public int MediumDamageAmount;
    [Min(1)] public int StrongDamageAmount;

    public override void PickUpEffect()
    {
        ServiceLocator.Get<LevelManager>().DamagePlayer(GetDamageAmount());
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_CAT_HURT_EVENT);
    }

    private int GetDamageAmount()
    {
        switch(Intensity)
        {
            case ItemIntensity.Weak:
                return WeakDamageAmount;
            case ItemIntensity.Medium:
                return MediumDamageAmount;
            case ItemIntensity.Strong:
                return StrongDamageAmount;
            default:
                throw new InvalidOperationException($"Invalid item intensity: {Intensity}");
        }
    }
}
