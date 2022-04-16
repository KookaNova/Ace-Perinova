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
        public string playerName = "Unknown";
        public int team = 0;
        public ShipObject ship;
        
        [HideInInspector] public float currentSpeed, speedTarget;

        #region Ship Data
        protected float 
            acceleration, 
            m_pitch, 
            m_yaw, 
            m_roll;

        protected ShipUtility shipUtility;
        protected Rigidbody rb;
        protected Weapons.WeaponComponent w_primary, w_secondary;
        [HideInInspector] public int aimDistance;
        #endregion

        #region Input
        bool canUsePrimaryWeapon = true, canUseSecondaryWeapon = true;
        Transform[] primaryWeaponPositions, secondaryWeaponPositions;
        int p_index = 0, s_index = 0;

        [HideInInspector] public TargetableObject lockedTarget;
        #endregion

        #region On Spawn
        private void Awake() {
            Activate(); //Activate is separate from awake in case I need more control later over what activate the player.
        }

        private void Activate(){
            shipUtility = Instantiate(ship.shipUtility, this.transform);
            rb = shipUtility.GetComponentInChildren<Rigidbody>();
            
            LoadShipData();
            OnActivate();
        }
        private void LoadShipData(){
            acceleration = ship.acceleration;
            m_pitch = ship.pitch;
            m_yaw = ship.yaw;
            m_roll = ship.roll;

            w_primary = ship.primary;
            w_secondary = ship.secondary;
            primaryWeaponPositions = shipUtility.primaryWeaponPositions;
            secondaryWeaponPositions = shipUtility.secondaryWeaponPositions;
            aimDistance = ship.aimDistance;

        }
        
        protected virtual void OnActivate(){}
        protected virtual void Movement(){}

        #endregion

        private void FixedUpdate() {
            Movement(); //finds target speed and set rotation
            speedTarget = Mathf.Clamp(speedTarget, ship.minSpeed, ship.maxSpeed);
            currentSpeed = Mathf.Lerp(currentSpeed, speedTarget, (acceleration/50) * Time.fixedDeltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, ship.minSpeed, ship.maxSpeed);
            rb.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);
            
        }

        protected IEnumerator UsePrimaryWeapon(){
            if(canUsePrimaryWeapon){
                canUsePrimaryWeapon = false;
                Transform t = primaryWeaponPositions[p_index];
                var w = Instantiate(w_primary, t.position, t.rotation, null);
                w.owner = this;
                w.target = lockedTarget;
                w.Activate();
                shipUtility?.primaryMuzzle[p_index].Play();
                p_index++;
                if(p_index >= primaryWeaponPositions.Length){
                    p_index = 0;
                }
                yield return new WaitForSecondsRealtime(w_primary.fireDelay);
                canUsePrimaryWeapon = true;
            }
            
        }

         protected IEnumerator UseSecondaryWeapon(){
            if(canUseSecondaryWeapon){
                canUseSecondaryWeapon = false;
                Transform t = secondaryWeaponPositions[s_index];
                var w = Instantiate(w_secondary, t.position, t.rotation, null);
                w.owner = this;
                w.target = lockedTarget;
                w.Activate();
                shipUtility?.secondaryMuzzle[s_index]?.Play();
                s_index++;
                if(s_index >= secondaryWeaponPositions.Length){
                    s_index = 0;
                }
                yield return new WaitForSecondsRealtime(w_secondary.fireDelay);
                canUseSecondaryWeapon = true;
            }
            
        }

        
        
    }

}

