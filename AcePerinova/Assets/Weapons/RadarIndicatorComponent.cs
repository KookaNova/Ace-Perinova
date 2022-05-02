using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.Controller;
using UnityEngine.UI;

namespace AcePerinova.Utilities{
    public class RadarIndicatorComponent : MonoBehaviour
    {
        public Texture2D icon;
        public Image image;
        public Color32 targetColor;
        
        [HideInInspector] public TargetableObject target, ownerTarget;
        [HideInInspector] public Vector3 targetPosition;
        [HideInInspector] public Quaternion targetRotation;
        

        public void Activate() {
            image = GetComponent<Image>();
            if(target.team == 2){
                targetColor = ColorPaletteUtility.global;
            }
            else if(ownerTarget.team == target.team){
                targetColor = ColorPaletteUtility.friendly;
            }
            else{
                targetColor = ColorPaletteUtility.enemy;
            }

            image.color = targetColor;
        }

        private void LateUpdate() {
            targetPosition = target.transform.position;
            targetRotation = target.transform.rotation;
        }
        

    }

}


