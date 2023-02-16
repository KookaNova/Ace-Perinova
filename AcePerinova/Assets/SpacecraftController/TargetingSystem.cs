using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller
{
    public class TargetingSystem : MonoBehaviour
    {
        GameManagement.GameManager gm;
        SpacecraftController sc;
        InputInterpreter _in;
        
        [SerializeField] Camera targettingCamera;
        [SerializeField] Transform overlayHUD;

        int targetedTeam = 0;

        //Target Selection
        [HideInInspector] public List<TargetableObject> validTargets = new List<TargetableObject>();
        [HideInInspector] public TargetableObject currentTarget;
        
        public delegate void TargetSelect();
        public event TargetSelect OnTargetSelect;
        public delegate void LockStatusChanged();
        public event LockStatusChanged OnLockStatusChanged;

        //[HideInInspector] public UnityEvent targetSelectEvent, lockStatusEvent;
        //lock on
        [HideInInspector] public bool isLocking = false, targetLocked = false;
        [HideInInspector] public Transform lockTransform = null;
        float lockIncrementor = 0.05f;

        private void Awake() {
            sc = GetComponentInParent<SpacecraftController>();
            gm = FindObjectOfType<GameManagement.GameManager>();
            lockTransform = new GameObject("LockPositioner").transform;
            lockTransform.SetParent(overlayHUD);
            lockTransform.transform.localPosition = Vector3.zero;
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

        private void FixedUpdate() {
            if(currentTarget?.gameObject.activeInHierarchy == false)SelectTarget();
            if(validTargets.Count > 0){
                TargetLock();
            }
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
            if(currentTarget != null)DeselectTarget();
            LockFailed();
            float distance = 1;
            Vector3 center = Vector3.one/2;
            foreach(var target in validTargets){
                if(target == currentTarget)continue;
                Vector2 pos = Camera.main.WorldToViewportPoint(target.transform.position);
                float diff = Vector2.Distance(center, pos);

                if(diff < distance){
                    distance = diff;
                    if(currentTarget != null) currentTarget.isTargeted = false;
                    target.isTargeted = true;
                    currentTarget = target;
                }
                else{
                    continue;
                }
            }
            sc.lockedTarget = currentTarget;
            if(OnTargetSelect != null) OnTargetSelect();
        }

        public void DeselectTarget(){
            LockFailed();
            currentTarget = null;
            sc.lockedTarget = null;
            if(OnTargetSelect != null) OnTargetSelect();
        }

        private void TargetLock(){
            if(targettingCamera == null){
                Debug.LogWarningFormat("Targeting System on {0} is missing a targetting camera. TargetLock() can not be achieved.", this);
                return;
            }
            if(currentTarget == null){
                LockFailed();
                return;
            }
            //First check the FOV Camera
            Vector3 targetPosition = currentTarget.transform.position;
            Vector2 fovCamPosition = targettingCamera.WorldToViewportPoint(targetPosition);
            if(fovCamPosition.x > 1 || fovCamPosition.x < 0 || fovCamPosition.y > 1 || fovCamPosition.y < 0){
                //lock failed
                LockFailed();
                return;
            }
            //Raycast obstruction check
            LayerMask layerMask = 1 << 11; //weapons layer
            RaycastHit hit;
            Vector3 origin = targettingCamera.transform.position;
	        Vector3 dir = currentTarget.transform.position - origin;
            Debug.DrawRay(origin, dir, Color.red);
            Physics.SphereCast(origin, 2, dir, out hit, 2000, ~layerMask);

            if(hit.collider != null){
                if(hit.collider.attachedRigidbody == currentTarget.rb){
                    //the target was hit //Lock tracking
                    isLocking = true;
                    Vector3 targetScreenPos = MathC.WorldToHUDSpace(Camera.main, targetPosition, overlayHUD.transform.position);
                    Vector3 result = Vector3.MoveTowards(lockTransform.position, targetScreenPos, lockIncrementor * Time.deltaTime);
                    lockTransform.position = result;
                    lockIncrementor += lockIncrementor * Time.deltaTime;

                    if(lockTransform.position == targetScreenPos){
                        if(targetLocked != true){
                            Debug.Log("Target Locked");
                            currentTarget.isLocked = true;
                            targetLocked = true; 
                            sc.lockedTarget = currentTarget;
                            if(OnLockStatusChanged != null)OnLockStatusChanged();
                            lockIncrementor = 100;
                        }
                    }
                }
                else{
                    LockFailed();
                }
            }
        }
        private void LockFailed(){
            if(currentTarget != null) currentTarget.isLocked = false;
            sc.lockedTarget = null;
            isLocking = false;
            targetLocked = false;
            lockIncrementor = 0.05f;
            if(OnLockStatusChanged != null)OnLockStatusChanged();
            //lockIndicatorPosition = Vector3.one/2;
        }
    }
}

