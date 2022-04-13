using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AcePerinova.Utilities;

namespace AcePerinova.Controller
{
    public class HUDController : MonoBehaviour
    {
        Selectables.ShipUtility ship;
        Controller.PlayerController sc;

        [SerializeField] Canvas overlayHud;

       
        public Image[] thrustImage, speedImage;

        //Weapons 
        public Image weaponReticle, targetPositionReticle; //Maybe this can be swapped by the weapon choice
        public IndicatorComponent indicatorPrefab;
        private List<IndicatorComponent> indicators;

        public void Awake(){
            ship = this?.GetComponentInParent<Selectables.ShipUtility>();
            sc = this?.GetComponentInParent<Controller.PlayerController>();
            if(overlayHud == null){
                Debug.LogWarning("HUDController: No overlayHud canvas has been s elected in the inspector.");
            }
            overlayHud.worldCamera = Camera.main;
        }

        private void LateUpdate() {
            if(ship != null){
                AimWeaponReticle();
            }
            Fills();
        }

        private void Fills(){
            foreach (var item in thrustImage){
                item.fillAmount = MathC.NormalizeRange(sc.ship.maxSpeed, sc.speedTarget);
            }
            foreach (var item in speedImage){
                item.fillAmount = MathC.NormalizeRange(sc.ship.maxSpeed, sc.currentSpeed);
            }
        }

        private void AimWeaponReticle(){
            //reticle for accurate aim
            var actualScreenPosition = Camera.main.WorldToScreenPoint(ship.actualPosition);
            actualScreenPosition.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude; //adjusts the screen z position
            var actualPosition = Camera.main.ScreenToWorldPoint(actualScreenPosition);
            weaponReticle.transform.position = Vector3.Slerp(weaponReticle.transform.position, actualPosition, 15 * Time.deltaTime);
            //reticle for basically the center of the screen
            var targetScreenPosition = Camera.main.WorldToScreenPoint(ship.targetPosition);
            targetScreenPosition.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude; //adjusts the screen z position
            var targetPosition = Camera.main.ScreenToWorldPoint(targetScreenPosition);
            targetPositionReticle.transform.position = targetPosition;
            
        }

        private void CreateTargetIndicators(){
            
        }
    
    }

}
