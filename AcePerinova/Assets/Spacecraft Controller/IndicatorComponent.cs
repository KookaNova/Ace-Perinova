using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AcePerinova.Controller;

namespace AcePerinova.Utilities{
[ExecuteAlways]
    public class IndicatorComponent : MonoBehaviour
    {
        public TargetableObject targetableObject;
        public TargetingSystem targetingSystem;
        public Image indicatorImage;
        public Text player, objectName, distance;
        public GameObject objectiveIndicator, targetActive, lockIcon;
        public Color32 color;
        
        Animator ani;

        
                //ts?.targetSelectEvent.AddListener(ind.CheckTarget);
                //ts?.lockStatusEvent.AddListener(ind.CheckLock);

        private void Awake() {
            indicatorImage = GetComponent<Image>();
            ani = GetComponent<Animator>();
            lockIcon.SetActive(false);
        }

        private void OnEnable(){
            if(targetableObject == null)return;
            objectName.text = targetableObject.targetName;
            objectiveIndicator.SetActive(targetableObject.isObjective);
            CheckTarget();
            
            targetingSystem.OnTargetSelect += CheckTarget;
            targetingSystem.OnLockStatusChanged += CheckLock;

        }

        private void OnDisable() {
            targetingSystem.OnTargetSelect -= CheckTarget;
            targetingSystem.OnLockStatusChanged -= CheckLock;
        }

        public void ChangeColor(){
            indicatorImage.color = color;
            player.color = color;
            objectName.color = color;
            distance.color = color;
        }

        public void CheckTarget(){
            targetActive.SetActive(targetableObject.isTargeted);
            ani.SetBool("isActive", targetableObject.isTargeted);
            
            
        }
        public void CheckLock(){
            lockIcon.SetActive(targetableObject.isLocked);
        }

        #if UNITY_EDITOR
        private void Update(){
            indicatorImage.color = color;
            player.color = color;
            objectName.color = color;
            distance.color = color;

        }
        #endif

        
    
    }
}

