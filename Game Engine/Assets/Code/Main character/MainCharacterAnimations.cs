using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterAnimations : MonoBehaviour
{
    void Start()
    {
        // Set animators
    }

    public void SetAnimation(MainCharacterAnimation animation)
    {
        Debug.Log(animation.ToString());
        // Set animation.ToString() anim
    }
}

public enum MainCharacterAnimation
{
    IDLE,
    LONGIDLE,
    WALKING,
    RUNNING,
    CROUCH,
    WALKINGCROUCHED
}