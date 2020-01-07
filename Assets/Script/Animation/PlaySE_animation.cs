using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySE_animation : MonoBehaviour
{
    [SerializeField] AudioSource audioSource_;
    [SerializeField] AudioClip[] clips_;

    public void PlaySE_animationEvent(int clipNum)
    {
        audioSource_.PlayOneShot(clips_[clipNum]);
    }

    public void StopSE_animationEvent()
    {
        audioSource_.Stop();
    }
}
