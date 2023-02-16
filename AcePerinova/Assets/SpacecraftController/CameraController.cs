using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Takes the camera input from the Input Interpreter and turns it into 
    /// rotations on the gameobject this component is attached to.
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        InputInterpreter _in;
        TargetingSystem ts;
        bool targetFollowCamera = false;
        Vector2 rotationInput = Vector2.zero;

        public GameObject[] cameras;
        int cameraIndex = 0;

        private void Awake() {
            _in = GetComponentInParent<InputInterpreter>();
            ts = gameObject.transform.parent?.GetComponentInChildren<TargetingSystem>();

            _in.cameraChangedEvent.AddListener(ChangeCamera);
            _in.cameraFollowToggleEvent.AddListener(ToggleFollowCamera);
        }

        private void ChangeCamera(){
            cameras[cameraIndex].SetActive(false);
            cameraIndex++;
            if(cameraIndex >= cameras.Length){
                cameraIndex = 0;
            }
            cameras[cameraIndex].SetActive(true);
        }

        private void ToggleFollowCamera(){
            if(ts != null)
            targetFollowCamera = !targetFollowCamera;
        }

        private void LateUpdate() {
            if(_in == null)return;

            switch (targetFollowCamera){
                case true:
                    if(ts.currentTarget == null || ts?.currentTarget.gameObject.activeInHierarchy == false){
                        targetFollowCamera = false;
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

                case false:
                    rotationInput.x = _in.cameraInput.x * 150; //Turns input directly into rotation values.
                    rotationInput.y = _in.cameraInput.y * 90;
                    Quaternion targetRotation = Quaternion.Euler(rotationInput.y, rotationInput.x, 0); //y and x are swapped from inputs
                
                    targetRotation.x = Mathf.Clamp(targetRotation.x, -1, .3f); //clamps 1st person limitation. May be adjusted later.
                    gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, 10f * Time.deltaTime); //set rotation
                    break;
            } 
        }
    }
}
