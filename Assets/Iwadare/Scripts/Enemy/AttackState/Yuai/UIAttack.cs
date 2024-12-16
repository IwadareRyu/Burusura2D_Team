using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAttack : MonoBehaviour, AttackInterface, PauseTimeInterface
{
    [SerializeField] UIPosition[] _uiPos;
    [SerializeField] SpadeAttack _spade;
    [SerializeField] CloverAttack _clover;
    [SerializeField] HeartAttack _heart;
    [SerializeField] DaiyaAttack _daiya;
    [SerializeField] Image _timerPanel;
    Text _timerText;
    [SerializeField] Transform _centerPos;
    [SerializeField] Transform _hidePos;
    float _currentTime = 0f;
    [SerializeField] float _stayTime = 0.1f;
    [SerializeField] float _moveTime = 1.0f;
    [SerializeField] int _lookingAroundCount = 2;
    int _uiPosNumber = -1;
    List<int> _aroundNumber = new List<int>();
    [SerializeField] UIPositionState _posState;

    public void Init()
    {
        _spade.Init();
        _clover.Init();
        _heart.Init();
        _daiya.Init();
        for (int i = 0; i < _uiPos.Length; i++)
        {
            switch (_uiPos[i]._uiPosState)
            {
                case UIPositionState.LeftUp:
                    _uiPos[i]._attackScript = _clover;
                    break;
                case UIPositionState.LeftDown:
                    _uiPos[i]._attackScript = _heart;
                    break;
                case UIPositionState.RightUp:
                    _uiPos[i]._attackScript = _spade;
                    break;
                case UIPositionState.RightDown:
                    _uiPos[i]._attackScript = _daiya;
                    break;
            }
        }
        _timerText = _timerPanel.GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void StayUpdate(EnemyBase enemy)
    {
        _currentTime += Time.deltaTime * enemy._timeScale;
        if (_currentTime > _stayTime)
        {
            if (enemy._useGravity)
            {
                enemy._enemyRb.gravityScale = 0;
                enemy._enemyRb.velocity = Vector2.zero;
            }
            enemy._bossState = EnemyBase.BossState.MoveState;
        }
    }

    public IEnumerator Move(EnemyBase enemy)
    {
        SetEnemyAngle(enemy);
        /// 中央に移動
        yield return enemy.transform.DOMove(_centerPos.position, _moveTime).SetLink(enemy.gameObject).WaitForCompletion();
        enemy.ObjSetRotation(0);
        ///　キョロキョロ
        for (var i = 0; i < _lookingAroundCount; i++)
        {
            enemy.BossObjFlipX(false);
            yield return WaitforSecondsCashe.Wait(0.5f);
            enemy.BossObjFlipX(true);
            yield return WaitforSecondsCashe.Wait(0.5f);
        }
        yield return WaitforSecondsCashe.Wait(0.5f);
        /// 移動する場所決め(ランダム)
        if (_posState == UIPositionState.Random)
        {
            if (_aroundNumber.Count >= _uiPos.Length) _aroundNumber.Clear();
            var randomNumber = _uiPosNumber;
            while (true)
            {
                randomNumber = RamdomMethod.RamdomNumber(_uiPos.Length);
                bool _chackAround = false;
                /// 1順で全ての場所に移動できるよう調整。
                for (var i = 0; i < _aroundNumber.Count; i++)
                {
                    if (randomNumber == _aroundNumber[i])
                    {
                        _chackAround = true;
                        break;
                    }
                }
                if (!_chackAround)
                {
                    _aroundNumber.Add(randomNumber);
                    break;
                }
            }
            Debug.Log(randomNumber);
            _uiPosNumber = randomNumber;
        }
        else
        {
            /// 移動する場所決め(指定)
            _uiPosNumber = (int)_posState;
        }
        /// 移動
        yield return enemy.transform.DOMove(_uiPos[_uiPosNumber]._movePoint.position, _moveTime).SetLink(enemy.gameObject).WaitForCompletion();
        Debug.Log("移動しました");
        _uiPos[_uiPosNumber]._yuaiText.enabled = true;
        /// 隠れるアニメーション
        enemy.transform.position = _hidePos.position;
        enemy._bossState = EnemyBase.BossState.AttackState;
    }

    public IEnumerator Attack(EnemyBase enemy)
    {
        StartCoroutine(_uiPos[_uiPosNumber]._attackScript.Attack(enemy));
        yield return WaitforSecondsCashe.Wait(_uiPos[_uiPosNumber]._attackScript.GetAllAttackTime());
        enemy.transform.position = _uiPos[_uiPosNumber]._movePoint.position;
        _uiPos[_uiPosNumber]._yuaiText.enabled = false;
        enemy._bossState = EnemyBase.BossState.ChangeActionState;
    }

    public void ActionReset(EnemyBase enemy)
    {
        enemy.ObjSetRotation(0);
        enemy.BossObjFlipX(false);
        if (_uiPosNumber != -1)
        {
            _uiPos[_uiPosNumber]._yuaiText.enabled = false;
        }
    }

    void SetEnemyAngle(EnemyBase enemy)
    {
        var angle = GetAngle(enemy.transform, _centerPos);
        Debug.Log(angle);
        if (angle <= 90 && angle >= -90)
        {
            enemy.BossObjFlipX(false);
            enemy.ObjSetRotation(angle);
        }
        else
        {
            enemy.BossObjFlipX(true);
            enemy.ObjSetRotation(angle >= 0 ? -(180 - angle) : -(-180 - angle));
        }
    }

    float GetAngle(Transform enemyPos, Transform targetPos)
    {
        var distance = enemyPos.position - targetPos.position;
        return Mathf.Atan2(distance.y, distance.x) * Mathf.Rad2Deg;
    }

    public void StartPause()
    {

    }


    public void EndPause()
    {

    }


    public void TimeScaleChange(float timeScale)
    {

    }

    [Serializable]
    struct UIPosition
    {
        public UIPositionState _uiPosState;
        public IUIAttack _attackScript;
        public Transform _movePoint;
        public Text _yuaiText;
    }

    enum UIPositionState
    {
        LeftUp,
        RightUp,
        LeftDown,
        RightDown,
        Random,
    }
}
