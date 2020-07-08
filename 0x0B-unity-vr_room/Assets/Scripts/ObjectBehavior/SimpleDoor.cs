using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObjectBehavior
{
    public class SimpleDoor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        public enum DoorState
        {
            Broken,
            NoPower,
            Closed,
            Open,
        }

        [Tooltip("Initial Door State")] [SerializeField]
        private DoorState state = DoorState.Closed;

        [SerializeField] private string openedStateName;
        [SerializeField] private string closedStateName;

        [InspectorName("Operation Range")] public float opRang = 1.5f;
        private Animator _anim;
        private MatSwap _matSwap;

        [SerializeField] private Color glowClr = Color.red;
        [SerializeField] private float glowInts = .6f;
        private static readonly int Use = Animator.StringToHash("use");

        private GameObject _sign;
        private Image _image;
        [SerializeField] private Sprite repair;
        [SerializeField] private Sprite power;


        public DoorState State
        {
            get => state;
        }

        private void Awake()
        {
            _anim = transform.parent.GetComponent<Animator>();
            if (GetComponent<MatSwap>() == null)
                _matSwap = gameObject.AddComponent<MatSwap>();
            _matSwap.glowClr = glowClr;
            _matSwap.glowInts = glowInts;
            _sign = transform.GetChild(0).gameObject;
            _image = _sign.transform.GetChild(0).GetComponent<Image>();
            
            _anim.Play(closedStateName, -1, 0f);
            switch (State)
            {
                case DoorState.Broken:
                    _image.sprite = repair;
                    break;
                case DoorState.NoPower:
                    _image.sprite = power;
                    break;
                case  DoorState.Open:
                    _anim.Play(openedStateName, -1, 0f);
                    break;
                default:
                    break;
            }

            _sign.SetActive(false);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            switch (State)
            {
                case DoorState.Broken:
                    _sign.SetActive(true);
                    _image.sprite = repair;
                    _matSwap.glowClr = Color.gray;
                    _matSwap.glowInts = .4f;
                    break;
                case DoorState.NoPower:
                    _sign.SetActive(true);
                    _image.sprite = power;
                    _matSwap.glowClr = Color.gray;
                    _matSwap.glowInts = .4f;
                    break;
                default:
                    _matSwap.glowClr = glowClr;
                    _matSwap.glowInts = glowInts;
                    break;
            }

            _matSwap.HighlighMat(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _sign.SetActive(false);
            _matSwap.HighlighMat(false);
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            // Debug.Log("Click!");
            switch (State)
            {
                case DoorState.Broken:
                    Debug.Log("Door is broken.");
                    break;
                case DoorState.NoPower:
                    Debug.Log("Door needs power to unlock dead bolts.");
                    break;
                case DoorState.Closed:
                    // open door
                    state = DoorState.Open;
                    _anim.SetTrigger(Use);
                    _matSwap.glowClr = Color.gray;
                    _matSwap.glowInts = .3f;
                    break;
                case DoorState.Open:
                    // close door
                    state = DoorState.Closed;
                    _anim.SetTrigger(Use);
                    _matSwap.glowClr = glowClr;
                    _matSwap.glowInts = glowInts;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            return;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            return;
        }
    }
}