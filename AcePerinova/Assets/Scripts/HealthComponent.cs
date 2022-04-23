using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    [RequireComponent(typeof(Rigidbody))]
    public class HealthComponent : MonoBehaviour
    {
        Rigidbody rb;
        public float maxHealth, maxShield;
        [SerializeField] float chargeRate, chargeDelay;
        [HideInInspector] public float currentHealth, currentShield;

        #region Events
        public delegate void TakeDamage(string cause, float damage);
        public delegate void ShieldBroken();
        public delegate void Eliminated(string cause);
        public event TakeDamage OnTakeDamage;
        public event ShieldBroken OnShieldBroken;
        public event Eliminated OnEliminate;
        #endregion

        private void OnEnable() {
            currentHealth = maxHealth;
            currentShield = maxShield;
        }

        public void SetDataFromShip(ShipObject ship){
            maxHealth = ship.health;
            maxShield = ship.shields;
            chargeRate = ship.shieldChargeRate;
            chargeDelay = ship.shieldChargeDelay;
            currentHealth = maxHealth;
            currentShield = maxShield;
        }

        /// <summary>
        /// Substracts health by given damage value, and raises the event OnTakeDamage
        /// </summary>
        public void DealDamage(string cause, float damage){

            if(currentShield > 0){
                Debug.LogFormat("Health Component: DealDamage(), {0} dealt {1} damage to shield.", cause, damage);
                float targetShield = currentShield - damage;
                if(targetShield > 0){
                    currentShield = targetShield;
                }
                else{
                    //shield just broke
                    currentShield = 0;
                    currentHealth += (targetShield * 0.66f);
                    if(OnShieldBroken != null) OnShieldBroken();
                }
            }
            else{
                Debug.LogFormat("Health Component: DealDamage(), {0} dealt {1} damage to health.", cause, damage);
                currentHealth -= damage;
            }

            //Death check
            if(currentHealth <= 0){
                currentHealth = 0;
                Eliminate(cause);
            }
            else{
                if(OnTakeDamage != null)OnTakeDamage(cause, damage);
            }

            Debug.LogFormat("Health = {0} | Shields = {1}", currentHealth, currentShield);
            
            
        }

        private void OnCollisionEnter(Collision other) {
            if(other.gameObject.layer != 11){
                DealDamage("Collision", 100);
            }
        }

        private void Eliminate(string cause){
            Debug.LogFormat("Health Component: Eliminated by {0}", cause);
            if(OnEliminate != null)OnEliminate(cause);

        }


    }
}

