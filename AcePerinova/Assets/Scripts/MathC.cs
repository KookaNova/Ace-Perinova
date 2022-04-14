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

    public static Vector3 WorldToHUDSpace(Camera camera, Vector3 worldObjectPosition, Vector3 referenceObjectPosition){
         //Position indicator to the indicated objects screen position
        var screenPosition = camera.WorldToScreenPoint(worldObjectPosition);
        screenPosition.z = Vector3.Distance(referenceObjectPosition, camera.transform.position); //adjust Z position
        var result = camera.ScreenToWorldPoint(screenPosition);
        return result;
        //activeIndicators[i].transform.position = Vector3.Slerp(activeIndicators[i].transform.position, desiredScreenWorldPosition, 60 * Time.deltaTime);

        

    }
}
