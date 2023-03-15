using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// Generic class that handles basic ship functions and movement
    /// </summary>
    public abstract class SpacecraftController : MonoBehaviour
    {
        #region Player
        public string playerName = "Unknown";
        public int team = 0;
        public ShipObject ship;
        #endregion
        
        #region Utility
        [HideInInspector] public float currentSpeed, speedTarget;
        bool isAwaitingRespawn = false;
        #endregion

        #region Ship Data
        protected float 
            acceleration, 
            m_pitch, 
            m_yaw, 
            m_roll;

        protected ShipUtility shipUtility;
        protected HealthComponent hc;
        protected Rigidbody rb;
        protected Weapons.WeaponComponent w_primary, w_secondary;
        public GameObject activeWeapon = null;
        private int p_maxUse, s_maxUse;
        private float p_reloadTime, s_reloadTime;
        [HideInInspector] public int aimDistance;
        #endregion

        #region Input
        bool canUsePrimaryWeapon = true, canUseSecondaryWeapon = true;
        #endregion

        #region Weapons
        
        Transform[] primaryWeaponPositions, secondaryWeaponPositions;
        int p_index = 0, s_index = 0, p_used = 0, s_used = 0;
        [HideInInspector] public TargetableObject lockedTarget;

        #endregion

        #region Events
        public delegate void PrimaryFire(int index);
        public delegate void SecondaryFire(int index);
        public event PrimaryFire OnPrimaryFire;
        public event SecondaryFire OnSecondaryFire;
        public delegate void MissedTarget();
        public delegate void HitTarget();
        public delegate void EliminatedTarget();
        public event MissedTarget OnTargetMissed;
        public event HitTarget OnTargetHit;
        public event EliminatedTarget OnTargetEliminated;

        #endregion

        #region On Spawn
        private void OnEnable() {
            shipUtility = Instantiate(ship.shipUtility, this.transform);
            rb = shipUtility.GetComponentInChildren<Rigidbody>();
            hc = shipUtility.gameObject?.GetComponent<HealthComponent>();
            LoadShipData();
            Activate();
            SpawnSpacecraft();
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
            p_maxUse = w_primary.maxUseCount;
            p_reloadTime = w_primary.reloadTime;
            s_maxUse = w_secondary.maxUseCount;
            s_reloadTime = w_secondary.reloadTime;

            if(hc != null){
                hc.SetDataFromShip(ship);
            }
        }
        
        protected virtual void Activate(){}
        protected virtual void Movement(){}

        #endregion

        private void FixedUpdate() {
            Movement(); //finds target speed and set rotation
            speedTarget = Mathf.Clamp(speedTarget, ship.minSpeed, ship.maxSpeed);
            currentSpeed = Mathf.Lerp(currentSpeed, speedTarget, (acceleration/50) * Time.fixedDeltaTime);
            currentSpeed = Mathf.Clamp(currentSpeed, ship.minSpeed, ship.maxSpeed);
            rb.AddRelativeForce(Vector3.forward * currentSpeed, ForceMode.Acceleration);
            
        }

        #region Weapons
        protected virtual IEnumerator UsePrimaryWeapon(){
            if(canUsePrimaryWeapon){
                if(p_used == 0){
                    StartCoroutine(ReloadPrimaryWeapon());
                }
                p_used++;
                canUsePrimaryWeapon = false;
                Transform t = primaryWeaponPositions[p_index];
                var w = Instantiate(w_primary, t.position, t.rotation, null);
                w.owner = this;
                w.target = lockedTarget;
                if(w.isTrackable)activeWeapon = w.gameObject;
                w.Activate();
                shipUtility?.primaryMuzzle[p_index].Play();
                if(OnPrimaryFire != null)OnPrimaryFire(p_index);
                p_index++;
                if(p_index >= primaryWeaponPositions.Length){
                    p_index = 0;
                }
                
                yield return new WaitForSecondsRealtime(w_primary.canUseDelay);
                canUsePrimaryWeapon = true;
            }
            
        }

         protected virtual IEnumerator UseSecondaryWeapon(){
            if(canUseSecondaryWeapon){
                if(s_used == 0){
                    StartCoroutine(ReloadSecondaryWeapon());
                }
                s_used++;
                canUseSecondaryWeapon = false;
                Transform t = secondaryWeaponPositions[s_index];
                var w = Instantiate(w_secondary, t.position, t.rotation, null);
                w.owner = this;
                w.target = lockedTarget;
                if (w.isTrackable) activeWeapon = w.gameObject;
                w.Activate();
                shipUtility?.secondaryMuzzle[s_index]?.Play();
                if(OnSecondaryFire != null)OnSecondaryFire(s_index);
                s_index++;
                if(s_index >= secondaryWeaponPositions.Length){
                    s_index = 0;
                }
                yield return new WaitForSecondsRealtime(w_secondary.canUseDelay);
                
                canUseSecondaryWeapon = true;
            }
            
        }

        protected IEnumerator ReloadPrimaryWeapon(){
            yield return new WaitForSecondsRealtime(p_reloadTime);
            p_used--;
            if(p_used > 0){
                StartCoroutine(ReloadPrimaryWeapon());
            }
        }
        protected IEnumerator ReloadSecondaryWeapon(){
            yield return new WaitForSecondsRealtime(s_reloadTime);
            s_used--;
            if(s_used > 0){
                StartCoroutine(ReloadSecondaryWeapon());
            }
        }
        public void TargetMissed(){
            if(OnTargetMissed != null)OnTargetMissed();
        }
        public void TargetHit(){
            if(OnTargetHit != null)OnTargetHit();
        }
        public void TargetEliminated(){
            if(OnTargetEliminated != null)OnTargetEliminated();
        }
        #endregion

        private void Eliminate(string cause){
            isAwaitingRespawn = true;
            if(hc != null){
                hc.OnEliminate -= Eliminate;
            }

            StartCoroutine(RespawnTimer(3));

            shipUtility.gameObject.SetActive(false);

        }

        private IEnumerator RespawnTimer(int seconds){
            yield return new WaitForSecondsRealtime(seconds);
            if(isAwaitingRespawn) SpawnSpacecraft();

        }

        private void SpawnSpacecraft(){
            isAwaitingRespawn = false;
            if(hc != null){
                hc.OnEliminate += Eliminate;
            }
            shipUtility.gameObject.SetActive(true);
        }
    }
}

