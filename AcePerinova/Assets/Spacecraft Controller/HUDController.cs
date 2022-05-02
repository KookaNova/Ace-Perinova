using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AcePerinova.Utilities;
using System;

namespace AcePerinova.Controller
{
    public class HUDController : MonoBehaviour
    {
        //Non-serialized fields
        ShipUtility ship;
        Controller.PlayerController sc;
        GameManagement.GameManager gm;
        HealthComponent hc;
        TargetingSystem ts;
        int team;
        
        [Header("Movement and Health")]
        [SerializeField] Canvas overlayHud;
        [SerializeField] Image[] thrustImage, speedImage, healthImage, shieldImage;
        [SerializeField] Text[] thrustText, speedText, healthText, shieldText;
        [SerializeField] Image orientationImage;

        [Header("Alerts & Messages")]
        [SerializeField] GameObject[] missileWarning;
        [SerializeField] GameObject[] takingDamage, lowHealth;
        [SerializeField] GameObject missed, hit, eliminated;
        Coroutine missedTimer, hitTimer, eliminatedTimer;

        //Weapons 
        [Header("Weapons")]
        [SerializeField] GameObject targetPointer;
        [SerializeField] Image weaponReticle, centerPositionReticle, lockOnIndicator; //Maybe this can be swapped by the weapon choice
        [SerializeField] IndicatorComponent indicatorPrefab;
        List<IndicatorComponent> indicators = new List<IndicatorComponent>();
        

        public void Awake(){
            gm = FindObjectOfType<GameManagement.GameManager>();
            ship = this?.GetComponentInParent<ShipUtility>();
            sc = this?.GetComponentInParent<Controller.PlayerController>();
            hc = this?.GetComponent<HealthComponent>();
            ts = this?.GetComponent<TargetingSystem>();
            team = sc.team;
            if(overlayHud == null){
                Debug.LogWarning("HUDController: No overlayHud canvas has been s elected in the inspector.");
            }
            overlayHud.worldCamera = Camera.main;
        }
        private void OnEnable() {
            missed.SetActive(false);
            hit.SetActive(false);
            eliminated.SetActive(false);

            foreach (var alerts in missileWarning){
                alerts.SetActive(false);
            }
            foreach (var alerts in takingDamage){
                alerts.SetActive(false);
            }
            foreach (var alerts in lowHealth){
                alerts.SetActive(false);
            }

            if(sc != null){
                sc.OnTargetMissed += TargetMissed;
                sc.OnTargetHit += TargetHit;
                sc.OnTargetEliminated += TargetEliminated;
            }
            
        }
        private void OnDisable() {
            StopAllCoroutines();
            if(sc != null){
                sc.OnTargetMissed -= TargetMissed;
                sc.OnTargetHit -= TargetHit;
                sc.OnTargetEliminated -= TargetEliminated;
            }
            
        }

        private void LateUpdate() {
            Fills();//Fill basic ship info like speed, health, etc.

            //Target info
            if(indicators.Count != gm.allTargets.Count){
                CreateTargetIndicators();
            }
            if(gm.allTargets.Count > 0){
                PositionIndicators();
            }
            //Weapon reticle positioning
            if(ship != null){
                AimWeaponReticle();
            }
            if(orientationImage.IsActive()){
                UpdateRoll();
            }
            //target locking
            if(ts.isLocking){
                LockOnIndicator();
            }
            else{
                if(lockOnIndicator.gameObject.activeSelf == true) lockOnIndicator.gameObject.SetActive(false);
            }
        }

        private void Fills(){
            foreach (var image in thrustImage){
                image.fillAmount = MathC.NormalizeRange01(0, sc.ship.maxSpeed, sc.speedTarget);
            }
            foreach (var text in thrustText){
                text.text = sc.speedTarget.ToString("[00.00]");
            }
            foreach (var image in speedImage){
                image.fillAmount = MathC.NormalizeRange01(0, sc.ship.maxSpeed, sc.currentSpeed);
            }
            foreach (var text in speedText){
                text.text = sc.currentSpeed.ToString("[00.00]");
            }
            foreach (var image in healthImage){
                image.fillAmount = MathC.NormalizeRange01(0, hc.maxHealth, hc.currentHealth);
            }
            foreach (var text in healthText){
                text.text = hc.currentHealth.ToString("[000.00]");
            }
            foreach (var image in shieldImage){
                image.fillAmount = MathC.NormalizeRange01(0, hc.maxShield, hc.currentShield);
            }
            foreach (var text in shieldText){
                text.text = hc.currentShield.ToString("[000.00]");
            }
        }

        private void UpdateRoll(){
            float roll = -ship.GetComponent<Rigidbody>().rotation.eulerAngles.z;
            orientationImage.transform.rotation = Quaternion.Euler(orientationImage.transform.rotation.eulerAngles.x,  orientationImage.transform.rotation.eulerAngles.y, 0);

            orientationImage.transform.position = centerPositionReticle.transform.position;
        }

