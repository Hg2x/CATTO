using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Const
{
    public static string FMOD_BGM_EVENT = "event:/BGM";
    public static string FMOD_UI_CLICK_EVENT = "event:/UIClickEvent";
    public static string FMOD_CAT_JUMP_EVENT = "event:/CatJumpEvent";
    public static string FMOD_CAT_POSITIVE_EVENT = "event:/CatPositiveEvent";
    public static string FMOD_CAT_HURT_EVENT = "event:/CatHurtEvent";

    public static Color LightCreme
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Color32 color32 = new(239, 235, 234, 255);
            Color color = new(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
            return color;
        }
    }

    public static Color MediumCreme
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Color32 color32 = new(228, 219, 214, 255);
            Color color = new(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
            return color;
        }
    }

    public static Color LightOrange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Color32 color32 = new(243, 180, 134, 255);
            Color color = new(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
            return color;
        }
    }

    public static Color MediumOrange
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Color32 color32 = new(212, 113, 93, 255);
            Color color = new(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
            return color;
        }
    }

    public static Color DarkPurple
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            Color32 color32 = new(77, 35, 74, 255);
            Color color = new(color32.r / 255f, color32.g / 255f, color32.b / 255f, color32.a / 255f);
            return color;
        }
    }
}

public enum ItemIntensity
{
    Weak = 0,
    Medium = 1,
    Strong = 2
}
