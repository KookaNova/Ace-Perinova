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

        protected override void Activate() {
            _in = GetComponent<InputInterpreter>();
        }

        private void Update() {
            if(_in.pIsFiring){
                StartCoroutine(UsePrimaryWeapon());
            }
            if(_in.sIsFiring){
                StartCoroutine(UseSecondaryWeapon());
            }
        }

        protected override void Movement(){
            if(_in == null)return; //if no input interpreter is found, don't use inputs.

            //find speed
            speedTarget += _in.thrust;
            speedTarget -= _in.brake;


            Vector3 torque = new Vector3(_in.torque.y * (m_pitch * 10), _in.yaw * (m_yaw * 10), _in.torque.x * (m_roll * 10));
            transform.Rotate(torque * Time.deltaTime);

            //Add Torque
            //rb.AddRelativeTorque(_in.torque.y * (m_pitch * 10) * Time.fixedDeltaTime, _in.yaw * (m_yaw * 10) * Time.fixedDeltaTime, _in.torque.x * (m_roll * 10) * Time.fixedDeltaTime, ForceMode.Acceleration);
            //torque.y = pitch, torque.x = roll
        }

        
    }
}

