using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BZCVisual
{
    public Color _PathColor = Color.green;
    public Color _InactivePathColor = Color.gray;
    public Color _FrustrumColor = Color.white;
    public Color _HandleColor = Color.yellow;
}

public enum eBZCECurveType
{
    EaseInAndOut,
    Linear,
    Custom
}

public enum eBZCEAfterLoop
{
    Continue,
    Stop
}

[System.Serializable]
public class BZCPoint
{
    public Vector3 _vPosition;
    public Quaternion _qRotation;
    public Vector3 _vHandlePrev;
    public Vector3 _vHandleNext;
    public eBZCECurveType _eCurveTypeRotation;
    public AnimationCurve _RotationCurve;
    public eBZCECurveType _eCurveTypePosition;
    public AnimationCurve _PositionCurve;
    public bool _bChained;

    public BZCPoint(Vector3 vPos, Quaternion qRot)
    {
        _vPosition = vPos;
        _qRotation = qRot;
        _vHandlePrev = Vector3.back;
        _vHandleNext = Vector3.forward;
        _eCurveTypeRotation = eBZCECurveType.EaseInAndOut;
        _RotationCurve = AnimationCurve.EaseInOut(0,0,1,1);
        _eCurveTypePosition = eBZCECurveType.Linear;
        _PositionCurve = AnimationCurve.Linear(0,0,1,1);
        _bChained = true;
    }
}

public class BZCObjectPath : MonoBehaviour
{
    int miCurrentWaypointIndex;
    float mfCurrentTimeInWaypoint;
    float mfTimePerSegment;
    bool mbPaused = false;
    bool mbPlaying = false;
    bool mbReachedDropPoint = false;

    public GameObject _SelectedObject;
    public Transform _tLookAtTarget;
    public BZCVisual BZCVisualScr;
    public eBZCEAfterLoop _eAfterLoop = eBZCEAfterLoop.Continue;
    public List<BZCPoint> _ListOfPoints = new List<BZCPoint>();
    public float _fPlayOnAwakeTime = 10;
    public int _iPickupPointIndex = 0;
    public int _iDropPointIndex = 0;
    public bool _bUseMainCamera = true;
    public bool _bLookAtTarget = false;
    public bool _bPlayOnAwake = false;
    public bool _bLooped = false;
    public bool _bAlwaysShow = true;
    public bool _bPickupPoint = false;
    public bool _bDropPoint = false;
    public delegate void ArrivedAtThePickupPoint(Vector3 vPosition);
    public ArrivedAtThePickupPoint _ArrivedAtThePickupPointCallback;
    public delegate void ArrivedAtTheDropPoint(Vector3 vPosition);
    public ArrivedAtTheDropPoint _ArrivedAtTheDropPointCallback;
    public delegate void ArrivedAtTheEndPoint(Vector3 vPosition);
    public ArrivedAtTheEndPoint _ArrivedAtTheEndPointCallback;

    void Start ()
    {
	    if (_bUseMainCamera)
	        _SelectedObject = Camera.main.gameObject;

	    if (_bLookAtTarget && _tLookAtTarget == null)
	    {
	        _bLookAtTarget = false;
            Debug.LogError("No target selected to look at, defaulting to normal rotation");
        }

	    foreach (var index in _ListOfPoints)
	    {
            if (index._eCurveTypeRotation == eBZCECurveType.EaseInAndOut) index._RotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index._eCurveTypeRotation == eBZCECurveType.Linear) index._RotationCurve = AnimationCurve.Linear(0, 0, 1, 1);
            if (index._eCurveTypePosition == eBZCECurveType.EaseInAndOut) index._PositionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
            if (index._eCurveTypePosition == eBZCECurveType.Linear) index._PositionCurve = AnimationCurve.Linear(0, 0, 1, 1);
        }

        if (_bPlayOnAwake)
            PlayPath(_fPlayOnAwakeTime);
        
