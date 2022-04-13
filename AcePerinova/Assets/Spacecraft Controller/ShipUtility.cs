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


        public Transform[] primaryWeaponPositions;
        public VisualEffect[] primaryMuzzle;
        public Transform[] secondaryWeaponPositions;
        public VisualEffect[] secondaryMuzzle;

        public GameObject[] cameras;
        int cameraIndex = 0;

        [HideInInspector] public Vector3 targetPosition, actualPosition;

        private void Awake() {
            sc = GetComponentInParent<SpacecraftController>();
            _in = this?.GetComponentInParent<InputInterpreter>();
        }

        private void Update(){
            targetPosition = transform.position + (transform.forward * sc.aimDistance);
            actualPosition = Vector3.Lerp(actualPosition, targetPosition, 15f * Time.deltaTime);

            foreach(var item in primaryWeaponPositions){
                item.LookAt(actualPosition);
            }
            if(_in?.cameraIndex != cameraIndex){
                Camera();
            }

        }

        private void Camera(){
            cameras[cameraIndex].SetActive(false);
            if(_in?.cameraIndex >= cameras.Length){
                _in.cameraIndex = 0;
                cameraIndex = 0;
            }
            else{
                cameraIndex++;
            }
            cameras[cameraIndex].SetActive(true);


        }
    }
}

