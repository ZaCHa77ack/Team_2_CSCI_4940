using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedSetter : MonoBehaviour
{
    public string AnimationSpeedParameterName = "AnimationSpeed";

    public float AnimationOverrideSpeed = -1;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        animator.SetFloat(AnimationSpeedParameterName, AnimationOverrideSpeed);
    }
}
