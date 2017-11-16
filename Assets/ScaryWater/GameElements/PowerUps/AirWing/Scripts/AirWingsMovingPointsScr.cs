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

public class AirWingsMovingPointsScr : MonoBehaviour 
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
}
