using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Takes inputs from InputInterpreter and converts them into movement for a spacecraft.
    /// </summary>
    public class PlayerController : SpacecraftController
    {
        #region Input Data
        public float thrust;
        public float brake;
        public float pitch;
        public float yaw;
        public float roll;
        #endregion

        protected override void Movement()
        {
            //find speed
            speedTarget += thrust;
            speedTarget -= brake;
            speedTarget = Mathf.Clamp(speedTarget, minSpeed, maxSpeed);

            currentSpeed = Mathf.Lerp(currentSpeed, speedTarget, acceleration * Time.fixedDeltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, minSpeed, maxSpeed);

            rb.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);
            rb.AddRelativeTorque(pitch * m_pitch * Time.fixedDeltaTime, yaw * m_yaw * Time.fixedDeltaTime, roll * m_roll * Time.fixedDeltaTime, ForceMode.Acceleration);

            Debug.Log(currentSpeed);
        }
    }
}