        #region TargetStatus
        private void TargetMissed(){
            if(missedTimer != null)StopCoroutine(missedTimer);
            missedTimer = StartCoroutine(MessageTimer(missed, 1));
        }
        private void TargetHit(){
            if(hitTimer != null)StopCoroutine(hitTimer);
            hitTimer = StartCoroutine(MessageTimer(hit, 1));
        }
        private void TargetEliminated(){
            if(eliminatedTimer != null)StopCoroutine(eliminatedTimer);
            eliminatedTimer = StartCoroutine(MessageTimer(eliminated, 2));
        }

        private IEnumerator MessageTimer(GameObject messageObject, int seconds){
            messageObject.SetActive(true);
            yield return new WaitForSecondsRealtime(seconds);
            messageObject.SetActive(false);

        }
        #endregion

        #region Alerts
        private void MissileWarning(){

        }
        private void TakingDamage(){

        }
        private void LowHealth(){

        }

        #endregion

        #region Targeting and Weapons
        public void CreateTargetIndicators(){
            foreach(var item in indicators){
                Destroy(item.gameObject);
            }
            indicators.Clear();
            for(int i = 0; i < gm.allTargets.Count; i++){
                TargetableObject target = gm.allTargets[i];
                var ind = Instantiate(indicatorPrefab, overlayHud.transform);
                ind.targetableObject = target;
                ind.targetingSystem = ts;
                indicators.Add(ind);
                if(target.team != 2){
                    if(target.team == team){
                        ind.color = ColorPaletteUtility.friendly;
                    }
                    else{
                        ind.color = ColorPaletteUtility.enemy;

                    }
                }
                else{
                    ind.color = ColorPaletteUtility.global;
                }
                ind.ChangeColor();
                
                ind.gameObject.SetActive(false);
            }
        }

        private void PositionIndicators()
        {
            for(int i = 0; i < indicators.Count; i++){
                var ind = indicators[i];
                var target = gm.allTargets[i];
                if(sc == target.sc){
                    continue; //skip if the target is me
                }
                //simple check for mesh visibility
                if(!target.mesh.isVisible){
                    ind.gameObject.SetActive(false);
                    continue;
                }
                //Ray to check for obstructions
                LayerMask layerMask = 1 << 11; //weapons layer
                RaycastHit hit;
                Vector3 origin = this.transform.position + (transform.up * 0.5f); //distance offset
	            Vector3 dir = target.transform.position - origin;
                Debug.DrawRay(origin, dir, Color.green);
                Physics.Raycast(origin, dir, out hit, 2000, ~layerMask);
                if(hit.collider != null){
                    //something was hit
                    if(hit.collider.attachedRigidbody == target.rb){
                        //the target was hit
                        if(ind.gameObject.activeSelf == false)
                        ind.gameObject.SetActive(true);
                    }
                    else{
                        //the target is obstructed
                        if(ind.gameObject.activeSelf == true)
                        ind.gameObject.SetActive(false);
                        continue;
                    }
                }
                else{
                    continue; //skip if the nothing was hit
                }

                //position indicators
                var desiredPosition = MathC.WorldToHUDSpace(Camera.main, target.transform.position, overlayHud.transform.position);
                ind.transform.position = Vector3.Lerp(ind.transform.position, desiredPosition, 50 * Time.deltaTime);
                //fill details
                float distance = Vector3.Distance(transform.position, target.transform.position);
                ind.distance.text = distance.ToString(".#0");
                //ind.transform.localScale =  (ind.transform.localScale + (Vector3.one * 75)) / distance; This will scale the target indicator.

            }
            if(ts.currentTarget == null || ts.currentTarget.gameObject.activeInHierarchy == false){
                targetPointer.SetActive(false);
            }
            else{
                targetPointer.SetActive(true);
                targetPointer.transform.LookAt(ts.currentTarget.transform, transform.up);
            }
            
        }

        private void AimWeaponReticle(){
            //reticle for accurate aim
            var actualPosition = MathC.WorldToHUDSpace(Camera.main, ship.aimPosition, overlayHud.transform.position);
            weaponReticle.transform.position = Vector3.Slerp(weaponReticle.transform.position, actualPosition, 15 * Time.deltaTime);
            //reticle for basically the center of the screen
            var targetPosition = MathC.WorldToHUDSpace(Camera.main, ship.centerPosition, overlayHud.transform.position);
            centerPositionReticle.transform.position = Vector3.Lerp(centerPositionReticle.transform.position, targetPosition, 25 * Time.deltaTime);
            //Debug.Log(centerPositionReticle.transform.position);
        }

        private void LockOnIndicator(){
            if(lockOnIndicator.gameObject.activeSelf == false)lockOnIndicator.gameObject.SetActive(true);
            lockOnIndicator.transform.position = Vector3.Lerp(lockOnIndicator.transform.position, ts.lockTransform.position, 50f * Time.deltaTime);
        }
        #endregion

    }

}
