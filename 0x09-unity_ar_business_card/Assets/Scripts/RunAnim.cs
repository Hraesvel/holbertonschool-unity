using UnityEngine;

public class RunAnim : MonoBehaviour
{
    private static readonly int OpenMenu = Animator.StringToHash("OpenMenu");
    [SerializeField] private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim.Play("Start");
    }


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            TriggerAnim();
    }

    // Update is called once per frame

    public void TriggerAnim()
    {
        if (!anim.GetBool(OpenMenu)
        )
        {
            anim.SetBool(OpenMenu, true);
        }
        else
        {
            anim.Play("Start", -1, 0f);
            anim.SetBool(OpenMenu, false);
        }
    }
}