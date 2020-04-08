using System;
using System.Collections;
using System.Collections.Generic;
using PlayControls;
using UnityEngine;
using UnityEngine.Animations;

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
    public float distance = 6.25f;

    private Vector3 _offset;

    private Transform _camera;
    private Vector3 _cameraRestPos;


    /// <summary>
    /// Property for getting and setting the camera Yaw (Y axis)
    /// </summary>
    public float Yaw
    {
        get => transform.rotation.eulerAngles.y;
        set => transform.Rotate(0, value, 0);
    }

    /// <summary>
    /// Property for adjusting the camera Height
    /// </summary>
    public float Height
    {
        get => _camera.position.y;
        set
        {
            if (IsInverted)
                value *= -1;
            
            if (_cameraRestPos.y + value > distance)
                _cameraRestPos.y = distance;

            if (_cameraRestPos.z + value > -1.5f)
                _cameraRestPos.z = -2.5f;


            if (value < -0.1 && _camera.position.y + value < player.transform.position.y)
                return;

            _cameraRestPos.Set(_cameraRestPos.x, _cameraRestPos.y + value,
                Math.Abs(_cameraRestPos.z) < distance ? _cameraRestPos.z + value : -distance);
        }
    }

    /// <summary>
    /// Property for Get/Set camera Y-Axis invert status
    /// </summary>
    public bool IsInverted { get; set; }


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
        _cameraRestPos = new Vector3(0, 2.5f, -distance);
    }

    

    private void FixedUpdate()
    {
        _camera.LookAt(player.transform.position + new Vector3(0, 2.5f, 0));

        transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * 10);
        _camera.transform.localPosition = Vector3.Lerp(_camera.transform.localPosition, _cameraRestPos, Time.deltaTime);
        // if (!HandelOutOfSight())
        // {
        //     if (!Physics.CheckSphere(_camera.transform.position, .25f, obstacleLayer))
        //     {
        //         _camera.transform.localPosition =
        //             Vector3.Lerp(_camera.transform.localPosition, _cameraRestPos, Time.deltaTime);
        //     }
        // }
        // else
        // {
        //     _camera.transform.localPosition = _cameraRestPos = _cameraMinDist;
        // }
    }


    private readonly Vector3 _cameraMinDist = new Vector3(0, 2.5f, -2.5f);

    private bool HandelOutOfSight()
    {
        var position = player.transform.position;
        if (!Physics.Raycast(_camera.transform.position, (position - _camera.transform.position).normalized,
            (position - _camera.transform.position).magnitude, obstacleLayer))
            return false;
        Debug.Log("Can't See Player");
        return true;
    }
}