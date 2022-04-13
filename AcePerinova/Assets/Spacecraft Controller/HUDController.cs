using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AcePerinova.Controller
{
    public class HUDController : MonoBehaviour
    {
        public Selectables.ShipUtility ship;
        [SerializeField] Canvas overlayHud;

        public Image weaponReticle; //Maybe this can be swapped by the weapon choice

        public void Awake(){
            if(overlayHud == null){
                Debug.LogWarning("HUDController: No overlayHud canvas has been s elected in the inspector.");
            }
            overlayHud.worldCamera = Camera.main;
        }

        private void LateUpdate() {
            
            if(ship != null){
                AimWeaponReticle();
            }
        }

        private void AimWeaponReticle(){
            var screen = Camera.main.WorldToScreenPoint(ship.aimPosition);
            screen.z = (overlayHud.transform.position - Camera.main.transform.position).magnitude;
            var position = Camera.main.ScreenToWorldPoint(screen);
            weaponReticle.transform.position = Vector3.Slerp(weaponReticle.transform.position, position, 15 * Time.fixedDeltaTime);
        }
    
    }

}
