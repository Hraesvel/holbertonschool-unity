using UnityEngine;
using UnityEngine.UI;

internal class Controllable : IControllable
{
    private Vector2 _dir;

    public Controllable()
    {
        Sensitivity = 1;
        _dir = new Vector2(0,0);
    }

    public Image StartImage { get; set; }

    public Image ToImage { get; set; }

    public Vector2 StartPos { get; set; }

    public Vector2 ToPos { get; set; }

    public float Sensitivity { get; set; }

    public bool TouchController(ref Vector3 dir)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);


            if (touch.phase == TouchPhase.Began)
            {
                StartImage.gameObject.SetActive(true);
                ToImage.gameObject.SetActive(true);

                StartPos = touch.position;
                StartImage.rectTransform.position = StartPos;
                ToImage.rectTransform.position = StartPos;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                ToPos = touch.position;
                ToImage.rectTransform.position = ToPos;

                _dir.Set(ToPos.x - StartPos.x, ToPos.y - StartPos.y);

                if (_dir.magnitude > 10)
                {
                    dir.x = Mathf.Clamp(_dir.x * .01f * Sensitivity, -1, 1);
                    dir.z = Mathf.Clamp(_dir.y * .01f * Sensitivity, -1, 1);
                }
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
                if (_dir.magnitude > 10)
                {
                    dir.x = Mathf.Clamp(_dir.x * .01f * Sensitivity, -1, 1);
                    dir.z = Mathf.Clamp(_dir.y * .01f * Sensitivity, -1, 1);
                }
            }
        }
        else
        {
            StartImage.gameObject.SetActive(false);
            ToImage.gameObject.SetActive(false);
            dir.x = 0f;
            dir.z = 0f;
            return false;
        }

        return true;
    }

    public bool KeyController(ref Vector3 dir)
    {
        if (!Input.anyKey) return false;
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) dir.x += 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) dir.x -= 1;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) dir.z += 1;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) dir.z -= 1;

        return true;
    }
}