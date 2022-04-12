using System.Collections;
using UnityEngine;

namespace AcePerinova.Utilities{
    public class AutoDestroy : MonoBehaviour
    {
        public float time;
        public bool disable = false;

        private void Awake() {
            StartCoroutine(Terminate());
        }

        IEnumerator Terminate(){
            yield return new WaitForSecondsRealtime(time);
            if(disable){
                this.gameObject.SetActive(false);
            }
            else{
                Destroy(this.gameObject);
            }
        }
    }
}

