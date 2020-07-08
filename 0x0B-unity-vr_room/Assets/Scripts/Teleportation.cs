using System;
using UnityEngine;
using UnityEngine.XR;

namespace AG
{
    public class Teleportation : MonoBehaviour
    {
        private InputDevice _inputDevice;
        private bool TurnPlayer;
        [SerializeField] private int collisionLayer;

        [SerializeField] private bool debugTarget;
        [SerializeField] private DominateHand hand;
        [SerializeField] private Transform head, cameraRig;
        [SerializeField] private RaycastInput _raycastInput;
        [SerializeField] private Color invalidColor = Color.red;
        [SerializeField] private LineRenderer laser;
        [SerializeField] private float laserSegmentDistance = 1f, dropPerSegment = .1f;
        [SerializeField] private int laserSteps = 30;
        [SerializeField] private float laserWidth = .01f;
        [SerializeField] private Material laserMat;
        [SerializeField] private float maxHeigh = 1.5f;
        [SerializeField] private float minHeigh = -5f;

        private bool targetAcquired;
        [SerializeField] private GameObject targetIcon;

        private Vector3 targetPos;

        [SerializeField] private Color validColor = Color.green;

        public InputDevice HandDevice => _inputDevice;

        private void Awake()
        {
            if (laser == null)
                laser = gameObject.AddComponent<LineRenderer>();


            laser.positionCount = laserSteps;
            laser.startWidth = laser.endWidth = laserWidth;
            laser.material = laserMat;
        }


        private void InitXr()
        {
            if (hand == DominateHand.Left)
                _inputDevice = XRInputSingleton.Instance.LeftDevice;
            else
                _inputDevice = XRInputSingleton.Instance.RightDevice;
        }


        private void Update()
        {
            if (!_inputDevice.isValid)
                InitXr();

            if (debugTarget)
            {
                TryToGetTeleportTarget();
                laser.startWidth = laser.endWidth = laserWidth;
                return;
            }


            if (_inputDevice.isValid)
            {
                if (_inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out var rh_primary2d))

                    if (rh_primary2d.x > .8f || rh_primary2d.x < -.8f)
                    {
                        TurnPlayer = true;
                    }
                    else if (TurnPlayer)
                    {
                        var angle = new Vector3(0, rh_primary2d.x > 0 ? 35f : -35f, 0);
                        cameraRig.rotation *= Quaternion.Euler(angle);

                        TurnPlayer = false;
                    }

                if (rh_primary2d.y > .8f)
                {
                    if (_raycastInput.Enabled)
                        _raycastInput.Enabled = false;
                    TryToGetTeleportTarget();
                }
                else if (targetAcquired && rh_primary2d.y < .2f)
                {
                    _raycastInput.Enabled = true;
                    Teleport();
                }
                else if (targetAcquired == false && rh_primary2d.y < .2f) ResetLaser();
            }
        }

        public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }


        private void TryToGetTeleportTarget()
        {
            targetAcquired = false;
            var origin = transform.position;
            laser.SetPosition(0, origin);
            targetIcon.SetActive(true);

            for (var i = 0; i < laserSteps - 1; i++)
            {
                // trajectory down offset
                var offset = (transform.forward + Vector3.down * (dropPerSegment * i)).normalized *
                             laserSegmentDistance;

                if (Physics.Raycast(origin, offset, out var hit, laserSegmentDistance))
                {
                    targetIcon.transform.position = hit.point;

                    // collapse remainder points onto hit location 
                    for (var j = i + 1; j < laser.positionCount; j++) laser.SetPosition(j, hit.point);

                    if (hit.transform.gameObject.layer == collisionLayer)
                    {
                        laser.startColor = laser.endColor = validColor;
                        targetPos = hit.point;

                        targetAcquired = true;
                        return;
                    }

                    laser.startColor = laser.endColor = invalidColor;
                    return;
                }

                // perform simple arc down 
                laser.SetPosition(i + 1, origin + offset);
                origin += offset;
            }

            // targetIcon.transform.position = origin;
            laser.startColor = laser.endColor = invalidColor;
        }

        private void Teleport()
        {
            targetAcquired = false;
            ResetLaser();

            var offset = new Vector3(
                targetPos.x - head.transform.position.x,
                targetPos.y - cameraRig.position.y,
                targetPos.z - head.transform.position.z);

            cameraRig.position += offset;
        }

        private void ResetLaser()
        {
            targetIcon.SetActive(false);
            for (var i = 0; i < laser.positionCount; i++) laser.SetPosition(i, Vector3.zero);
        }
    }
}