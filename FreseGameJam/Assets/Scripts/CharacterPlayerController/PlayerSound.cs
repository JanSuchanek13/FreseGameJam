using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [Header("Walking Sound Array:")]
    [SerializeField] AudioSource[] arrayOfWalkingSounds;
    private AudioSource currentSound;
    bool isPlaying = false;

    [Header("REFERENCE")]
    [SerializeField] AudioSource falling_Sound;
    private Animator animator;
    private ThirdPersonMovement Movement;

    void Start()
    {
        animator = GetComponent<Animator>();
        //Debug.Log(animator);

        Movement = GetComponentInParent<ThirdPersonMovement>();
        //Debug.Log(Movement);
    }

    // Update is called once per frame
    void Update()
    {
        WalkingSound();
        FallingSound();
    }

    private void WalkingSound()
    {
        if (Movement.walking)
        {
            PlayArray();
        }
    }

    private void FallingSound()
    {
        if (Movement.falling && !falling_Sound.isPlaying)
        {
            falling_Sound.Play();
        }
    }

    private void PlayArray()
    {
        if (!isPlaying || !currentSound.isPlaying)
        {
            currentSound = arrayOfWalkingSounds[Random.Range(0, arrayOfWalkingSounds.Length)];
            currentSound.Play();
            isPlaying = true;
        }
    }
}
