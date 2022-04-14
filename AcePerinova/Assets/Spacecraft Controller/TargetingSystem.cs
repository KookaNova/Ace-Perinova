using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller
{
    public class TargetingSystem : MonoBehaviour
    {
        TargetableObject selectedTarget;
        GameManagement.GameManager gm;
        SpacecraftController sc;
        HUDController hud;
        int targetedTeam = 0;

        public List<Vector3> targetScreenPos;
        //store screen positions of targets.
        //decide if a target is visible

        private void Awake() {
            if(sc.team == 0){
                targetedTeam = 1;
            }
            else{
                targetedTeam = 0;
            }
        }

        private void Update() {

            
        }
    }
}

