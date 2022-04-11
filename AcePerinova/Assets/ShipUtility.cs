using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace AcePerinova.Selectables{
    /// <summary>
    /// Used for etc. utilities like storing positions and potentially certain sounds or effects
    /// </summary>
    public class ShipUtility : MonoBehaviour
    {
        public Transform[] primaryWeaponPositions;
        public VisualEffect[] primaryMuzzle;
        public Transform[] secondaryWeaponPositions;
        public VisualEffect[] secondaryMuzzle;
    }
}

