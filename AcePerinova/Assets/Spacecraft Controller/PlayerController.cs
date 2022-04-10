using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Takes inputs from InputInterpreter and converts them into movement for a spacecraft.
    /// </summary>
    public class PlayerController : SpacecraftController
    {
        private InputInterpreter _in;

        protected override void Activate() {
            _in = GetComponent<InputInterpreter>();
        }

        protected override void Movement(){
            if(_in == null)return; //if no input interpreter is found, don't use inputs.

            //find speed
            speedTarget += _in.thrust;
            speedTarget -= _in.brake;
            speedTarget = Mathf.Clamp(speedTarget, minSpeed, maxSpeed);
            currentSpeed = Mathf.Lerp(currentSpeed, speedTarget, acceleration * Time.fixedDeltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);
            rb.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);
            //Add Torque
            rb.AddRelativeTorque(_in.torque.y * m_pitch * Time.fixedDeltaTime, _in.yaw * m_yaw * Time.fixedDeltaTime, _in.torque.x * m_roll * Time.fixedDeltaTime, ForceMode.Acceleration);
            //torque.y = pitch, torque.x = roll
        }
    }
}

