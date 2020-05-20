using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class AnimaBCard : MonoBehaviour, ITrackableEventHandler
{
    private TrackableBehaviour _mTrackableBehaviour;

    private static readonly int OpenMenu = Animator.StringToHash("OpenMenu");
    [SerializeField] private List<Animator> anim = new List<Animator>();


    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("Track state changed to something!");

        if (anim.Count == 0)
            return;

        if (
            newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED
        )
        {
            if (anim[0].GetBool(OpenMenu)) return;
            Debug.Log("Opening Menu");
            foreach (var a in anim)
                a.SetBool(OpenMenu, true);
        }
        else
        {
            foreach (var a in anim)
            {
                a.Play("Start", -1, 0f);
                a.SetBool(OpenMenu, false);
            }
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        _mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (_mTrackableBehaviour)
        {
            _mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
}