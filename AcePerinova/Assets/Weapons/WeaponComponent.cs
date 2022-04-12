using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    /// <summary>
    /// This component makes an object behave like a weapon.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class WeaponComponent : MonoBehaviour
    {
        //Consider including pool information on the specific weapon.
        [SerializeField] WeaponModifier[] modifiers;
        [SerializeField] bool randomizeModifiers = true;

        public string weaponName = "Unnamed Weapon";
        public float fireDelay = .25f;
        [Tooltip("Use ammo amount, or use charge.")]
        public bool useCharge = true;
        /// <summary> Speed at which ammo or charge is used.</summary>
        public float fireUsage = .05f;
        /// <summary> Speed at which reload happens or recharge happens.</summary>
        public float rechargeSpeed = 0.01f;

        public GameObject target = null;


        [HideInInspector] public Controller.SpacecraftController owner;
        Rigidbody rb;
        Collider thisCollider;
        int i = 0;

        public void Activate(){
            rb = GetComponent<Rigidbody>();
            thisCollider = GetComponent<Collider>();
            thisCollider.enabled = false;
            ActivateModifier();
            StartCoroutine(StartUp());

        }

        public void IncrementModifiers(){
            i++;
            if(i < modifiers.Length){
                ActivateModifier();
            }
            else{
                EndUse();
            }

        }

        private void OnCollisionEnter(Collision other) {
            if(modifiers[i].collisionVFX != null){
               Instantiate(modifiers[i].collisionVFX, this.transform.position, this.transform.rotation, null);
           }
            if(modifiers[i].terminateOnCollision){
                TerminateModifier();
            }
            
        }

        private void ActivateModifier(){
           // Instantiate(modifiers[i].startUpFX, this.transform.position, this.transform.rotation, null);
           if(randomizeModifiers){
               i = Random.Range(0, modifiers.Length - 1);
           }
           if(modifiers[i].startUpVFX != null){
               Instantiate(modifiers[i].startUpVFX, this.transform.position, this.transform.rotation, null);
           }
            StartCoroutine(ModifierTimer());
            if(!modifiers[i].isStationary)
            rb.velocity = (transform.forward * modifiers[i].moveForce);
            
        }
        private void TerminateModifier(){
            if(randomizeModifiers){
                EndUse();
                return;
            }
            //Instantiate(modifiers[i].endFX, null);
            IncrementModifiers();
        }

        private void Update() {
            if(modifiers[i].updateForce && !modifiers[i].isStationary){
                rb.AddRelativeForce(0, 0, owner.currentSpeed + modifiers[i].moveForce, ForceMode.Acceleration);
            }
            if(modifiers[i].isTracking && target != null)
            {
                var toTarget = target.transform.position - rb.position;
                var targetRotation = Vector3.RotateTowards(rb.transform.forward, toTarget, modifiers[i].trackingStrength * Time.fixedDeltaTime, 1080);
                rb.transform.rotation = Quaternion.LookRotation(targetRotation);

                Debug.Log("To Target: " + toTarget + " | Target Rotation: " + targetRotation);
                //if to Target is too high, we miss? Might be a better miss system.
            }
            
            
        }
        private IEnumerator StartUp(){
            yield return new WaitForSecondsRealtime(0.01f);
            thisCollider.enabled = true;
        }

        private IEnumerator ModifierTimer(){
            yield return new WaitForSecondsRealtime(modifiers[i].activeTime);
            TerminateModifier();
        }

        private void EndUse(){
            Destroy(this.gameObject); //we should be pooling, but we need to learn how to use it with Fusion
        }
        


    }   
}

