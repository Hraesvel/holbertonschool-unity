using UnityEngine;

namespace PlayControls
{
    public class Controller : MonoBehaviour
    {
        private Vector3 _dir;
        private Vector2 _leftAxis;
        private Vector2 _rightAxis;
        [Range(0.001f, 1f)]
        public float deadZone = 0.01f;


        public bool TryGetGamePadAxis(GamePad feature, out Vector2 axis)
        {
            bool set = false;
            switch (feature)
            {
                case GamePad.LeftAxis:
                    if (Mathf.Abs(Input.GetAxis("Horizontal")) > deadZone ||
                        Mathf.Abs(Input.GetAxis("Vertical")) > deadZone)
                    {
                        _leftAxis.x = Input.GetAxis("Vertical");
                        _leftAxis.y = Input.GetAxis("Horizontal");
                        axis = _leftAxis;
                        return true;
                    }

                    break;
                case GamePad.RightAxis:
                    if (
                        Mathf.Abs(Input.GetAxis("Axis2Horizontal")) > deadZone ||
                        Mathf.Abs(Input.GetAxis("Axis2Vertical")) > deadZone)
                    {
                        _rightAxis.x = Input.GetAxis("Axis2Horizontal");
                        _rightAxis.y = Input.GetAxis("Axis2Vertical");
                        axis = _rightAxis;
                        return true;
                    }

                    break;
                case GamePad.LeftTrigger:
                    break;
                case GamePad.RightTrigger:
                    break;
            }

            axis = Vector2.zero;
            return set;
        }
        
        public bool TryGetMouseAxis(Mouse feature, out Vector2 axis)
        {
            bool set = false;
            switch (feature)
            {
                case Mouse.Mouse1:
                    if (Input.GetMouseButton(0))
                    {
                        _leftAxis.x = Input.GetAxis("Mouse X");
                        _leftAxis.y = Input.GetAxis("Mouse Y");
                        axis = _leftAxis;
                        return true;
                    }
                    break;
                case Mouse.Mouse2:
                    // if (Input.GetMouseButton(1))
                    // {
                    //     _rightAxis.x = Input.GetAxis("Axis2Horizontal");
                    //     _rightAxis.y = Input.GetAxis("Axis2Vertical");
                    //     axis = _rightAxis;
                    //     return true;
                    // }

                    break;
                
            }

            axis = Vector2.zero;
            return set;
        }
        
        
    }

    public enum GamePad
    {
        LeftAxis,
        RightAxis,
        LeftTrigger,
        RightTrigger
    }

    public enum Mouse
    {
        Mouse1,
        Mouse2
    }
}