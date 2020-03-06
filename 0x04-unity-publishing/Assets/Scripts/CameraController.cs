using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    
    private Vector3 _relativePosition;
    // Start is called before the first frame update
    void Start()
    {
        _relativePosition =  transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var playerPosition = player.transform.position;

        transform.position = playerPosition + _relativePosition;
    }
}
