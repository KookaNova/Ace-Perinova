using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.GameManagement;

namespace AcePerinova.Controller
{
    public class TargetableObject : MonoBehaviour
    {
        public string targetName = "Unknown";

        GameManager gm;

        private void Awake() {

            gm = FindObjectOfType<GameManager>();
            gm.allTargets.Add(this);

            targetName = this?.GetComponent<SpacecraftController>().playerName;

        }
    
    }
}


