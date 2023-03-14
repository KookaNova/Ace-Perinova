using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    public class ImpactBullet : WeaponComponent
    {

        protected override void WeaponAction(){
        }

        private void Update() {
            var forwardForce = Vector3.forward * force * Time.deltaTime;
            transform.Translate(forwardForce, Space.Self);
        }
    }
}


