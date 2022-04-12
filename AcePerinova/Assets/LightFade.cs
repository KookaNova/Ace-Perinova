using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Utilities
{
    public class LightFade : MonoBehaviour
    {
        public float endIntensity;
        public float rate;

        bool decreasing;
        Light l;

        private void Awake() {
            l = GetComponent<Light>();
        }

        private void Update() {
            l.intensity = Mathf.Lerp(l.intensity, endIntensity, rate * Time.deltaTime);
        }
    }

    
}

