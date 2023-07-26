using System.Collections;
using UnityEngine;

namespace AcePerinova.Weapons{
    public class ImpactMissile : WeaponComponent
    {
        public float trackingStrength = .5f;
        bool canMiss = false;
        bool isTracking = false;
        Vector3 forwardForce = Vector3.down;
        protected override void WeaponAction(){
            StartCoroutine(Startup());
            forwardForce = (owner.currentSpeed * Vector3.forward) + (Vector3.down * 2);
        }
        private void Update() {
            var newForce = Vector3.forward * force;
            if(isTracking) forwardForce = Vector3.Slerp(forwardForce, newForce, 4 * Time.deltaTime);
            transform.Translate(forwardForce * Time.deltaTime);

            if (target == null || !isTracking) return;

            var toTarget = target.transform.position - transform.position;
            var targetRotation = Vector3.RotateTowards(transform.forward, toTarget, trackingStrength * Time.deltaTime, 1) * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(targetRotation);

            if (toTarget.magnitude < 5f) canMiss = false;
            Debug.Log(toTarget.magnitude);

            if (Missed() && canMiss) {
                owner.TargetMissed();
                target = null;

            }
        }

        private bool Missed() {
            string message = "";
            Debug.DrawRay(transform.position, transform.forward * 2000, Color.green);
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2000)) {
                if (hit.transform.gameObject == target.gameObject) {
                        return false;
                }
                else {
                    message = $"Hit object is {hit.transform.gameObject} and not equal to target {target.gameObject}";
                }
            }
            else {
                message = "No raycast hit.";
            }
            message = $"Missed: {message}.";
            Debug.LogWarning(message);
            return true;
        }

        private IEnumerator Startup() {
            yield return new WaitForSeconds(colliderDelay/3);
            isTracking = true;
            yield return new WaitForSeconds(colliderDelay);
            canMiss = true;
        }
    }
}


