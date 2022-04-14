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
        Selectables.ShipUtility ship;
        Controller.PlayerController sc;
        GameManagement.GameManager gm;
        

        [SerializeField] Canvas overlayHud;
       
        public Image[] thrustImage, speedImage;

        //Weapons 
        public Image weaponReticle, centerPositionReticle; //Maybe this can be swapped by the weapon choice
        public IndicatorComponent indicatorPrefab;
        public List<IndicatorComponent> indicators;
        public Image orientationImage;

        public void Awake(){
            gm = FindObjectOfType<GameManagement.GameManager>();
            ship = this?.GetComponentInParent<Selectables.ShipUtility>();
            sc = this?.GetComponentInParent<Controller.PlayerController>();
            if(overlayHud == null){
                Debug.LogWarning("HUDController: No overlayHud canvas has been s elected in the inspector.");
            }
            overlayHud.worldCamera = Camera.main;
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
        }

        private void Fills(){
            foreach (var item in thrustImage){
                item.fillAmount = MathC.NormalizeRange(sc.ship.maxSpeed, sc.speedTarget);
            }
            foreach (var item in speedImage){
                item.fillAmount = MathC.NormalizeRange(sc.ship.maxSpeed, sc.currentSpeed);
            }
        }

        private void UpdateRoll(){
            float yaw = ship.transform.rotation.eulerAngles.y;
            float pitch = ship.transform.rotation.eulerAngles.x;
            float roll = ship.transform.rotation.eulerAngles.z;
            orientationImage.transform.rotation = Quaternion.Euler(pitch, yaw, -roll);

            orientationImage.transform.position = centerPositionReticle.transform.position;
        }

        #region Target and Weapons
        public void CreateTargetIndicators(){
            foreach(var item in indicators){
                Destroy(item);
            }
            indicators.Clear();
            for(int i = 0; i < gm.allTargets.Count; i++){
                TargetableObject target = gm.allTargets[i];
                var ind = Instantiate(indicatorPrefab, overlayHud.transform);
                indicators.Add(ind);
                ind.objectName.text = target.targetName;

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
                Vector3 origin = this.transform.position + (transform.forward * 5); //distance offset
	            Vector3 dir = target.transform.position - origin;
                Debug.DrawRay(origin, dir, Color.green);
                Physics.SphereCast(origin, 2, dir, out hit, 2000, ~layerMask);
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
                ind.distance.text = Vector3.Distance(transform.position, target.transform.position).ToString(".#0");
                

            }
        }

        private void AimWeaponReticle(){
            //reticle for accurate aim
            var actualPosition = MathC.WorldToHUDSpace(Camera.main, ship.actualPosition, overlayHud.transform.position);
            weaponReticle.transform.position = Vector3.Slerp(weaponReticle.transform.position, actualPosition, 15 * Time.deltaTime);
            //reticle for basically the center of the screen
            var targetPosition = MathC.WorldToHUDSpace(Camera.main, ship.targetPosition, overlayHud.transform.position);
            centerPositionReticle.transform.position = Vector3.Lerp(centerPositionReticle.transform.position, targetPosition, 25 * Time.deltaTime);
        }

        #endregion

        
    
    }

}
