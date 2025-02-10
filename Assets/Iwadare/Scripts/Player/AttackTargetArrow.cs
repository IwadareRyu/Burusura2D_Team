using Cinemachine;
using System.Collections;
using UnityEngine;

public class AttackTargetArrow : MonoBehaviour
{
    PlayerController _controller;
    [SerializeField] CinemachineVirtualCamera _playerCamera;
    CinemachineFramingTransposer _playerCameraFraming;
    [SerializeField] float _rotateGap = 90f;
    [SerializeField] Transform _arrowObj;
    [SerializeField] PlayerAttack _attackScript;
    [SerializeField] PlayerSlashing _slashingScript;
    [Tooltip("攻撃間のインターバル"),Header("攻撃間のインターバル")]
    [SerializeField] float _attackInterval = 0.2f;
    [Tooltip("攻撃の有効入力時間"),Header("攻撃の有効入力時間")]
    [SerializeField] float _attackVaildInputTime = 2f;
    [SerializeField] int MaxAttackCount = 3;
    [SerializeField] float _stopPlayerCameraMove = 0.25f;
    [SerializeField] float _movePlayerCameraMove = 0.15f;
    public void Init(PlayerController playerController)
    {
        _controller = playerController;
        PlayerDirection(playerController.PlayerObj.transform.localScale);
        _playerCameraFraming = _playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (GameStateManager.Instance.GameState == GameState.InBattleState)
    //    {
    //        _x = Input.GetAxisRaw("Horizontal");
    //        _y = Input.GetAxisRaw("Vertical");
    //    }
    //}

    public void ArrowUpdate(PlayerController controller)
    {
        if (controller.X != 0 || controller.Y != 0)
        {
            RotationArrow(controller.X, controller.Y,controller._upPlayerAnim.transform);
        }
        else
        {
            CameraMove(0);
        }
    }

    /// <summary>弾を出す向きの矢印を調整するメソッド</summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void RotationArrow(float x, float y,Transform _upBody)
    {
        var rota = transform.eulerAngles;
        rota.z = ArrowDirection(x, y);
        transform.eulerAngles = rota;
        var bodyrota = _upBody.localEulerAngles;
        bodyrota.z = Mathf.Max(Mathf.Min(90 * Mathf.Cos(rota.z * Mathf.Deg2Rad) / 2,40f),-40f);
        _upBody.localEulerAngles = bodyrota;
    }

    /// <summary>上下左右入力に応じて向きを変えるメソッド</summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    float ArrowDirection(float x, float y)
    {
        var rad = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        CameraMove(x);
        if (_controller.IsGround && rad < 0)
        {
            var dir = _controller.PlayerObj.transform.localScale.x;
            CameraMove(dir / Mathf.Abs(dir));
            return _rotateGap * (dir / Mathf.Abs(dir));
        }   // 下方向入力で、Playerが地面についている場合、Playerが向いている方向を返す。
        else
        {
            CameraMove(x);
            return rad + _rotateGap;
        }
    }

    /// <summary>左右入力時、カメラのX軸調整</summary>
    /// <param name="x"></param>
    void CameraMove(float x)
    {
        if (Input.GetButton("StopMove"))
        {
            _playerCameraFraming.m_ScreenX = 0.5f - x * _stopPlayerCameraMove;
        }
        else
        {
            _playerCameraFraming.m_ScreenX = 0.5f - x * _movePlayerCameraMove;
        }
    }

    /// <summary>接地時、下方向に矢印がある場合、矢印の方向をPlayerの向いている方向にリセットする処理</summary>
    public void ResetDirection()
    {
        var difX = _arrowObj.transform.position.x - transform.position.x;
        var difY = _arrowObj.transform.position.y - transform.position.y;
        if (Mathf.Atan2(difX, difY) * Mathf.Rad2Deg < 0)
        {
            PlayerDirection(_controller.PlayerObj.transform.localScale);
        }
    }

    /// <summary>初期の向きを設定するときと、接地時に呼ばれる処理</summary>
    /// <param name="playerScale"></param>
    void PlayerDirection(Vector3 playerScale)
    {
        var rota = transform.eulerAngles;
        if (playerScale.x > 0) { rota.z = -90f; }
        else { rota.z = 90f; }
        transform.eulerAngles = rota;
    }

    /// <summary>攻撃のクールタイムの処理</summary>
    /// <param name="count">攻撃回数</param>
    public IEnumerator AttackTime(PlayerController player,int count,Animator upBodyAnim)
    {
        Debug.Log($"{count}回目の攻撃！");
        upBodyAnim.SetTrigger("AttackTrigger");
        AttackSlash();
        player._audio.AttackPlayAudio();
        yield return WaitforSecondsCashe.Wait(_attackInterval);
        for (var time = 0f; time < _attackVaildInputTime; time += Time.deltaTime)
        {
            if (player.IsAttack && count < MaxAttackCount)
            {
                yield return StartCoroutine(AttackTime(player,count + 1,upBodyAnim));
                break;
            }
            yield return null;
        }
    }

    /// <summary>攻撃を出す処理</summary>
    void AttackSlash()
    {
        Instantiate(_attackScript.gameObject,_arrowObj.transform.position,transform.rotation);
        Instantiate(_slashingScript.gameObject, _arrowObj.transform.position, transform.rotation);
    }
}