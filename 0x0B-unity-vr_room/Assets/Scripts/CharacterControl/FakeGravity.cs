using System;
using UnityEngine;

namespace CharacterControl
{
    public class FakeGravity : MonoBehaviour
    {
        [SerializeField] private Collider[] colliders;

        [SerializeField] private Collider col;

        private bool _grounded;
        private int ignore;

        // Start is called before the first frame update
        void Start()
        {
            colliders = new Collider[5];
            ignore = 1 << 5;
            ignore = 1 << 10;
            ignore = ~ignore;
        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log($"g = {_grounded}");
        }

        private Vector3 vel;
        private void FixedUpdate()
        {
            vel += Physics.gravity * Time.deltaTime;
            
            if (
                Physics.OverlapBoxNonAlloc(
                    transform.position,
                    col.bounds.extents,
                    colliders,
                    transform.rotation,
                    ignore
                ) > 0) {
                
                foreach (var c in colliders)
                {
                    if (c == null)
                        continue;
                    if (c.gameObject.layer == 13)
                        if (c.CompareTag("Ground"))
                            _grounded = true;
                    
                }
            }

            if (!_grounded)
            {
                transform.position += vel * Time.deltaTime;
            }

            _grounded = false;
        }
    }
}