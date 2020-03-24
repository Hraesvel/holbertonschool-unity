using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

public class CameraVisablity : MonoBehaviour
{
      public GameObject target;
    public LayerMask obstacl;


    void Update()
    {
        var position = target.transform.position;
        if (Physics.Raycast(transform.position, (position - transform.position).normalized,
            (position - transform.position).magnitude, obstacl))
        {
            transform.localPosition = new Vector3(0, 2.5f, -2f);
        }
        else if (transform.localPosition != new Vector3(0, 2.5f, -6.5f) &&
                 !Physics.CheckSphere(transform.position, .5f, obstacl))
        {
            transform.localPosition =
                Vector3.Lerp(transform.localPosition, new Vector3(0, 2.5f, -6.5f), Time.deltaTime);
        }
    }
    
    
}