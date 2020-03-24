using UnityEngine;

namespace PlayControls
{

    public class Controller : MonoBehaviour
    {
        
        private Vector3 _dir;
        private Vector2 _leftAxis;
        private Vector2 _rightAxis;
        
        /// <summary>
        /// field for adjusting the  minimum threshold of a gamepad analog sticks
        /// </summary>
        [Range(0.001f, 1f)]
        public float deadZone = 0.01f;


        /// <summary>
        /// try to check if a gamepad feature has been triggered and return a true or false
        /// </summary>
        /// <param name="feature"> GamePad type to watch for</param>
        /// <param name="axis"> data to read out of the method</param>
        /// <returns>True if GamePad feature was registered else false</returns>
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
        
        /// <summary>
        /// try to check if a mouse feature has been triggered and return a true or false
        /// </summary>
        /// <param name="feature"> Mouse Input type to watch for</param>
        /// <param name="axis"> data to read out of the method</param>
        /// <returns>True if mouse button was registered else false</returns>
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

    /// <summary>
    /// Gamepad features
    /// </summary>
    public enum GamePad
    {
        LeftAxis,
        RightAxis,
        LeftTrigger,
        RightTrigger
    }

    /// <summary>
    /// Mouse features
    /// </summary>
    public enum Mouse
    {
        Mouse1,
        Mouse2
    }
}