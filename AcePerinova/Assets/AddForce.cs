using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{
    public float force;
    public bool isRandomDirection = true;
    public bool onAwake = true;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        if (onAwake) {
            Force();
        }
    }

    private void Force() {
        Vector3 direction = Vector3.forward;
        if (isRandomDirection) {
            direction = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
        }

        direction *= force;
        rb.velocity = direction;
    }
}
