using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using PlayControls;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private string _id;

    /// <summary>
    /// Property to use to get a players ID
    /// </summary>
    public String Id
    {
        get => _id;
    }

    /// <summary>
    /// player start position
    /// </summary>
    public bool startAtPlayerPos = false;

    [SerializeField] private Controller controller;

    [SerializeField] private Timer timer;
    [SerializeField] private PauseMenu pauseMenu;

    private readonly Vector3 _start = new Vector3(0, 1.25f, 0);
    private Vector3 _spawn;

    private CameraController _camera;

    /// <summary>
    /// Camera Controller to use for the player
    /// </summary>
    public new GameObject cameraController;

    private Vector3 _direction;

    /// <summary>
    /// Player's normal movenment speed on the ground
    /// </summary>
    public float speed = 12f;

    /// <summary>
    /// Player's max speed while grounded and in the air
    /// </summary>
    public float maxSpeed = 16f;

    /// <summary>
    /// speed that the player/camera pivot around
    /// </summary>
    [Range(1, 10)] public float rotationSpeed = 5;

    /// <summary>
    /// Layer mask that the is a platform for the player
    /// </summary>
    public LayerMask platform;

    /// <summary>
    /// force use to push the play upwards into a jump
    /// </summary>
    public float jumpForce = 7f;

    /// <summary>
    /// the force use the magnify the effect of gravity on player's decent.
    /// </summary>
    public float fallSpeed = 5f;

    private int _invert = 1;

    private float _yaw;
    private float _pitch;


    [SerializeField] private Rigidbody rig;
    [SerializeField] private CapsuleCollider col;
    private bool _jump;
    private bool _hasJumped = false;
    private float _jumpTimeStamp;

    public float maxFall;

    private Transform _playerMesh;
    private Animator _playerAnime;
    private Vector3 _facingRot;


    [ContextMenu("Use Debug Spawn point")]
    private void ChangePlaySpawn()
    {
        if (GameObject.Find("spawnPt") != null)
        {
            Debug.Log("Moving player to spawnPt");
            transform.position = GameObject.Find("spawnPt").transform.position;
        }
        else
        {
            Debug.LogError("Error: spawnPt object doesn't exist, will do nothing");
        }
    }


    void Awake()
    {
        _id = "player_1";
        _direction = new Vector3(0, 0, 0);
        col = transform.GetComponent<CapsuleCollider>();
        rig = transform.GetComponent<Rigidbody>();
        _camera = cameraController.GetComponent<CameraController>();
        _camera.Yaw = 0;
        _camera.isInverted = PlayerPrefs.GetInt("invert_Y") != 0 ? true : false;
        _playerMesh = transform.GetChild(0);
        _playerAnime = _playerMesh.GetComponent<Animator>();
    }

    private void Start()
    {
        _spawn = startAtPlayerPos ? transform.position : _start;
    }


// Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause")) && !timer.HasWon)
        {
            if (pauseMenu.IsPaused)
            {
                Time.timeScale = 1;
                pauseMenu.Resume();
                if (SceneManager.sceneCount > 1)
                    SceneManager.UnloadSceneAsync("Scenes/Options");
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.Pause();
            }
        }

        if (controller.TryGetGamePadAxis(GamePad.LeftAxis, out var axis1))
        {
            _direction = (cameraController.transform.forward * axis1.x +
                          cameraController.transform.right * axis1.y) * speed;
        }
        else
        {
            _direction = Vector3.zero;
            _playerAnime.SetFloat(Move, 0f);
        }


        if (controller.TryGetGamePadAxis(GamePad.RightAxis, out var axis2))
            (_pitch, _yaw) = (axis2.y, axis2.x * rotationSpeed);
        else if (controller.TryGetMouseAxis(Mouse.Mouse1, out axis2))
            (_pitch, _yaw) = (axis2.y, axis2.x * rotationSpeed);
        else
            (_pitch, _yaw) = (0, 0);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _jump = true;
            _playerAnime.SetTrigger(Jump);
            _playerAnime.SetBool(Grounded, false);
            _jumpTimeStamp = Time.time + maxFall;
        }
        else if (_jump && !IsGrounded())
        {
            if (Time.time > _jumpTimeStamp)
            {
                // Debug.Log("Falling from jump");
                _playerAnime.SetBool(Falling, true);
            }
        }
        else if (!IsGrounded() && rig.velocity.y < _fallThreshold)
        {
            // Debug.Log(("Fell cuz I tripped"));
            _playerAnime.SetBool(Falling, true);
            _playerAnime.SetBool(Grounded, false);
        }
    }

    public bool IsGrounded()
    {
        return Physics.CheckCapsule(col.bounds.center,
            new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z),
            col.radius * .45f,
            platform);
    }

    private void OnCollisionEnter(Collision other)
    {
        _jump = false;
        _hasJumped = false;
        _playerAnime.SetBool(Falling, false);
        _playerAnime.SetBool(Grounded, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlayZone"))
        {
            transform.position = new Vector3(_spawn.x, 12f, _spawn.z);
        }

        if (other.gameObject.CompareTag("TimerTrigger"))
        {
            foreach (GameObject t in GameObject.FindGameObjectsWithTag("TimerTrigger"))
            {
                t.SetActive(false);
            }


            timer.TimeOffset = Time.timeSinceLevelLoad;
            timer.Run = timer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
        }
    }


    private float _limitSpeed;
    private static readonly int Move = Animator.StringToHash("move");
    private static readonly int Jump = Animator.StringToHash("jump");
    private static readonly int Grounded = Animator.StringToHash("isGrounded");
    private static readonly int Falling = Animator.StringToHash("falling");
   
    [SerializeField ]private float _fallThreshold = -5f;

    private void FixedUpdate()
    {
        _camera.Yaw = _yaw;
        _camera.Height = _pitch;


        var vel = rig.velocity + _direction;

        if (vel.sqrMagnitude > maxSpeed * maxSpeed)
            vel = vel.normalized * maxSpeed;

        if (_jump && !_hasJumped)
        {
            rig.velocity = vel * 0.5f;
            rig.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _hasJumped = true;
            _limitSpeed = Mathf.Clamp(vel.magnitude, 2f, maxSpeed);
        }
        else
        {
            if (_jump)
            {
                rig.velocity = Vector3.Lerp(rig.velocity, vel.normalized * (_limitSpeed * .9f), Time.deltaTime * 1.25f);
            }
            else
                rig.velocity = Vector3.Lerp(rig.velocity, vel, Time.deltaTime * 2);
        }

        if (rig.velocity.y < -.2f)
        {
            rig.velocity = Vector3.Lerp(rig.velocity,
                vel.normalized * (maxSpeed * .2f) + Vector3.up * -fallSpeed,
                Time.deltaTime);
        }

        // Velocity decay in _direction for fast stopping
        if (_direction == Vector3.zero)
        {
            var velocity = rig.velocity;
            vel = new Vector3(velocity.x * .2f, velocity.y, velocity.z * .2f);
            rig.velocity = Vector3.Lerp(velocity, vel, Time.deltaTime * 3);
            _playerAnime.SetFloat(Move, 0f);
        }
        else
        {
            _facingRot.Set(rig.velocity.x, 0f, rig.velocity.z);
            _playerAnime.SetFloat(Move, rig.velocity.magnitude);
        }


        if (_facingRot != Vector3.zero)
            _playerMesh.localRotation = Quaternion.Slerp(
                _playerMesh.localRotation,
                Quaternion.LookRotation(_facingRot),
                Time.deltaTime * 10);
    }
}