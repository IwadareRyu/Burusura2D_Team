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
    [Tooltip("�U���Ԃ̃C���^�[�o��"),Header("�U���Ԃ̃C���^�[�o��")]
    [SerializeField] float _attackInterval = 0.2f;
    [Tooltip("�U���̗L�����͎���"),Header("�U���̗L�����͎���")]
    [SerializeField] float _attackVaildInputTime = 2f;
    [SerializeField] int MaxAttackCount = 3;
    float _x;
    float _y;

    public void Init(PlayerController playerController)
    {
        _controller = playerController;
        PlayerDirection(playerController.PlayerSprite.localScale);
        _playerCameraFraming = _playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    // Update is called once per frame
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        if (_x != 0 || _y != 0)
        {
            RotationArrow(_x, _y);
        }
        else
        {
            CameraMove(0);
        }
    }

    void RotationArrow(float x, float y)
    {
        var rota = transform.eulerAngles;
        rota.z = ArrowDirection(x, y);
        transform.eulerAngles = rota;
    }

    float ArrowDirection(float x, float y)
    {
        var rad = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        CameraMove(_x);
        if (_controller.IsGround && rad < 0)
        {
            var dir = _controller.PlayerSprite.localScale.x;
            CameraMove(dir / Mathf.Abs(dir));
            return _rotateGap * (dir / Mathf.Abs(dir));
            
        }
        else
        {
            CameraMove(x);
            return rad + _rotateGap;
        }
    }

    void CameraMove(float x)
    {
        _playerCameraFraming.m_ScreenX = 0.5f - x * 0.25f;
    }

    public void ResetDirection()
    {
        var difX = _arrowObj.transform.position.x - transform.position.x;
        var difY = _arrowObj.transform.position.y - transform.position.y;
        if (Mathf.Atan2(difX, difY) * Mathf.Rad2Deg < 0)
        {
            PlayerDirection(_controller.PlayerSprite.localScale);
        }
    }

    /// <summary>�����̌�����ݒ肷��Ƃ��ɌĂ΂�郁�\�b�h</summary>
    /// <param name="playerScale"></param>
    void PlayerDirection(Vector3 playerScale)
    {
        var rota = transform.eulerAngles;
        if (playerScale.x > 0) { rota.z = -90f; }
        else { rota.z = 90f; }
        transform.eulerAngles = rota;
    }

    // �U���̃N�[���^�C���̏���
    public IEnumerator AttackTime(int count)
    {
        Debug.Log($"{count}��ڂ̍U���I");
        AttackSlash();
        yield return new WaitForSeconds(_attackInterval);
        for (var time = 0f; time < _attackVaildInputTime; time += Time.deltaTime)
        {
            if (Input.GetButton("Fire1") && count < MaxAttackCount)
            {
                yield return StartCoroutine(AttackTime(count + 1));
                break;
            }
            yield return null;
        }
    }

    void AttackSlash()
    {
        Instantiate(_attackScript.gameObject,_arrowObj.transform.position,transform.rotation);
        Instantiate(_slashingScript.gameObject, _arrowObj.transform.position, transform.rotation);
    }
}