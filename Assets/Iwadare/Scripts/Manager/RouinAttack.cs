﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RouinAttack : MonoBehaviour
{
    [SerializeField] Animator _animRouin;
    [SerializeField] SpriteRenderer _kenSprite;
    [SerializeField] float _gapX = -5f;
    [SerializeField] float _gapY = 0.5f;
    [SerializeField] float _moveTime = 0.15f;
    [SerializeField] float _lifeTime = 5f;
    [SerializeField] AnimationClip _clip;
    float _damagePower = 15f;
    float _currentLifeTime;
    EnemyBase _enemy;
    bool _isReverse = false;

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectWithTag("Boss").GetComponent<EnemyBase>();
        _animRouin.transform.position = _enemy.transform.position;
        _animRouin.transform.SetParent(_enemy.transform);
        var pos = _animRouin.transform.localPosition;
        var scale = _animRouin.transform.localScale;
        _isReverse = RamdomMethod.RandomNumber99() < 50 ? true : false;
        if (_isReverse)
        {
            pos.x = -_gapX;
            scale.x *= -1;
        }
        else
        {
            pos.x = _gapX;
        }
        pos.y = RamdomMethod.RamdomNumberMinMax(-_gapY,_gapY);
        _animRouin.transform.localPosition = pos;
        _animRouin.transform.localScale = scale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRouin(float power)
    {
        _damagePower = power;
        _animRouin.Play(_clip.name);
    }

    public void MoveRouin()
    {
        if (_isReverse)
        {
            transform.DOLocalMoveX(_gapX, _moveTime).SetLink(gameObject);
        }
        else
        {
            transform.DOLocalMoveX(-_gapX, _moveTime).SetLink(gameObject);
        }
    }

    public void EnemyDamage()
    {
        _enemy.PerforateDamage(_damagePower);
        StartCoroutine(FadeOutRouin());
    }

    IEnumerator FadeOutRouin()
    {
        var rouinSprite = _animRouin.GetComponent<SpriteRenderer>();
        _kenSprite.DOFade(0f,_lifeTime);
        yield return rouinSprite.DOFade(0f,_lifeTime).WaitForCompletion();
        Destroy(this.gameObject);
    }
}
