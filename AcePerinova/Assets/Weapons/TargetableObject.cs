using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.GameManagement;

namespace AcePerinova.Controller
{   
    [RequireComponent(typeof(Rigidbody))]
    public class TargetableObject : MonoBehaviour
    {
        public string targetName = "Unknown";
        public int team = 0; //0 = a, 1 = b, 2 = global
        public bool isObjective = false;
        public MeshRenderer mesh;
        public Rigidbody rb;
        public SpacecraftController sc;

        private void Awake() {
            var gm = FindObjectOfType<GameManager>();
            mesh = this?.GetComponent<MeshRenderer>(); //a mesh is ultimately required to determine visibility.
            rb = GetComponent<Rigidbody>(); //required for obstruction check
            if(mesh == null){
                mesh = GetComponentInChildren<MeshRenderer>();
            }
            
            gm.allTargets.Add(this);


            sc = this?.GetComponentInParent<SpacecraftController>();
            if(sc != null){
                targetName = sc.playerName;
                team = sc.team;
            }
            

        }
    
    }
}


