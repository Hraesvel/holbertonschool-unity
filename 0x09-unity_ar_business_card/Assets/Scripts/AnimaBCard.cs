using UnityEngine;
using Vuforia;

public class AnimaBCard : MonoBehaviour, ITrackableEventHandler
{

    private TrackableBehaviour _mTrackableBehaviour;
    
    private static readonly int OpenMenu = Animator.StringToHash("OpenMenu");
    [SerializeField] private Animator anim;
    
    
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("Track state changed to something!");

        if (!anim)
            return;

        if (
            newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED
        )
        {
            
            if (anim.GetBool(OpenMenu)) return;
            Debug.Log("Opening Menu");
            anim.SetBool(OpenMenu, true);
        }
        else
        {
            anim.Play("Start", -1, 0f);
            anim.SetBool(OpenMenu, false);
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