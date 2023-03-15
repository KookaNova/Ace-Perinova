using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    public class ImpactMissile : WeaponComponent
    {
        public float trackingStrength = .5f;
        protected override void WeaponAction(){
        }

        private void Update() {
            var forwardForce = Vector3.forward * force * Time.deltaTime;
            transform.Translate(forwardForce, Space.Self);
        }

        private void LateUpdate() {

            if(target == null)return;

            var toTarget = target.transform.position - transform.position;
            var targetRotation = Vector3.RotateTowards(transform.forward, toTarget, 0.05f, 1) * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(targetRotation);

            if (Missed()) {
                target = null;
                owner.TargetMissed();
                Debug.Log("Target missed!");
            }
            
        }

        private bool Missed() {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * 2000, Color.green);
            if (Physics.SphereCast(this.transform.forward,0.25f, this.transform.forward, out hit, 2000)) {
                if (hit.collider != null) {
                    if (hit.collider.gameObject != target.gameObject) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}


