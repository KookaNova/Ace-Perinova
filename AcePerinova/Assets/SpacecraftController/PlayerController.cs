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
            if (w_primary.isTrackable) _in.primaryFireStartedEvent.AddListener(FirePrimaryOnce);
            if (w_secondary.isTrackable) _in.secondaryFireStartedEvent.AddListener(FireSecondaryOnce);
        }

        private void LateUpdate() {
            if(_in.pIsFiring && !w_primary.isTrackable){
                StartCoroutine(UsePrimaryWeapon());
            }
            if(_in.sIsFiring && !w_secondary.isTrackable){
                StartCoroutine(UseSecondaryWeapon());
            }
        }

        private void FirePrimaryOnce() {
            StartCoroutine(UsePrimaryWeapon());
        }
        private void FireSecondaryOnce() {
            StartCoroutine(UseSecondaryWeapon());
        }

        protected override void Movement(){
            if(_in == null)return; //if no input interpreter is found, don't use inputs.

            //find speed
            speedTarget += _in.thrust;
            speedTarget -= _in.brake;

            //Add Torque
            int mod = 6;
            rb.AddRelativeTorque(_in.torque.y * (m_pitch * mod) * Time.fixedDeltaTime, _in.yaw * (m_yaw * mod) * Time.fixedDeltaTime, _in.torque.x * (m_roll * mod) * Time.fixedDeltaTime, ForceMode.Acceleration);
            rb.AddRelativeForce(0, _in.torque.y * (m_pitch * mod) * Time.fixedDeltaTime * (-currentSpeed/8), 0, ForceMode.Acceleration); //this eliminates drift
            //torque.y = pitch, torque.x = roll
        }
    }
}

