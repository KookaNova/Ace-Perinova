using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Utilities{
    public class OrientationLock : MonoBehaviour
    {
        [Header("Orientation Lock")]
        [SerializeField] bool xLock = false;
        [SerializeField] bool yLock = false;
        [SerializeField] bool zLock = false;

        Vector3 lockedOrientation = Vector3.zero;

        private void Update() {
            float x = 0; 
            float y = 0; 
            float z = 0;
            if(!xLock){
                x = this.gameObject.transform.rotation.eulerAngles.x;
            }
            if(!yLock){
                y = this.gameObject.transform.rotation.eulerAngles.y;
            }
            if(!zLock){
                z = this.gameObject.transform.rotation.eulerAngles.z;
            }

            lockedOrientation = new Vector3(x,y,z);

            this.gameObject.transform.rotation = Quaternion.Euler(lockedOrientation);

        }
    }
}

