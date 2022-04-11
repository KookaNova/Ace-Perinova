using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    /// <summary>
    /// This scriptable object modifies the behaviour of a weapon object.
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "Weapon Modifier", fileName = "New Modifier")]
    public class WeaponModifier : ScriptableObject
    {   
        public GameObject startUpFX, trailFX, endFX;
        public AudioClip startSound, activeSound, endSound;

        public float activeTime = 6f;
        [Tooltip("When 'true', the modifier will terminate at the end on collision, despite the time remaining on a delay.")]
        public bool useCollision = true;
        [Tooltip("When 'true', the weapon does not move.")]
        public bool isStationary = false;
        public float moveForce = 100f;
        public bool updateForce = false;
        [Tooltip("When 'true', the weapon does not move.")]
        public bool isTracking = false;
        public float trackingStrength = 1f;

    }
}

