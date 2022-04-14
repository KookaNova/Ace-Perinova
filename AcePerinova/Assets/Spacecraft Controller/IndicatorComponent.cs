using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AcePerinova.Utilities{
    public class IndicatorComponent : MonoBehaviour
    {
        public Text player, objectName, distance;
        public GameObject lockIcon;

        private void Awake() {
            //lockIcon.SetActive(false);
        }
    
    }
}

