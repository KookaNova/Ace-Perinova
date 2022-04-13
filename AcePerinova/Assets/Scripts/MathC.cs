using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Math used often but not included in MathF.
/// </summary>
static class MathC
{
    /// <summary>
    /// Normalizes a given number and it's maximum between 0 and 1.
    /// </summary>
    public static float NormalizeRange(int max, int current){
        float difference = max - current;
        float amount = 1 - (difference/max);
        return amount;
    }
    /// <summary>
    /// Normalizes a given number and it's maximum between 0 and 1.
    /// </summary>
    public static float NormalizeRange(float max, float current){
        float difference = max - current;
        float amount = 1 - (difference/max);
        return amount;
    }
}
