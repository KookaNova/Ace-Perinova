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
        bool targetTracking = false;
        Vector2 rotationInput = Vector2.zero;

        private void Awake() {
            _in = GetComponentInParent<InputInterpreter>();
        }

        public void ChangeCamera(){
            //toggle bool using 1st person
            //activate and deactivate appropriate cameras
            //on weapon controller, consider allowing a missile follow when holding down the missile button.
        }

        private void LateUpdate() {
            if(_in == null)return;

            switch (targetTracking){
                case true:
                    //Lock camera to target, ignoring imputs
                    break;
                case false:
                    rotationInput.x = _in.cameraInput.x * 150; //Turns input directly into rotation values.
                    rotationInput.y = _in.cameraInput.y * 90;
                    Quaternion targetRotation = Quaternion.Euler(rotationInput.y, rotationInput.x, 0); //y and x are swapped from inputs
                
                    targetRotation.x = Mathf.Clamp(targetRotation.x, -1, .3f); //clamps 1st person limitation. May be adjusted later.
                    gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, targetRotation, 15f * Time.deltaTime); //set rotation
                    break;
            } 
        }
    }
}
