using System.Collections;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    [SerializeField] Vector3 _point0;
    [SerializeField] Vector3 _point1;
    [SerializeField] float _lineRange = 1.0f;
    Vector3 _disPoint;
    [SerializeField] float _lineTime = 1.0f;
    [SerializeField] float _shotTime = 0.5f;
    [SerializeField] float _shotRange = 0.5f;
    float _currentTime = 0;
    float _persent;
    [SerializeField] bool _isVertical = false;
    bool _isTime = false;
    [SerializeField] LineRenderer _lineForward;
    [SerializeField] LineRenderer _lineBack;
    [SerializeField] LineRenderer _shotLine;
    [SerializeField] ParticleSystem _shotBulletParticle;
    [SerializeField] ParticleSystem _shotEffectParticle;
    private void Start()
    {
        //_lineForwardの初期設定
        _lineForward.SetPosition(0, _point0);
        if (_isVertical)
        {
            _lineForward.SetPosition(1, _point0);
            _lineForward.startWidth = _lineRange;
            _disPoint = _point1 - _point0;
        }
        else
        {
            _lineForward.SetPosition(1, _point1);
            _lineForward.startWidth = 0f;
        }
        Vector3[] vecs = { _point0, _point1 };
        // _lineBackの初期設定
        _lineBack.SetPositions(vecs);
        _lineBack.startWidth = _lineRange;

        // _shotLineの初期設定
        _shotLine.SetPositions(vecs);
        _shotLine.startWidth = 0;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        _persent = _currentTime / _lineTime;
        if (_lineTime > _currentTime)
        {
            if (_isVertical)
            {
                LineUpdateVertical(_point0 + _disPoint * _persent);
            }
            else
            {
                LineUpdateHorizontal(_lineRange * _persent);
            }
        }
        else if (!_isTime)
        {
            _isTime = true;
            StartCoroutine(ShotBullet());
        }
    }

    void LineUpdateHorizontal(float timePersent)
    {
        _lineForward.startWidth = timePersent;
    }

    void LineUpdateVertical(Vector3 timePersent)
    {
        _lineForward.SetPosition(1, timePersent);
    }

    IEnumerator ShotBullet()
    {
        // shotEffectParticle
        _shotEffectParticle.transform.position = _point1;
        _shotEffectParticle.transform.LookAt(_point0);
        _shotEffectParticle.Play();
        // shotBulletParticle
        _shotBulletParticle.transform.position = _point1;
        _shotBulletParticle.Play();
        //Line
        _lineBack.startWidth = 0f;
        _lineForward.startWidth = 0f;
        for(float currenttime = _shotTime;currenttime > 0f;currenttime -= Time.deltaTime)
        {
            _shotLine.startWidth = _shotRange * (currenttime / _shotTime);
            yield return new WaitForFixedUpdate();
        }
        _shotLine.startWidth = 0f;
    }
}
