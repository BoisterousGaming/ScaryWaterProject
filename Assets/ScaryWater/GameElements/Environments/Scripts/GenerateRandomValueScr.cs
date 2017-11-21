using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomValueScr
{
    int miFPreviousValue;
    int miSPreviousValue;
    int miCurrentValue;
    int miCount;
    int miLimit = 5;

    public int Random(int min, int max)
    {
        miCurrentValue = RollTheDice(min, max);
        do
        {
            if (miFPreviousValue == miCurrentValue | miSPreviousValue == miCurrentValue)
                miCurrentValue = RollTheDice(min, max);
            else
            {
                miFPreviousValue = miSPreviousValue;
                miSPreviousValue = miCurrentValue;
                return miCurrentValue;
            }
            miCount++;
        }
        while (miCount < miLimit);
        return RollTheDice(min, max);
    }

    int RollTheDice(int min, int max)
    {
        int tMax = max + 1;
        return UnityEngine.Random.Range(min, tMax);
    }
}




//public class GenerateRandomValueScr
//{
//    int miCurrentValue;
//    int miLimit = 5;

//    Queue<int> _AlreadyUsed = new Queue<int>();
//    int _QueueSize = 3;
//    float _Threshold = 0.5f;


//    public EnvironmentManager _EnvironmentManager;

//    public int Random(int min, int max)
//    {
//        float tPossibilityForUse = 0;
//        do
//        {
//            miCurrentValue = UnityEngine.Random.Range(min, max + 1);
//            tPossibilityForUse = CheckPossiblilityForUse(miCurrentValue);

//            if (_AlreadyUsed.Count > miLimit && tPossibilityForUse < _Threshold)
//            {
//                _AlreadyUsed.Dequeue(0);
//                _AlreadyUsed.Insert(_AlreadyUsed.Count, miCurrentValue);
//            }
//            else
//            {
                
//            }
//        }
//        while (tPossibilityForUse < _Threshold);
//        Debug.Log("Index: " + miCurrentValue);
//        return miCurrentValue;
//    }

//    //Check the possiblility of use for a particular value
//    float CheckPossiblilityForUse(int iValue)
//    {
//        float possiblilityForUse = 1;
//        for (int i = 0; i < _AlreadyUsed.Count; i++)
//        {
//            if (iValue == _AlreadyUsed[i])
//                possiblilityForUse = i / _AlreadyUsed.Count;
//        }
//        return possiblilityForUse;
//    }
//}
