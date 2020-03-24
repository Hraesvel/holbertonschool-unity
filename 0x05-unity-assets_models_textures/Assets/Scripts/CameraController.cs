using System;
using System.Collections;
using System.Collections.Generic;
using PlayControls;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    /// <summary>
    /// Player to follow
    /// </summary>
    public GameObject player;
    /// <summary>
    /// Layer that obstacles are on that are line of sight blockers
    /// </summary>
    public LayerMask obstacleLayer;
    /// <summary>
    /// Camera to apply controls to
    /// </summary>
    public GameObject camera;

    /// <summary>
    /// Max distance the camera need to return to from the subject.
    /// </summary>
    public float distance = 6.5f;



    private Vector3 _offset;

    private Transform _camera;

    
    /// <summary>
    /// Property for getting and setting the camera Yaw (Y axis)
    /// </summary>
    public float Yaw
    {
        get => transform.rotation.eulerAngles.y;
        set => transform.Rotate(0, value, 0);
    }


    private void Awake()
    {
        _offset = new Vector3(0, 2.5f, -distance);
        _camera = camera.transform;
        _camera.position = transform.position;
        _camera.parent = transform;
        player = GameObject.FindWithTag("Player");

        transform.position = player.transform.position;
        _camera.localPosition = Vector3.zero + _offset;
        _camera.rotation = Quaternion.Euler(9, 0, 0);
    }


    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * 10);
        if (!HandelOutOfSight())
            if (_camera.transform.localPosition != _cameraRestPos &&
                !Physics.CheckSphere(_camera.transform.position, .5f, obstacleLayer))
            {
                _camera.transform.localPosition =
                    Vector3.Lerp(_camera.transform.localPosition, _cameraRestPos, Time.deltaTime);
            }
    }


    private readonly Vector3 _cameraMinDist = new Vector3(0, 2.5f, -2f);
    private readonly Vector3 _cameraRestPos = new Vector3(0, 2.5f, -6.5f);

    private bool HandelOutOfSight()
    {
        var position = player.transform.position;
        
        if (!Physics.Raycast(_camera.transform.position, (position - _camera.transform.position).normalized,
            (position - _camera.transform.position).magnitude, obstacleLayer))
            return false;
        Debug.Log("Can't See Player");

        _camera.transform.localPosition = _cameraMinDist;
        return true;
    }
}