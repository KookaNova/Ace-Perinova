using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class VectorVisualizer : MonoBehaviour
{
    public Vector3 origin;
    public Vector3 endPoint;


    // Update is called once per frame

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(origin, 1);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(endPoint, 2);
        var direction = (endPoint - origin).normalized;
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origin, direction * 20);
    }

    void Update()
    {


        
    }
}
