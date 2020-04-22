using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditorInternal;
using UnityEngine.SceneManagement;
using AnimatorController = UnityEditor.Animations.AnimatorController;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private AnimatorController animCTRL;

    [SerializeField] private AnimationClip introClip;
    
    [SerializeField] private Camera camera;
    [SerializeField] private PlayerController PlayerController;
    [SerializeField] private Timer timer;
    [SerializeField] private float _lengthOffset;

    // Start is called before the first frame update
    private void Awake()
    {
        AnimatorOverrideController ov = new AnimatorOverrideController(animCTRL);
        // var clips = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        // ov.GetOverrides(clips);

        // for (int i = 0 ; i < clips.Count; i++)
        // {
        //     if (clips[i].Key.ToString() != "Intro01")
        //         continue;
        //    
        //     clips[i] = new KeyValuePair<AnimationClip, AnimationClip>(introClip, introClip);
        // }

        ov["Intro01"] = introClip;
        ov.name = $"OVERRIDE - {introClip.name}";
        

        anim.runtimeAnimatorController = ov;
    }

    private void Start()
    {
        camera.gameObject.SetActive(false);
        PlayerController.enabled = false;
        timer.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Intro") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime > anim.GetCurrentAnimatorStateInfo(0).length + _lengthOffset
        )
        {
            this.gameObject.SetActive(false);
            camera.gameObject.SetActive(true);
            PlayerController.enabled = true;
            timer.gameObject.SetActive(true);

            // Destroy(this.gameObject);
            
        }
    }

    private void FixedUpdate()
    {
        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;
    }
}