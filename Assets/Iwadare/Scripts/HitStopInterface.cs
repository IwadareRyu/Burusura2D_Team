using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HitStopInterface
{
    abstract void HitStopStart(float _hitStopPower);

    abstract void HitStopEnd();
}
