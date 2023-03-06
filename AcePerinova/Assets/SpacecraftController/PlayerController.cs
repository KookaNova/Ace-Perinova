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
        int divider = 10;
        float torqueSpeeds = 2;

        protected override void Activate() {
            _in = GetComponent<InputInterpreter>();
        }

        protected override void Update() {
            base.Update();
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


            //Vector3 inputs = new Vector3(_in.torque.y * (m_pitch * 10) * Time.deltaTime, _in.yaw * (m_yaw * 10) * Time.deltaTime, _in.torque.x * (m_roll * 10) * Time.deltaTime);
            currentPitch = Mathf.Lerp(currentPitch, _in.torque.y * (m_pitch / divider), torqueSpeeds * Time.deltaTime);
            currentYaw = Mathf.Lerp(currentYaw, _in.yaw *(m_yaw / divider), torqueSpeeds * Time.deltaTime);
            currentRoll = Mathf.Lerp(currentRoll, _in.torque.x * (m_roll / divider), torqueSpeeds * Time.deltaTime);
            
            if(_in.torque.y == 0) {
                currentPitch = Mathf.Lerp(currentPitch, 0, torqueSpeeds * 2 * Time.deltaTime);
            }
            if (_in.yaw == 0) {
                currentYaw = Mathf.Lerp(currentYaw, 0, torqueSpeeds * 2 * Time.deltaTime);
            }
            if (_in.torque.x == 0) {
                currentRoll = Mathf.Lerp(currentRoll, 0, torqueSpeeds * 2 * Time.deltaTime);
            }
            
            currentPitch = Mathf.Clamp(currentPitch, -m_pitch / divider, m_pitch / divider);
            currentYaw = Mathf.Clamp(currentYaw, -m_yaw / divider, m_yaw / divider);
            currentRoll = Mathf.Clamp(currentRoll, -m_roll / divider, m_roll / divider);

            Vector3 rot = new Vector3(currentPitch, currentYaw, currentRoll);

            transform.Rotate(rot);

            //Add Torque
            //rb.AddRelativeTorque(_in.torque.y * (m_pitch * 10) * Time.fixedDeltaTime, _in.yaw * (m_yaw * 10) * Time.fixedDeltaTime, _in.torque.x * (m_roll * 10) * Time.fixedDeltaTime, ForceMode.Acceleration);
            //torque.y = pitch, torque.x = roll
        }

        
    }
}

