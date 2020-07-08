using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class RaycastInput : MonoBehaviour
{
    public Transform originXform;
    public Transform heldObjXform;
    [SerializeField] private DominateHand _dominateHand = DominateHand.Right;
    public bool UseDistancePickup;

    private InputDevice _inputDevice;
    private Ray _ray;
    private RaycastResult _raycastResult;
    private RaycastHit _rayHit;

    private float _trigger;

    [SerializeField] private bool _printTrigger;
    [SerializeField] private bool DebugMode;

    private PointerEventData eventData;

    private bool isBusy;

    private LineRenderer line;
    [SerializeField] private Gradient lineColor;
    [SerializeField] private float lineDistance = 5f;
    [SerializeField] private float lineWidth = 0.1f;
    [SerializeField] private Material lineMat;

    //Temp
    [SerializeField] private Collider col;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator anim;
    private static readonly int Trigger = Animator.StringToHash("trigger");
    private static readonly int TouchTrigger = Animator.StringToHash("touchTrigger");


    //velocity calc
    private Vector3 oldPos;
    private Vector3 vel;

    private Transform heldObject;

    public bool Enabled { get; set; }


    private void Awake()
    {
        Enabled = true;
        isBusy = false;
    }

    private void Start()
    {
        if (originXform == null)
            originXform = transform;
        if (heldObjXform == null)
            heldObjXform = transform;
        eventData = new PointerEventData(EventSystem.current);
        eventData.pointerId = 0;

        line = gameObject.AddComponent<LineRenderer>();
        line.startWidth = line.endWidth = lineWidth;
        line.colorGradient = lineColor;
        line.material = lineMat;
        line.SetPosition(1, transform.forward * lineDistance);
        _ray = new Ray(transform.position, transform.forward * lineDistance);

        // if (XRInputSingleton.Instance.DominateHand == _dominateHand)
        //     this.enabled = false;
    }

    private void Update()
    {
        if (!Enabled)
        {
            if (eventData.pointerEnter != null)
            {
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
                eventData.pointerEnter = null;
            }
            line.enabled = false;
            return;
        }

        if (!_inputDevice.isValid)
            InitXr();
        
        if (!line.enabled)
            line.enabled = true;


        if (_inputDevice.TryGetFeatureValue(CommonUsages.trigger, out _trigger))
            anim.SetFloat(Trigger, _trigger);

        if (UseDistancePickup)
        {
            if (heldObject != null && _trigger > 0.8f)
            {
                vel = (originXform.position - oldPos) / Time.deltaTime;
                oldPos = originXform.position;
                return;
            }

            if (!isBusy && heldObject != null && _trigger < 0.8f)
            {
                DropObject();
                line.enabled = true;
            }
        }


        //Todo: shrink coded
        var lineDist = originXform.forward * lineDistance;
        line.SetPosition(0, originXform.position);
        line.SetPosition(1, originXform.position + lineDist);

        // Debug.DrawRay(parent.position, lineDist);

        _ray.origin = originXform.position;
        _ray.direction = lineDist;

        if (DebugMode)
        {
            line.startWidth = line.endWidth = lineWidth;
            line.colorGradient = lineColor;
            line.material = lineMat;
        }


        if (_printTrigger)
            Debug.Log($"{_dominateHand} trigger : {_trigger}");


        PerformInteraction();
        CheckHit();
    }

    private void InitXr()
    {
        switch (_dominateHand)
        {
            case DominateHand.Right:
                _inputDevice = XRInputSingleton.Instance.RightDevice;
                break;
            case DominateHand.Left:
                _inputDevice = XRInputSingleton.Instance.LeftDevice;
                break;
        }
    }


    private void PerformInteraction()
    {
        Physics.Raycast(_ray, out _rayHit, lineDistance);

        if (_rayHit.transform == null)
        {
            LookAway();
            return;
        }

        eventData.pointerCurrentRaycast = ConvertRayToRayResult(_rayHit);

        if (eventData.pointerEnter == _rayHit.transform.gameObject)
            return;

        LookAway();

        eventData.pointerEnter = _rayHit.transform.gameObject;
        ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerEnterHandler);
    }

    private void LookAway()
    {
        if (eventData.pointerEnter == null) return;
        ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
        eventData.pointerEnter = null;
    }

    private RaycastResult ConvertRayToRayResult(RaycastHit hit)
    {
        var result = new RaycastResult();
        result.distance = hit.distance;
        result.worldPosition = hit.point;
        result.worldNormal = hit.normal;
        return result;
    }

    private void CheckHit()
    {
        if (_trigger > 0.8f && eventData.pointerEnter != null)
        {
            if (UseDistancePickup)
                if (eventData.pointerEnter.CompareTag("Pickup") && !isBusy)
                {
                    PickupObject(eventData.pointerEnter.transform);
                    ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerExitHandler);
                    eventData.pointerEnter = null;
                    return;
                }

            eventData.pointerPressRaycast = eventData.pointerCurrentRaycast;
            eventData.pointerPress =
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerDownHandler);
        }
        else if (_trigger < .8f)
        {
            if (eventData.pointerPress != null)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerPress, eventData, ExecuteEvents.pointerUpHandler);

            if (eventData.pointerPress == eventData.pointerEnter && eventData.pointerPress != null &&
                eventData.pointerEnter != null)
                ExecuteEvents.ExecuteHierarchy(eventData.pointerEnter, eventData, ExecuteEvents.pointerClickHandler);

            eventData.pointerPress = null;
        }
    }

    private void DropObject()
    {
        heldObject.transform.parent = null;
        var h_rb = heldObject.GetComponent<Rigidbody>();
        h_rb.isKinematic = false;
        h_rb.WakeUp();
        h_rb.AddForce(vel, ForceMode.Impulse);
        h_rb.useGravity = true;
        StartCoroutine(DropGC(heldObject.gameObject));
    }

    IEnumerator DropGC(GameObject held)
    {
        isBusy = true;
        yield return new WaitForSeconds(.5f);
        foreach (var h_col in held.GetComponents<Collider>())
            Physics.IgnoreCollision(
                col,
                h_col,
                false);

        heldObject = null;
        isBusy = false;
    }


    private void PickupObject(Transform obj)
    {
        line.enabled = false;
        heldObject = obj;
        var component = heldObject.GetComponent<Rigidbody>();
        component.Sleep();
        component.useGravity = false;
        component.isKinematic = true;
        heldObject.position = heldObjXform.position;
        heldObject.parent = originXform;
        foreach (var h_col in heldObject.GetComponents<Collider>())
            Physics.IgnoreCollision(
                col,
                h_col,
                true);
    }
}