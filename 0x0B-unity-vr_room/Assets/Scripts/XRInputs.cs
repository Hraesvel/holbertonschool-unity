using System;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.XR;

public class XRInputs : MonoBehaviour
{
    private Vector3 _lhPosistion;
    private Quaternion _lhRotation;
    private Vector3 _rhPosistion;
    private Quaternion _rhRotation;
    private Vector3 hmd_position;
    private Quaternion hmd_rotation;
    public XRInputSingleton Instance;
    public Transform m_Camera;
    public Transform m_Lefthand;
    public Transform m_Righthand;
    public Transform m_Body;

    public (Vector3, Quaternion ) HmdXform => (hmd_position, hmd_rotation);
    public (Vector3, Quaternion) LeftDeviceXform => (_lhPosistion, _lhRotation);
    public (Vector3, Quaternion) RightDeviceXform => (_rhPosistion, _rhRotation);

    private void Awake()
    {
        lock (XRInputSingleton.Instance)
        {
            Instance = XRInputSingleton.Instance;
        }

        lock (Instance)
        {
            Instance.FetchDevices();
            var subsystems = new List<XRInputSubsystem>();
            SubsystemManager.GetInstances(subsystems);
            if (subsystems.Count == 0) Debug.Log("failed");
            foreach (var t in subsystems)
                t.TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
            
        }
    }

    private void Update()
    {
        if (!Instance.IsValid)
            if (Instance != null)
                lock (Instance)
                {
                    if (!Instance.FetchDevices() && Instance.IsValid)
                    {
                        Debug.Log("LOSER!");
                        return;
                    }
                }


        // Track primary device features
        TrackPrimaryDevices();
    }

    void TrackPrimaryDevices()
    {
        if (Instance.Hmd.TryGetFeatureValue(CommonUsages.centerEyePosition, out hmd_position) &&
            Instance.Hmd.TryGetFeatureValue(CommonUsages.centerEyeRotation, out hmd_rotation))
        {
            m_Camera.localPosition = hmd_position;
            m_Camera.localRotation = hmd_rotation;

            // m_Body.localPosition = new Vector3(hmd_position.x,  m_Body.localPosition.y, hmd_position.z);
            // m_Camera.localPosition = hmd_position;
            // m_Camera.localPosition= new Vector3(m_Body.localPosition.x, hmd_position.y, m_Body.localPosition.z);
        }

        if (Instance.LeftDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _lhPosistion) &&
            Instance.LeftDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _lhRotation)
        )
        {
            m_Lefthand.localPosition = _lhPosistion;
            m_Lefthand.localRotation = _lhRotation;
        }

        if (Instance.RightDevice.TryGetFeatureValue(CommonUsages.devicePosition, out _rhPosistion) &&
            Instance.RightDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out _rhRotation))
        {
            m_Righthand.localPosition = _rhPosistion;
            m_Righthand.localRotation = _rhRotation;
        }
    }
}

public sealed class XRInputSingleton : IDisposable
{
    private static XRInputSingleton _instance;
    private static readonly object padlock = new object();

    private XRInputSingleton()
    {
        DominateHand = DominateHand.Right;
        var leftDevices = new List<InputDevice>();
        var rightDevices = new List<InputDevice>();
        var headMountDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.Head, headMountDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (headMountDevices.Count == 1) Hmd = headMountDevices[0];
        if (leftDevices.Count == 1) LeftDevice = leftDevices[0];
        if (rightDevices.Count == 1) RightDevice = rightDevices[0];
    }

    public static XRInputSingleton Instance
    {
        get
        {
            lock (padlock)
            {
                if (_instance == null) _instance = new XRInputSingleton();
                return _instance;
            }
        }
    }

    public bool IsValid
    {
        get
        {
            if (!Hmd.isValid)
            {
                Debug.Log("No Xr Devices or HMD were found");
                return false;
            }

            if (!LeftDevice.isValid)
            {
                Debug.Log("Failed to get Left Hand Device");
                return false;
            }

            if (!RightDevice.isValid)
            {
                Debug.Log("Failed to get Left Hand Device");
                return false;
            }

            return true;
        }
    }

    public DominateHand DominateHand { get; }

    public InputDevice Hmd { get; private set; }

    public InputDevice LeftDevice { get; private set; }

    public InputDevice RightDevice { get; private set; }

    public void Dispose()
    {
        _instance = null;
    }

    public bool FetchDevices()
    {
        var headMountDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, headMountDevices);
        if (headMountDevices.Count != 1) return false;
        Hmd = headMountDevices[0];
        var leftDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftDevices);
        if (leftDevices.Count != 1) return false;
        LeftDevice = leftDevices[0];
        var rightDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightDevices);
        if (rightDevices.Count != 1) return false;
        RightDevice = rightDevices[0];
        return true;
    }
}

public enum DominateHand
{
    Left,
    Right
}