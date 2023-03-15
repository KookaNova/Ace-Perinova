using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Takes the camera input from the Input Interpreter and turns it into 
    /// rotations on the gameobject this component is attached to.
    /// </summary>
    
    public enum CameraStates {
        PlayerControlled,
        TargetFollow,
        WeaponTracking

    }
    public class CameraController : MonoBehaviour
    {
        InputInterpreter _in;
        TargetingSystem ts;
        SpacecraftController sc;
        CameraStates state, prevState;
        Vector2 rotationInput = Vector2.zero;

        public GameObject[] cameras;
        public GameObject weaponTrackingCamera;
        int cameraIndex = 0;

        private void Awake() {
            _in = GetComponentInParent<InputInterpreter>();
            ts = gameObject.transform.parent?.GetComponentInChildren<TargetingSystem>();
            sc = transform.parent.GetComponentInParent<SpacecraftController>();
            _in.cameraChangedEvent.AddListener(ChangeCamera);
            _in.cameraFollowToggleEvent.AddListener(ToggleFollowCamera);
            _in.cameraWeaponTrackEvent.AddListener(WeaponTrackingCamera);
            _in.cameraWeaponTrackCancelEvent.AddListener(CancelWeaponTracking);
        }

        private void ChangeCamera(){
            cameras[cameraIndex].SetActive(false);
            cameraIndex++;
            if(cameraIndex >= cameras.Length){
                cameraIndex = 0;
            }
            cameras[cameraIndex].SetActive(true);
        }
        private void WeaponTrackingCamera() {
            if(state != CameraStates.WeaponTracking) {
                prevState = state;
                state = CameraStates.WeaponTracking;
                weaponTrackingCamera.SetActive(true);
            }
            else {
                CancelWeaponTracking();
            }
            
        }
        private void CancelWeaponTracking() {
            state = prevState;
            weaponTrackingCamera.SetActive(false);
        }
        private void ToggleFollowCamera(){
            if(ts != null) {
                if (state == CameraStates.PlayerControlled) {
                    state = CameraStates.TargetFollow;
                }
                else {
                    state = CameraStates.PlayerControlled;
                }
            }
        }

        private void LateUpdate() {
            if(_in == null)return;

            switch (state) {
                case CameraStates.TargetFollow:
                    if(ts.currentTarget == null || ts?.currentTarget.gameObject.activeInHierarchy == false){
                        state = CameraStates.PlayerControlled;
                        break;
                    }
                    Vector3 toTarget = ts.currentTarget.transform.position - transform.position; //find ideal forward direction
                    Quaternion targetRot = Quaternion.LookRotation(toTarget, transform.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 100f * Time.fixedDeltaTime);

                    gameObject.transform.localRotation = new Quaternion(
                        Mathf.Clamp(gameObject.transform.localRotation.x, -1f, .3f), 
                        Mathf.Clamp(gameObject.transform.localRotation.y, -.85f, .85f), 
                        Mathf.Clamp(gameObject.transform.localRotation.z, -.1f, .1f), 
                        Mathf.Clamp(gameObject.transform.localRotation.w, -1, 1));
                    
                    break;

                case CameraStates.PlayerControlled:
                    rotationInput.x = _in.cameraInput.x * 150; //Turns input directly into rotation values.
                    rotationInput.y = _in.cameraInput.y * 90;
                    Quaternion targetRotation = Quaternion.Euler(rotationInput.y, rotationInput.x, 0); //y and x are swapped from inputs
                
                    targetRotation.x = Mathf.Clamp(targetRotation.x, -1, .3f); //clamps 1st person limitation. May be adjusted later.
                    gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, 10f * Time.deltaTime); //set rotation
                    break;
                case CameraStates.WeaponTracking:
                    if(sc.activeWeapon == null) {
                        CancelWeaponTracking();
                        break;
                    }

                    weaponTrackingCamera.transform.position = sc.activeWeapon.transform.position - sc.activeWeapon.transform.forward * 3 + Vector3.up + Vector3.right * 0.5f;
                    weaponTrackingCamera.transform.LookAt(sc.activeWeapon.transform);
                    break;
            } 
        }
    }
}
