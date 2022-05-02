using System.Collections.Generic;
using UnityEngine;
using AcePerinova.Controller;
using AcePerinova.Utilities;

namespace AcePerinova.Weapons{
    public class RadarComponent : MonoBehaviour
    {
        public Vector3 radarRotation;

        GameManagement.GameManager gm;
        TargetableObject tg;
        Canvas canvas;
        
        [SerializeField] Transform radarPositionParent;
        [SerializeField] RadarIndicatorComponent targetIndicator;

        List<RadarIndicatorComponent> indicators = new List<RadarIndicatorComponent>();

        private void Awake() {
            gm = FindObjectOfType<GameManagement.GameManager>();
            tg = GetComponentInParent<TargetableObject>();
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.planeDistance = 0.05f;
            GenerateIndicators();
        }

        private void GenerateIndicators(){
            foreach (var ind in indicators){
                Destroy(ind);
            }
            indicators.Clear();
            indicators.TrimExcess();
            for(int i = 0; i < gm.allTargets.Count; i++){
                if(gm.allTargets[i] == tg)continue;
                var ind = Instantiate(targetIndicator, radarPositionParent);
                ind.target = gm.allTargets[i];
                ind.name = ind.target.targetName;
                ind.ownerTarget = tg;
                ind.Activate();
                indicators.Add(ind);
            }
        }

        private void LateUpdate() {
            if(indicators.Count != gm.allTargets.Count - 1)GenerateIndicators();
            PositionIndicators();
        }

        private void PositionIndicators(){
            var rot = tg.gameObject.transform.rotation;
            radarPositionParent.localRotation = Quaternion.Inverse(rot);

            foreach (var ind in indicators){
                //Activate or deactivate indicator
                if(ind.target.gameObject.activeInHierarchy == false){
                    ind.gameObject.SetActive(false);
                    continue;
                    }
                if(ind.gameObject.activeSelf == false && ind.target.gameObject.activeInHierarchy == true)ind.gameObject.SetActive(true);
                //Find the direction vector and convert it to local transforms on the radar
                Vector3 direction = tg.transform.position - ind.targetPosition;
                ind.transform.localPosition = direction*0.5f;//number here controls radar strength. A smaller multiplier covers a larger range.

                //finds height to a plane based on the ship's up vector
                float dot = Vector3.Dot(transform.up, -direction);
                Debug.Log(dot);

                ind.transform.localScale = (Vector3.one) + (Vector3.one * MathC.NormalizeRangeNegative1Positive1(-180, 250 , dot));
                ind.image.color = new Color(ind.image.color.r, ind.image.color.g, ind.image.color.b, 1 - Mathf.Abs(MathC.NormalizeRangeNegative1Positive1(-90, 90 , dot)));
                ind.transform.rotation = transform.rotation;
            }
            
        }
    }
}

