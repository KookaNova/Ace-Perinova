using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AcePerinova.Utilities{
    public class IndicatorComponent : MonoBehaviour
    {
        public Text player, character, distance;
        public GameObject lockIndicator;

        private void Awake() {
            lockIndicator.SetActive(false);
        }
    
    }
}

