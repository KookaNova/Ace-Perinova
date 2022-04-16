using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AcePerinova.Weapons{
    public class ImpactBullet : WeaponComponent
    {

        protected override void WeaponAction(){
            rb.AddForce(transform.forward * force, ForceMode.VelocityChange);
        }
    }
}


