using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftSample : MonoBehaviour
{
    [SerializeField] Transform[] _liftMovePos;
    private void OnDrawGizmos()
    {
        if (_liftMovePos != null)
        {
            Gizmos.color = Color.red;
            for (var i = 0; i <= _liftMovePos.Length - 2; i++) 
            {
                Gizmos.DrawLine(_liftMovePos[i].position, _liftMovePos[i + 1].position);
            }
        }
    }
}
