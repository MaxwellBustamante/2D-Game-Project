using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimboyAnimation : MonoBehaviour
{
    public Animator PlayerAnimator;
    public string WalkTrigger = "SetWalk", JumpTrigger = "SetJump", ClimbTrigger = "SetClimb";

    public void SetAnimationTrigger(string TriggerName)
    {
        PlayerAnimator.SetTrigger(TriggerName);
    }

    void Start()
    {
        if (PlayerAnimator == null)
        {
            PlayerAnimator = GetComponent<Animator>();
        } 
    }

    [ContextMenu("TestWalk")]
    public void SetWalk()
    {
        SetAnimationTrigger(WalkTrigger);
    }

    [ContextMenu("TestJump")]
    public void SetJump()
    {
        SetAnimationTrigger(JumpTrigger);
    }

    [ContextMenu("TestClimb")]
    public void SetClimb()
    {
        SetAnimationTrigger(ClimbTrigger);
    }

    void Update()
    {
        
    }
}
