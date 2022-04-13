using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Takes inputs from InputInterpreter and converts them into movement for a spacecraft.
    /// </summary>
    public class PlayerController : SpacecraftController
    {
        [HideInInspector] public InputInterpreter _in;

        int currentCam;

        protected override void OnActivate() {
            _in = GetComponent<InputInterpreter>();
        }

        private void Update() {
            if(_in.pIsFiring){
                StartCoroutine(UsePrimaryWeapon());
            }
        }

        protected override void Movement(){
            if(_in == null)return; //if no input interpreter is found, don't use inputs.

            //find speed
            speedTarget += _in.thrust;
            speedTarget -= _in.brake;
            
            //Add Torque
            rb.AddRelativeTorque(_in.torque.y * (m_pitch * 10) * Time.fixedDeltaTime, _in.yaw * (m_yaw * 10) * Time.fixedDeltaTime, _in.torque.x * (m_roll * 10) * Time.fixedDeltaTime, ForceMode.Acceleration);
            //torque.y = pitch, torque.x = roll
        }

        
    }
}

