using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using AcePerinova.Controller;

namespace AcePerinova.Selectables{
    /// <summary>
    /// Used for etc. utilities like storing positions and potentially certain sounds or effects
    /// </summary>
    public class ShipUtility : MonoBehaviour
    {
        SpacecraftController sc;
        InputInterpreter _in;

        //weapons
        public Transform[] primaryWeaponPositions;
        public VisualEffect[] primaryMuzzle;
        public Transform[] secondaryWeaponPositions;
        public VisualEffect[] secondaryMuzzle;
        [HideInInspector] public Vector3 centerPosition, aimPosition;

        private void Awake() {
            sc = GetComponentInParent<SpacecraftController>();
            _in = this?.GetComponentInParent<InputInterpreter>();
        }

        private void Update(){
            centerPosition = transform.position + (transform.forward * sc.aimDistance);
            aimPosition = Vector3.Lerp(aimPosition, centerPosition, 15f * Time.deltaTime);

            foreach(var item in primaryWeaponPositions){
                item.LookAt(aimPosition);
            }

        }


    }
}

