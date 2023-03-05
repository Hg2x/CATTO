using ICKT.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishData", menuName = "ScriptableObject/ItemData/FishData", order = 0)]
public class FishData : ItemData
{
    [Header("Fish Specific")]
    public float WeakSpeedChange;
    [Min(0)] public float WeakDuration;
    public float MediumSpeedChange;
    [Min(0)] public float MediumDuration;
    public float StrongSpeedChange;
    [Min(0)] public float StrongDuration;

    public override void PickUpEffect()
    {
        var (speedChangeAmount, duration) = GetItemData();
        ServiceLocator.Get<LevelManager>().ChangePlayerSpeed(speedChangeAmount, duration);
        if (Intensity == ItemIntensity.Strong)
        {
            FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_CAT_HURT_EVENT);
        }
        else
        { 
            FMODUnity.RuntimeManager.PlayOneShot(Const.FMOD_CAT_POSITIVE_EVENT);
        }
    }

    private (float, float) GetItemData()
    {
        switch (Intensity)
        {
            case ItemIntensity.Weak:
                return (WeakSpeedChange, WeakDuration);
            case ItemIntensity.Medium:
                return (MediumSpeedChange, MediumDuration);
            case ItemIntensity.Strong:
                return (StrongSpeedChange, StrongDuration);
            default:
                throw new InvalidOperationException($"Invalid item intensity: {Intensity}");
        }
    }
}
