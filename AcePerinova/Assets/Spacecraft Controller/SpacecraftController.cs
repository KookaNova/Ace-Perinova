using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.Selectables;

namespace AcePerinova.Controller{
    /// <summary>
    /// Generic class that handles basic ship functions and movement
    /// </summary>
    public abstract class SpacecraftController : MonoBehaviour
    {
        public ShipObject ship;

        protected float maxSpeed = 120, currentSpeed, minSpeed = 0, speedTarget;

        #region Ship Data
        protected float acceleration, m_pitch, m_yaw, m_roll;

        #endregion

        protected Rigidbody rb;

        private void Awake() {
            rb = GetComponentInChildren<Rigidbody>();
            Activate();
            LoadShipData();
        }
        private void LoadShipData(){
            acceleration = ship.acceleration;
            m_pitch = ship.pitch;
            m_yaw = ship.yaw;
            m_roll = ship.roll;
        }

        protected virtual void Activate(){}

        private void FixedUpdate() {
            Movement();
        }

        protected virtual void Movement(){}

        

        
    }

}

