using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Controller{
    /// <summary>
    /// This component serves to help in controlling spacecraft.
    /// </summary>
    public class SpacecraftController : MonoBehaviour
    {
        public float thrust {get; set;}
        public Vector3 rotate {get;set;}

        public float speed = 20;
        public float pitch = 10;
        public float yaw = 5;
        public float roll = 10;

        private void Update() {
            this.transform.Translate(new Vector3(0,0,thrust * speed * Time.deltaTime), Space.Self);
            this.transform.Rotate(rotate.x * pitch * Time.deltaTime, rotate.y * yaw * Time.deltaTime, rotate.z * roll * Time.deltaTime);
        }
        
    }

}

