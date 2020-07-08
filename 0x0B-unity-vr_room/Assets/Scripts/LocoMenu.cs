using UnityEngine;
using UnityEngine.XR;

public class LocoMenu : MonoBehaviour
{
    private InputDevice _inputDevice;

    private Vector3 _spawnPos;
    private Quaternion _spawnRot;
    [SerializeField] private Transform camera;

    public DominateHand hand;

    private bool isOpen;
    // Start is called before the first frame update

    [SerializeField] private Transform menu;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (!_inputDevice.isValid)
            ConfigHand();


        if (_inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out var thumb) &&
            thumb)
        {
            if (!isOpen)
            {
                isOpen = true;
                _spawnPos = transform.position;
                _spawnRot = Quaternion.LookRotation(_spawnPos - camera.position);
            }


            menu.gameObject.SetActive(true);
            menu.position = _spawnPos;
            menu.rotation = _spawnRot;
        }
        else
        {
            isOpen = false;
            menu.gameObject.SetActive(false);
        }
    }

    // private void InitHand()
    // {
    //     _eventdata = new PointerEventData(EventSystem.current);
    //     if (XRDevice.isPresent)
    //     {
    //         var inputs = new List<InputDevice>();
    //         InputDevices.GetDevicesAtXRNode(inputDevice, inputs);
    //         if (inputs.Count == 1)
    //             _inputDevice = inputs[0];
    //     }
    // }

    private void ConfigHand()
    {
        if (hand != XRInputSingleton.Instance.DominateHand)
            gameObject.SetActive(false);

        _inputDevice = hand == DominateHand.Left
            ? XRInputSingleton.Instance.LeftDevice
            : XRInputSingleton.Instance.RightDevice;
    }
}