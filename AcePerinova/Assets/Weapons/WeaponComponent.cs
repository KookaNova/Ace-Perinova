using System.Collections;
using UnityEngine;

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
        public float force = 100;
        public float colliderDelay = 0.1f, activeTime = 6, canUseDelay = 0.25f;
        public int maxUseCount = 20;
        public float reloadTime = 0.5f;
        
        [HideInInspector] public Controller.TargetableObject target = null;
        [HideInInspector] public Controller.SpacecraftController owner;

        protected Rigidbody rb;
        Collider thisCollider;

        public void Activate(){
            rb = GetComponent<Rigidbody>();
            thisCollider = GetComponent<Collider>();
            thisCollider.enabled = false;
            WeaponAction();
            StartCoroutine(StartUp());
            StartCoroutine(Terminate());
        }
        protected virtual void WeaponAction(){}

        protected virtual void OnCollisionEnter(Collision other){
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

        private void EndUse(){
            //GetComponentInChildren<TrailRenderer>().transform.parent = null;
            Destroy(this.gameObject); //we should be pooling, but we need to learn how to use it with Fusion
        }
        


    }   
}

