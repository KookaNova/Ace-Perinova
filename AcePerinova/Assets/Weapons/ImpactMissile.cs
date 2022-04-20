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

        private void FixedUpdate() {
            rb.AddRelativeForce(0, 0, owner.currentSpeed + force, ForceMode.Acceleration);

            if(target == null)return;

            var toTarget = target.transform.position - rb.position;
            var targetRotation = Vector3.RotateTowards(rb.transform.forward, toTarget, trackingStrength * Time.fixedDeltaTime, 360);
            rb.transform.rotation = Quaternion.LookRotation(targetRotation);

            Debug.Log("To Target: " + toTarget + " | Target Rotation: " + targetRotation);
            //if to Target is too high, we miss? Might be a better miss system.
            
        }
    }
}

