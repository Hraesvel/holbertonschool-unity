using UnityEngine;

public class Card : MonoBehaviour
{
    /// <summary>
    ///     url for object to open on trigger
    /// </summary>
    private void Update()
    {
        if (Input.touches.Length > 0)
        {
            var ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            if (Physics.Raycast(ray, Mathf.Infinity))
            {
                var obj = Physics.RaycastAll(ray, Mathf.Infinity);
                foreach (var o in obj)
                    if (o.collider.CompareTag("Icon"))
                        o.transform.GetComponent<BCardInteraction>().InvokeAction();
            }
        }
    }
}