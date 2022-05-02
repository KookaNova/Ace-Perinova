using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AcePerinova.GameManagement;

namespace AcePerinova.Controller
{   
    [RequireComponent(typeof(Rigidbody))]
    public class TargetableObject : MonoBehaviour
    {
        GameManager gm;

        public string targetName = "Unknown";
        public int team = 0; //0 = a, 1 = b, 2 = global
        public bool isObjective = false;

        [HideInInspector] public MeshRenderer mesh;
        [HideInInspector] public Rigidbody rb;
        [HideInInspector] public SpacecraftController sc;
        [HideInInspector] public bool isTargeted, isLocked;

        private void Awake() {
            gm = FindObjectOfType<GameManager>();
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

        private void OnDestroy() {
            if(gm != null){
                gm.allTargets.Remove(this);
            }
        }
    }
}


