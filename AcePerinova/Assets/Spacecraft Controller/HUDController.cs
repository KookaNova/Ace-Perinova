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
        TargetingSystem ts;
        int team;
        
        [SerializeField] Canvas overlayHud;
       
        public Image[] thrustImage, speedImage;

        //Weapons 
        public Image weaponReticle, centerPositionReticle; //Maybe this can be swapped by the weapon choice
        public IndicatorComponent indicatorPrefab;
        public List<IndicatorComponent> indicators;
        public Image orientationImage;
        public GameObject targetPointer;

        public void Awake(){
            gm = FindObjectOfType<GameManagement.GameManager>();
            ship = this?.GetComponentInParent<Selectables.ShipUtility>();
            sc = this?.GetComponentInParent<Controller.PlayerController>();
            ts = this?.GetComponent<TargetingSystem>();
            team = sc.team;
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
                ind.targetableObject = target;
                indicators.Add(ind);
                ts?.targetSelectEvent.AddListener(ind.CheckTarget);
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
                float distance = Vector3.Distance(transform.position, target.transform.position);
                ind.distance.text = distance.ToString(".#0");
                //ind.transform.localScale =  (ind.transform.localScale + (Vector3.one * 75)) / distance; This will scale the target indicator.

            }
            if(ts.currentTarget == null){
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

        #endregion

        
    
    }

}
