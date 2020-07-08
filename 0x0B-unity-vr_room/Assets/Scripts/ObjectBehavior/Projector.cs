using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ObjectBehavior
{
    public class Projector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        public enum ProjectorState
        {
            Broken,
            NoPower,
            Off,
            On,
        }

        [Tooltip("Initial Door State")] [SerializeField]
        private ProjectorState state = ProjectorState.Off;


        [InspectorName("Operation Range")] public float opRang = 1.5f;

        // private Animator _anim;
        [SerializeField] private GameObject particles;
        private MatSwap _matSwap;

        [SerializeField] private Color glowClr = Color.red;
        [SerializeField] private float glowInts = .6f;
        private static readonly int Use = Animator.StringToHash("use");

        private GameObject _sign;
        private Image _image;
        [SerializeField] private Sprite repair;
        [SerializeField] private Sprite power;


        public ProjectorState State
        {
            get => state;
        }

        private void Awake()
        {
            _matSwap = GetComponent<MatSwap>();
            if (_matSwap == null)
                _matSwap = gameObject.AddComponent<MatSwap>();
            _matSwap.glowClr = glowClr;
            _matSwap.glowInts = glowInts;
            _sign = transform.GetChild(0).gameObject;
            _image = _sign.transform.GetChild(0).GetComponent<Image>();

            switch (State)
            {
                case ProjectorState.Broken:
                    _image.sprite = repair;
                    break;
                case ProjectorState.NoPower:
                    _image.sprite = power;
                    break;
                case ProjectorState.On:
                    particles.SetActive(true);
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
                case ProjectorState.Broken:
                    _sign.SetActive(true);
                    _image.sprite = repair;
                    _matSwap.glowClr = Color.gray;
                    _matSwap.glowInts = .4f;
                    break;
                case ProjectorState.NoPower:
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
            Debug.Log("Click!");
            switch (State)
            {
                case ProjectorState.Broken:
                    Debug.Log("Projector is broken.");
                    break;
                case ProjectorState.NoPower:
                    Debug.Log("Projector needs power to unlock dead bolts.");
                    break;
                case ProjectorState.Off:
                    state = ProjectorState.On;
                    particles.SetActive(true);
                    _matSwap.glowClr = Color.gray;
                    _matSwap.glowInts = .3f;
                    
                    break;
                case ProjectorState.On:
                    state = ProjectorState.Off;
                    particles.SetActive(false);
                    // particles.GetComponent<ParticleSystem>().Play();
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