using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Math used often but not included in MathF.
/// </summary>
static class MathC
{
    /// <summary>
    /// Normalizes a given value and its minimum and maximum between 0 and 1.
    /// </summary>
    public static float NormalizeRange01(float min, float max, float value){
        return (value - min)/(max - min);
    }
    /// <summary>
    /// Normalizes a given value and its minimum and maximum between -1 and 1.
    /// </summary>
    public static float NormalizeRangeNegative1Positive1(float min, float max, float value){
	    return ((value - min)/(max - min)*2) - 1;
    }
    /// <summary>
    /// Normalizes a given value and its min and max between any new minimum and maximum.
    /// </summary>
    public static float NormalizeRangeAnyMinMax(float min, float max, float value, float newMin, float newMax){
        return (((value - min)/(max - min))*(newMax - newMin)) + newMin;
    }

    public static Vector3 WorldToHUDSpace(Camera camera, Vector3 worldObjectPosition, Vector3 referenceObjectPosition){
         //Position indicator to the indicated objects screen position
        var screenPosition = camera.WorldToScreenPoint(worldObjectPosition); //convert the object from world to the point on the camera's screen.
        if(screenPosition.z > 0){
            screenPosition.z = Vector3.Distance(referenceObjectPosition, camera.transform.position); //adjust Z to be the correct distance from the camera, based on the given canvas
        }
        else{
            screenPosition.z = -Vector3.Distance(referenceObjectPosition, camera.transform.position); //needed to prevent HUD from appearing behind the player
        }
       
        var result = camera.ScreenToWorldPoint(screenPosition);
        return result;
        //activeIndicators[i].transform.position = Vector3.Slerp(activeIndicators[i].transform.position, desiredScreenWorldPosition, 60 * Time.deltaTime);
    }
}
