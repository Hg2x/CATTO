using ICKT.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MilkData", menuName = "ScriptableObject/ItemData/MilkData", order = 0)]
public class MilkData : ItemData
{
    [Header("Milk Specific")]
    [Range(0, 1)] public float WeakAmount;
    [Min(0)] public float WeakDuration;
    [Range(0, 1)] public float MediumAmount;
    [Min(0)] public float MediumDuration;
    [Range(0, 1)] public float StrongAmount;
    [Min(0)] public float StrongDuration;

    public override void PickUpEffect()
    {
        var (amount, duration) = GetItemData();
        ServiceLocator.Get<UIBattle>().ModifyVisionBlockerAlpha(amount, duration);
        FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_CAT_POSITIVE_EVENT);
    }

    private (float, float) GetItemData()
    {
        switch (Intensity)
        {
            case ItemIntensity.Weak:
                return (WeakAmount, WeakDuration);
            case ItemIntensity.Medium:
                return (MediumAmount, MediumDuration);
            case ItemIntensity.Strong:
                return (StrongAmount, StrongDuration);
            default:
                throw new InvalidOperationException($"Invalid item intensity: {Intensity}");
        }
    }
}
