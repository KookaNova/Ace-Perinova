using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazySusan : MonoBehaviour
{
    public float rotationSpeed;

    void Update()
    {
        Vector3 newRotation = new Vector3(0, rotationSpeed, 0);

        this.transform.Rotate(newRotation * Time.deltaTime);
    }
}
