using Spine.Unity;
using System;
using UnityEngine;

[Serializable]
public class AnimationController_Enemy : MonoBehaviour
{
    public AnimationType _animType;
    public Animator _objAnimator;
    public Spine.AnimationState _spineAnimationState;
    public Spine.Skeleton _skeleton;
    public SkeletonAnimation _skeletonAnimation;
    [SpineAnimation]
    public string _idleAnimationName;
    [SpineAnimation]
    public string _changeAnimationName;
    [SpineAnimation]
    public string _attackAnimationName;
    [SpineAnimation]
    public string _runAnimationName;
    [SpineAnimation]
    public string _damageAnimationName;

    public void Awake()
    {
        if (_animType == AnimationType.SkeletonAnimator)
        {
            _spineAnimationState = _skeletonAnimation.AnimationState;
            _skeleton = _skeletonAnimation.Skeleton;
        }
        gameObject.SetActive(false);
    }

    public void ChangeAnimationSpain(AnimationName animation)
    {
        if (!_skeletonAnimation) return;

        switch (animation)
        {
            case AnimationName.Idle:
                if (_idleAnimationName != "") _spineAnimationState.SetAnimation(0, _idleAnimationName, true);
                break;
            case AnimationName.Change:
                if (_changeAnimationName != "") _spineAnimationState.SetAnimation(0, _changeAnimationName, false);
                break;
            case AnimationName.Run:
                if (_runAnimationName != "") _spineAnimationState.SetAnimation(0, _runAnimationName, true);
                break;
            case AnimationName.Damage:
                if (_damageAnimationName != "") _spineAnimationState.SetAnimation(0, _damageAnimationName, true);
                break;
            case AnimationName.Attack:
                if (_attackAnimationName != "") _spineAnimationState.SetAnimation(0, _attackAnimationName, true);
                break;
        }
    }
}

public enum AnimationType
{
    NormalAnimator,
    SkeletonAnimator,
}

public enum AnimationName
{
    Idle,
    Change,
    Run,
    Damage,
    Attack
}
