using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SnakeScr : MonoBehaviour 
{
    Vector3 mvTargetPosition = Vector3.zero;
    Transform _PlayerTransform;

    public Transform[] _arrOfSnakes;
    public EnemyAndObstacleManager _EnemyAndObstacleManagerScr;
    public bool _bFollowThePlayer = false;
    public float _fSnakeFollowingSpeed = 10f;

    void Start()
    {
        Invoke("AfterStart", 1f);
    }

    void AfterStart()
    {
        _PlayerTransform = _EnemyAndObstacleManagerScr._playerManager._playerHandler._tPlayerTransform;
    }

    public void StopDOTweenSequence()
    {
        for (int i = 0; i < _arrOfSnakes.Length; i++)
        {
            _arrOfSnakes[i].DOKill();
        }
        _bFollowThePlayer = true;
    }

    void FixedUpdate()
    {
        if (_bFollowThePlayer)
            FollowThePlayer();
    }

    void FollowThePlayer()
    {
        mvTargetPosition.x = 2.0f;
        mvTargetPosition.y = -2.5f;
        mvTargetPosition.z = _PlayerTransform.position.z - 1f;
        Vector3 tSnake1Pos = _arrOfSnakes[0].transform.position;
        _arrOfSnakes[0].transform.position = Vector3.MoveTowards(tSnake1Pos, mvTargetPosition, Time.fixedDeltaTime * _fSnakeFollowingSpeed);

        mvTargetPosition.x = -2.0f;
        mvTargetPosition.y = -2.5f;
        mvTargetPosition.z = _PlayerTransform.position.z - 1f;
        Vector3 tSnake2Pos = _arrOfSnakes[1].transform.position;
        _arrOfSnakes[1].transform.position = Vector3.MoveTowards(tSnake2Pos, mvTargetPosition, Time.fixedDeltaTime * _fSnakeFollowingSpeed);
    }
}
