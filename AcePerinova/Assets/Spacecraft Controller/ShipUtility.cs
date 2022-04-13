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
        Controller.SpacecraftController sc;

        public Transform[] primaryWeaponPositions;
        public VisualEffect[] primaryMuzzle;
        public Transform[] secondaryWeaponPositions;
        public VisualEffect[] secondaryMuzzle;
        public Vector3 aimPosition;

        private void Awake() {
            sc = GetComponentInParent<Controller.SpacecraftController>();
            GetComponent<Controller.HUDController>().ship = this; 
        }

        private void Update(){
            aimPosition = transform.forward * sc.aimDistance;
            foreach(var item in primaryWeaponPositions){
                var toTarget = aimPosition - item.position;
                var targetRotation = Vector3.RotateTowards(item.forward, toTarget, 50 * Time.deltaTime, 1080);
                item.rotation = Quaternion.LookRotation(targetRotation);
            }
        }
    }
}

