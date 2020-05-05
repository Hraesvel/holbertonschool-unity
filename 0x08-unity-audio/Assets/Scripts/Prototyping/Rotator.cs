using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rate = 1;
    public bool reverse = false;

    public Vector3 dir;
    public Rigidbody rig;

    private bool _giveVel;
    private Rigidbody _rigidbody;

    // Update is called once per frame
    private void Start()
    {
        _rigidbody = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, reverse ? -rate : rate );
        // rig.AddTorque(transform.up * (reverse ? -rate : rate), ForceMode.Acceleration);
        rig.angularVelocity = Vector3.forward * rate;
        
        if (rig.angularVelocity.sqrMagnitude > 2 * 2)
            rig.angularVelocity = Vector3.up * rate;
        
    }

}
