using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AcePerinova.Controller
{
    public class TargetingSystem : MonoBehaviour
    {
        GameManagement.GameManager gm;
        SpacecraftController sc;
        InputInterpreter _in;

        int targetedTeam = 0;
        public List<TargetableObject> validTargets;
        public TargetableObject currentTarget;
        
        [HideInInspector] public UnityEvent targetSelectEvent;


        //store screen positions of targets.
        //decide if a target is visible

        private void Awake() {
            sc = GetComponentInParent<SpacecraftController>();
            gm = FindObjectOfType<GameManagement.GameManager>();
            if(sc.team == 0){
                targetedTeam = 1;
            }
            else{
                targetedTeam = 0;
            }
            _in = GetComponentInParent<InputInterpreter>();
            _in.teamIncrementEvent.AddListener(ChangeTargetTeam);
            _in.targetSelectEvent.AddListener(SelectTarget);
            FindValidTargets();
        }

        private void FindValidTargets(){
            validTargets.Clear();
            foreach(var target in gm.allTargets){
                if(target.team != targetedTeam){
                    continue;
                }
                else{
                    validTargets.Add(target);
                }
            }
            SelectTarget();

        }

        public void ChangeTargetTeam(){
            Debug.Log("Changing teams");
            targetedTeam++;
            if(targetedTeam > 2){
                targetedTeam = 0;
            }

            FindValidTargets();
        }

        public void SelectTarget(){
            if(currentTarget != null) currentTarget.isTargeted = false;
            float distance = 1;
            Vector3 center = Vector3.one/2;
            foreach(var target in validTargets){
                Vector3 pos = Camera.main.WorldToViewportPoint(target.transform.position);
                float diff = Vector2.Distance(center, pos);
                Debug.Log(diff);

                if(diff < distance){
                    distance = diff;
                    currentTarget = target;
                    currentTarget.isTargeted = true;
                    Debug.Log(currentTarget.targetName);
                }
                else{
                    continue;
                }
            }
            targetSelectEvent.Invoke();
            
        }
    }
}

