using Spine;
using Spine.Unity;
using System;
using System.Collections;
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

    bool _isAttackTime = false;
    bool _isParticlePlaying = false;
    [SerializeField] ParticleSystem _attackParticle;

    [SpineEvent]
    [SerializeField] string _attackEventName = "1";
    [SpineAnimation]
    public string _runAnimationName;
    [SpineAnimation]
    public string _damageAnimationName;

    [SerializeField] AnimationClip _idleAnimatior;
    [SerializeField] AnimationClip _moveAnimator;
    [SerializeField] AnimationClip _parryAnimator;
    [SerializeField] AnimationClip _deathAnimator;

    public AnimationName _initialName;

    public void Awake()
    {
        if (_animType == AnimationType.SkeletonAnimator)
        {
            _spineAnimationState = _skeletonAnimation.AnimationState;
            _skeleton = _skeletonAnimation.Skeleton;
        }
        if (_attackParticle) _attackParticle.Stop();
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        if (_animType == AnimationType.SkeletonAnimator)
        {
            _skeletonAnimation.AnimationState.Event += AddAttackAnimationEvent;
            _skeletonAnimation.AnimationState.End += RemoveAttackAnimationEvent;
        }
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
                if (_damageAnimationName != "") _spineAnimationState.SetAnimation(0, _damageAnimationName, false);
                break;
            case AnimationName.Attack:
                if (_attackAnimationName != "")
                {
                    _spineAnimationState.SetAnimation(0, _attackAnimationName, true);
                    _isAttackTime = true;
                }
                break;
        }
    }

    public void ChangeAnimationAnimator(AnimationName animation)
    {
        if (!_objAnimator) return;

        switch (animation)
        {
            case AnimationName.Idle:
                _objAnimator.Play(_idleAnimatior.name);
                break;
            case AnimationName.Run:
                _objAnimator.Play(_moveAnimator.name);
                break;
            case AnimationName.Parry:
                _objAnimator.Play(_parryAnimator.name);
                break;
            case AnimationName.Damage:
                _objAnimator.Play(_deathAnimator.name);
                break;
        }
    }

    public void AddAttackAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if(e.Data.Name == _attackEventName)
        {
            if (_isParticlePlaying)
            {
                _attackParticle.Stop();
                _isParticlePlaying = false;
            }
            else
            {
                _attackParticle.Play();
                _isParticlePlaying = true;
            }
        }
    }

    public void RemoveAttackAnimationEvent(TrackEntry trackEntry)
    {
        if(_isAttackTime)
        {
            _attackParticle.Stop();
            _isParticlePlaying = false;
            _isAttackTime = false;
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
    Attack,
    Parry,
}
