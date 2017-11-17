using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eAirWingPathState
{
    None = 0,
    PointA,
    PointB,
    PointC,
    PointD,
    PointE1,
    PointE2,
    PointF1,
    PointF2
}

public class AirWingsPathScr : MonoBehaviour 
{
    public Transform _tPointA;
    public Transform _tPointB;
    public Transform _tPointC;
    public Transform _tPointD;
    public Transform _tPointE1;
    public Transform _tPointE2;
    public Transform _tPointF1;
    public Transform _tPointF2;
    public eAirWingPathState _eAirWingPathState;
    public AirWingsScr _AirWingsScr;

    void Start()
    {
        SetVisibilityOfMovingPoints();
    }

    void SetVisibilityOfMovingPoints(bool state = false)
    {
        _tPointA.GetComponent<MeshRenderer>().enabled = state;
        _tPointB.GetComponent<MeshRenderer>().enabled = state;
        _tPointC.GetComponent<MeshRenderer>().enabled = state;
        _tPointD.GetComponent<MeshRenderer>().enabled = state;
        _tPointE1.GetComponent<MeshRenderer>().enabled = state;
        _tPointE2.GetComponent<MeshRenderer>().enabled = state;
        _tPointF1.GetComponent<MeshRenderer>().enabled = state;
        _tPointF2.GetComponent<MeshRenderer>().enabled = state;
    }
}
