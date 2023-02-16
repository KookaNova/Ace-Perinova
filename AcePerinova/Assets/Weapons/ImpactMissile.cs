using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    public class ImpactMissile : WeaponComponent
    {
        public float trackingStrength = 3;
        protected override void WeaponAction(){
            rb.AddRelativeForce(0, 0, owner.currentSpeed + force, ForceMode.VelocityChange);
        }

        private void LateUpdate() {
            rb.AddRelativeForce(0, 0, owner.currentSpeed + force, ForceMode.Acceleration);

            if(target == null)return;

            var toTarget = target.transform.position - rb.position;
            var targetRotation = Vector3.RotateTowards(rb.transform.forward, toTarget, trackingStrength, 1);
            rb.transform.rotation = Quaternion.LookRotation(targetRotation);
            //Debug.Log("To Target: " + toTarget.magnitude + " | Target Rotation: " + targetRotation.magnitude);

            


            RaycastHit hit;
            if(Physics.Raycast(this.transform.forward * 2, this.transform.forward, out hit, 2000)){
                if(hit.collider != null){
                    if(hit.collider.gameObject != target.gameObject){
                        target = null;
                        owner.TargetMissed();
                        Debug.Log("Target missed!");
                    }
                }

            }
            //if to Target is too high, we miss? Might be a better miss system.
            
        }

        
    }
}


