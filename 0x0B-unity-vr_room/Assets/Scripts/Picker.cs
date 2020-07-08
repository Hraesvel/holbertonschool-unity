using UnityEngine;
using UnityEngine.EventSystems;

public class Picker : MonoBehaviour
{
    private bool _blocked;
    private bool _rest;

    private PointerEventData eventdata;


    private Transform parent;

    [SerializeField] private float resetSpeed = 10f;


    private void Start()
    {
        parent = transform.parent;
        eventdata = new PointerEventData(EventSystem.current);
        eventdata.pointerId = 0;
    }


    private void Update()
    {
        _rest = transform.position == transform.parent.position;

        if (!_blocked)
            GetComponent<Rigidbody>().Sleep();
    }

    private void FixedUpdate()
    {
        if (!_rest && !_blocked)
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * resetSpeed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 5)
        {
            // Debug.Log($"button : {other.gameObject.name}");
            // Debug.Log($"Entered: {obj.name}");
            var obj = other.gameObject;
            ExecuteEvents.ExecuteHierarchy(obj, eventdata,
                ExecuteEvents.pointerEnterHandler);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 5)
        {
            var obj = other.gameObject;
            // Debug.Log($"Exited: {obj.name}");
            ExecuteEvents.ExecuteHierarchy(obj, eventdata,
                ExecuteEvents.pointerExitHandler);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        _blocked = true;
        if (other.gameObject.layer == 5)
        {
            // Debug.Log($"button : {other.gameObject.name}");
            // Debug.Log($"Entered: {obj.name}");
            var obj = other.gameObject;
            ExecuteEvents.ExecuteHierarchy(obj, eventdata,
                ExecuteEvents.pointerEnterHandler);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        _blocked = false;
        if (other.gameObject.layer == 5)
        {
            // Debug.Log($"Exited: {obj.name}");
            var obj = other.gameObject;
            ExecuteEvents.ExecuteHierarchy(obj, eventdata,
                ExecuteEvents.pointerExitHandler);
        }
    }
}