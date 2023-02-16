using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace AcePerinova.Controller{
    /// <summary>
    /// Used for etc. utilities like storing positions and potentially certain sounds or effects
    /// </summary>
    public class ShipUtility : MonoBehaviour
    {
        SpacecraftController sc;
        HealthComponent hc;
        InputInterpreter _in;

        [Header("Weapons")]
        public Transform[] primaryWeaponPositions;
        public VisualEffect[] primaryMuzzle;
        public Transform[] secondaryWeaponPositions;
        public VisualEffect[] secondaryMuzzle;
        [HideInInspector] public Vector3 centerPosition, aimPosition;

        [Header("Sounds")]
        public AudioSource collisionSFX;
        public AudioSource shieldBrokenSFX;
        public AudioSource[] primaryWeaponSFX, secondaryWeaponSFX;

        private void Awake() {
            sc = GetComponentInParent<SpacecraftController>();
            hc = this?.GetComponent<HealthComponent>();
            
            _in = this?.GetComponentInParent<InputInterpreter>();
        }

        private void OnEnable(){
            sc.OnPrimaryFire += PrimaryFire;
            sc.OnSecondaryFire += SecondaryFire;
            if(hc != null){
                hc.OnShieldBroken += ShieldBroken;
            }


        }

        private void OnDisable(){
            sc.OnPrimaryFire -= PrimaryFire;
            sc.OnSecondaryFire -= SecondaryFire;
            if(hc != null){
                hc.OnShieldBroken -= ShieldBroken;

            }
        }

        private void FixedUpdate(){
            centerPosition = transform.position + (transform.forward * sc.aimDistance);
            aimPosition = Vector3.Lerp(aimPosition, centerPosition, 15f * Time.deltaTime);

            foreach(var item in primaryWeaponPositions){
                item.LookAt(aimPosition);
            }

        }

        private void ShieldBroken(){
            shieldBrokenSFX.pitch = Random.Range(.8f, 1.2f);
            shieldBrokenSFX.Play();

        }

        private void OnCollisionEnter(Collision other) {
            collisionSFX.pitch = Random.Range(.8f, 1.2f);
            collisionSFX.Play();
        }

        private void PrimaryFire(int index){
            if(primaryWeaponSFX.Length <= 0)return;
            var sound = primaryWeaponSFX[index];
            sound.pitch = Random.Range(0.75f, 1.15f);
            sound.Play();

        }
        private void SecondaryFire(int index){
            if(secondaryWeaponSFX.Length <= 0)return;
            var sound = secondaryWeaponSFX[index];
            sound.pitch = Random.Range(0.75f, 1.15f);
            sound.Play();
        }


    }
}

