using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller
{
    public class TargetingSystem : MonoBehaviour
    {
        TargetableObject selectedTarget;
        GameManagement.GameManager gm;

        //store screen positions of targets.
        //decide if a target is visible

        private void Awake() {
            gm = FindObjectOfType<GameManagement.GameManager>();
        }

        private void LateUpdate() {
            
        }
    }
}

