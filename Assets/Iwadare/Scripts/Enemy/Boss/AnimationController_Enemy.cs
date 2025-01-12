using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimationController_Enemy : MonoBehaviour
{
    public AnimationType _animType;
    public Animator _objAnimator;
    public Spine.AnimationState _spineAnimationState;
    public Spine.Skeleton _skeleton;
    public SkeletonAnimation skeletonAnimation;
    [SpineAnimation]
    public string _idleAnimationName; 
    [SpineAnimation]
    public string _changeAnimationName; 
    [SpineAnimation]
    public string _runAnimationName;
    [SpineAnimation]
    public string _damageAnimationName;

    public void Awake()
    {
        if (_animType == AnimationType.SkeletonAnimator)
        {
            _spineAnimationState = skeletonAnimation.AnimationState;
            _skeleton = skeletonAnimation.Skeleton;
        }
        gameObject.SetActive(false);
    }
}

public enum AnimationType
{
    NormalAnimator,
    SkeletonAnimator,
}
