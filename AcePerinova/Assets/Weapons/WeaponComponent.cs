using System.Collections;
using UnityEngine;
using AcePerinova.Controller;

namespace AcePerinova.Weapons{
    /// <summary>
    /// This component makes an object behave like a weapon.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public abstract class WeaponComponent : MonoBehaviour
    {
        //Consider including pool information on the specific weapon.

        public string weaponName = "Unnamed Weapon";
        public GameObject collisionVFX;
        public float damageValue = 100;
        public float force = 100;
        public float colliderDelay = 0.1f, activeTime = 6, canUseDelay = 0.25f;
        public int maxUseCount = 20;
        public float reloadTime = 0.5f;
        public bool isTrackable = false;
        
        [HideInInspector] public TargetableObject target = null;
        [HideInInspector] public SpacecraftController owner;

        Collider thisCollider;

        public void Activate(){
            thisCollider = GetComponent<Collider>();
            thisCollider.enabled = false;
            force += owner.currentSpeed;
            WeaponAction();
            StartCoroutine(StartUp());
            StartCoroutine(Terminate());
        }
        protected virtual void WeaponAction(){}

        protected virtual void OnCollisionEnter(Collision other){
            HealthComponent hitHealth = other.collider?.attachedRigidbody?.GetComponent<HealthComponent>();
            if(hitHealth == null){
                if(other.collider?.attachedRigidbody?.gameObject?.GetComponent<TargetableObject>()){
                    owner?.TargetHit();
                }
            }
            if(hitHealth != null){
                hitHealth.DealDamage(owner, weaponName, damageValue);
            }
            if(collisionVFX != null){
                Instantiate(collisionVFX, transform.position, transform.rotation);
            }
            EndUse();
            
        }

        private IEnumerator StartUp(){
            yield return new WaitForSecondsRealtime(colliderDelay);
            thisCollider.enabled = true;
        }

        private IEnumerator Terminate(){
            yield return new WaitForSecondsRealtime(activeTime);
            EndUse();
        }

        protected virtual void EndUse(){
            //GetComponentInChildren<TrailRenderer>().transform.parent = null;
            if(owner.activeWeapon == this.gameObject) {
                owner.activeWeapon = null;
            }
            GetComponentInChildren<TrailRenderer>().transform.SetParent(null);
            Destroy(this.gameObject); //we should be pooling, but we need to learn how to use it with Fusion
        }
        


    }   
}