        PausePath();
        _iDropPointIndex = Mathf.Clamp(_iDropPointIndex, 0, _ListOfPoints.Count - 1);
        _iPickupPointIndex = Mathf.Clamp(_iPickupPointIndex, 0, _iDropPointIndex - 1);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //    ResumePath();
        //if (Input.GetKeyDown(KeyCode.S))
            //PlayPath(_fPlayOnAwakeTime);
    }

    /// <summary>
    /// Plays the path
    /// </summary>
    /// <param name="time">The time in seconds how long the object takes for the entire path</param>
    public void PlayPath(float time = 0f)
    {
        if (time <= 0.1f)
            time = _fPlayOnAwakeTime;
        
        if (time <= 0) time = 0.001f;
        mbPaused = false;
        mbPlaying = true;
        StopAllCoroutines();
        StartCoroutine(FollowPath(time));
    }

    /// <summary>
    /// Stops the path
    /// </summary>
    public void StopPath()
    {
        mbPlaying = false;
        mbPaused = false;
        StopAllCoroutines();
        Vector3 tPos = _ListOfPoints[miCurrentWaypointIndex]._vPosition;
        OnArriveEndPoint(tPos);
    }

    /// <summary>
    /// Stop the object
    /// </summary>
    public void OnArriveEndPoint(Vector3 vPosition)
    {
        Debug.Log("Reached end point");
        if (_ArrivedAtTheEndPointCallback != null)
            _ArrivedAtTheEndPointCallback(vPosition);
        //Destroy(_SelectedObject);
        //Destroy(this.gameObject);
    }

    /// <summary>
    /// Release attached object
    /// </summary>
    public void OnArriveDropPoint(Vector3 vPosition)
    {
        Debug.Log("Reached drop point");
        if (_ArrivedAtTheDropPointCallback != null)
            _ArrivedAtTheDropPointCallback(vPosition);
    }

    /// <summary>
    /// Allows to change the time variable specified in PlayPath(float time) on the fly
    /// </summary>
    /// <param name="seconds">New time in seconds for entire path</param>
    public void UpdateTimeInSeconds(float seconds)
    {
        mfTimePerSegment = seconds / ((_bLooped) ? _ListOfPoints.Count : _ListOfPoints.Count - 1);
    }

    /// <summary>
    /// Pauses the object's movement - resumable with ResumePath()
    /// </summary>
    public void PausePath()
    {
        mbPaused = true;
        mbPlaying = false;
    }

    /// <summary>
    /// Can be called after PausePath() to resume
    /// </summary>
    public void ResumePath()
    {
        _bPickupPoint = false;
        if (mbPaused)
            mbPlaying = true;
        mbPaused = false;
    }

    /// <summary>
    /// Gets if the path is paused
    /// </summary>
    /// <returns>Returns paused state</returns>
    public bool IsPaused()
    {
        return mbPaused;
    }

    /// <summary>
    /// Gets if the path is playing
    /// </summary>
    /// <returns>Returns playing state</returns>
    public bool IsPlaying()
    {
        return mbPlaying;
    }

    /// <summary>
    /// Gets the index of the current waypoint
    /// </summary>
    /// <returns>Returns waypoint index</returns>
    public int GetCurrentWayPoint()
    {
        //if (_bPickupPoint)
        //{
        //    if (_iPickupPointIndex == miCurrentWaypointIndex)
        //        PausePath();
        //}

        //if (_bDropPoint)
        //{
        //    Debug.Log("DropPoint Is Set");
        //    if (_iDropPointIndex == miCurrentWaypointIndex)
        //    {
        //        Debug.Log("DropPoint Is Reached");
        //        if (!mbReachedDropPoint)
        //        {
        //            mbReachedDropPoint = true;
        //            Vector3 tPos = _ListOfPoints[_iDropPointIndex]._vPosition;
        //            OnArriveDropPoint(tPos);
        //            Debug.Log("ArriveState Are Set");
        //        }
        //    }
        //}
        Debug.Log("Current Point Is: " + miCurrentWaypointIndex);
        return miCurrentWaypointIndex;
    }

    /// <summary>
    /// Gets the time within the current waypoint (Range is 0-1)
    /// </summary>
    /// <returns>Returns time of current waypoint (Range is 0-1)</returns>
    public float GetCurrentTimeInWaypoint()
    {
        return mfCurrentTimeInWaypoint;
    }

    /// <summary>
    /// Sets the current waypoint index of the path
    /// </summary>
    /// <param name="value">Waypoint index</param>
    public void SetCurrentWayPoint(int value)
    {
        miCurrentWaypointIndex = value;
    }

    /// <summary>
    /// Sets the time in the current waypoint 
    /// </summary>
    /// <param name="value">Waypoint time (Range is 0-1)</param>
    public void SetCurrentTimeInWaypoint(float value)
    {
        mfCurrentTimeInWaypoint = value;
    }

    /// <summary>
    /// When index/time are set while the path is not playing, this method will teleport the object to the position/rotation specified
    /// </summary>
    public void RefreshTransform()
    {
        _SelectedObject.transform.position = GetBezierPosition(miCurrentWaypointIndex, mfCurrentTimeInWaypoint);
        if (!_bLookAtTarget)
            _SelectedObject.transform.rotation = GetLerpRotation(miCurrentWaypointIndex, mfCurrentTimeInWaypoint);
        else
            _SelectedObject.transform.rotation = Quaternion.LookRotation((_tLookAtTarget.transform.position - _SelectedObject.transform.position).normalized);
    }

    IEnumerator FollowPath(float time)
    {
        UpdateTimeInSeconds(time);
        miCurrentWaypointIndex = 0;
        while (miCurrentWaypointIndex < _ListOfPoints.Count)
        {
            mfCurrentTimeInWaypoint = 0;
            while (mfCurrentTimeInWaypoint < 1)
            {
                if (!mbPaused)
                {
                    mfCurrentTimeInWaypoint += Time.deltaTime / mfTimePerSegment;
                    _SelectedObject.transform.position = GetBezierPosition(miCurrentWaypointIndex, mfCurrentTimeInWaypoint);
                    if (!_bLookAtTarget)
                        _SelectedObject.transform.rotation = GetLerpRotation(miCurrentWaypointIndex, mfCurrentTimeInWaypoint);
                    else
                        _SelectedObject.transform.rotation = Quaternion.LookRotation((_tLookAtTarget.transform.position - _SelectedObject.transform.position).normalized);
                }
                yield return 0;
            }
            ++miCurrentWaypointIndex;
            if (miCurrentWaypointIndex == _ListOfPoints.Count - 1 && !_bLooped) break;
            if (miCurrentWaypointIndex == _ListOfPoints.Count && _eAfterLoop == eBZCEAfterLoop.Continue) miCurrentWaypointIndex = 0;

            if (_bPickupPoint)
            {
                if (_iPickupPointIndex == miCurrentWaypointIndex)
                    PausePath();
            }

            if (_bDropPoint)
            {
                //Debug.Log("DropPoint Is Set");
                if (_iDropPointIndex == miCurrentWaypointIndex)
                {
                    //Debug.Log("DropPoint Is Reached");
                    if (!mbReachedDropPoint)
                    {
                        mbReachedDropPoint = true;
                        Vector3 tPos = _ListOfPoints[_iDropPointIndex]._vPosition;
                        OnArriveDropPoint(tPos);
                        //Debug.Log("ArriveState Are Set");
                    }
                }
            }
        }
        StopPath();
    }

    int GetNextIndex(int index)
    {
        if (index == _ListOfPoints.Count-1)
            return 0;
        return index + 1;
    }

    Vector3 GetBezierPosition(int pointIndex, float time)
    {
        float t = _ListOfPoints[pointIndex]._PositionCurve.Evaluate(time);
        int nextIndex = GetNextIndex(pointIndex);
        return
            Vector3.Lerp(
                Vector3.Lerp(
                    Vector3.Lerp(_ListOfPoints[pointIndex]._vPosition,
                        _ListOfPoints[pointIndex]._vPosition + _ListOfPoints[pointIndex]._vHandleNext, t),
                    Vector3.Lerp(_ListOfPoints[pointIndex]._vPosition + _ListOfPoints[pointIndex]._vHandleNext,
                        _ListOfPoints[nextIndex]._vPosition + _ListOfPoints[nextIndex]._vHandlePrev, t), t),
                Vector3.Lerp(
                    Vector3.Lerp(_ListOfPoints[pointIndex]._vPosition + _ListOfPoints[pointIndex]._vHandleNext,
                        _ListOfPoints[nextIndex]._vPosition + _ListOfPoints[nextIndex]._vHandlePrev, t),
                    Vector3.Lerp(_ListOfPoints[nextIndex]._vPosition + _ListOfPoints[nextIndex]._vHandlePrev,
                        _ListOfPoints[nextIndex]._vPosition, t), t), t);
    }

    private Quaternion GetLerpRotation(int pointIndex, float time)
    {
        return Quaternion.LerpUnclamped(_ListOfPoints[pointIndex]._qRotation, _ListOfPoints[GetNextIndex(pointIndex)]._qRotation, _ListOfPoints[pointIndex]._RotationCurve.Evaluate(time));
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject == gameObject || _bAlwaysShow)
        {
            if (_ListOfPoints.Count >= 2)
            {
                for (int i = 0; i < _ListOfPoints.Count; i++)
                {
                    if (i < _ListOfPoints.Count - 1)
                    {
                        var index = _ListOfPoints[i];
                        var indexNext = _ListOfPoints[i + 1];
                        UnityEditor.Handles.DrawBezier(index._vPosition, indexNext._vPosition, index._vPosition + index._vHandleNext,
                            indexNext._vPosition + indexNext._vHandlePrev,((UnityEditor.Selection.activeGameObject == gameObject) ? BZCVisualScr._PathColor : BZCVisualScr._InactivePathColor), null, 5);
                    }
                    else if (_bLooped)
                    {
                        var index = _ListOfPoints[i];
                        var indexNext = _ListOfPoints[0];
                        UnityEditor.Handles.DrawBezier(index._vPosition, indexNext._vPosition, index._vPosition + index._vHandleNext,
                            indexNext._vPosition + indexNext._vHandlePrev, ((UnityEditor.Selection.activeGameObject == gameObject) ? BZCVisualScr._PathColor : BZCVisualScr._InactivePathColor), null, 5);
                    }
                }
            }

            for (int i = 0; i < _ListOfPoints.Count; i++)
            {
                var index = _ListOfPoints[i];
                Gizmos.matrix = Matrix4x4.TRS(index._vPosition, index._qRotation, Vector3.one);
                Gizmos.color = BZCVisualScr._FrustrumColor;
                Gizmos.DrawFrustum(Vector3.zero, 90f, 0.25f, 0.01f, 1.78f);
                Gizmos.matrix = Matrix4x4.identity;
            }
        }
    }
#endif
}
