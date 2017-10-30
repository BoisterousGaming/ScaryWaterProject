using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRandomValueScr
{
    int miPreviousValue;
    int miCurrentValue;
    int miCount;
    int miLimit = 5;

    public EnvironmentManager _EnvironmentManager;

    public int Random(int min, int max)
    {
        miCurrentValue = RollTheDice(min, max);

        do
        {
            if (miPreviousValue == miCurrentValue)
                miCurrentValue = RollTheDice(min, max);

            else
            {
                miPreviousValue = miCurrentValue;
                //Debug.Log("CurrentValue: " + miCurrentValue);
                return miPreviousValue;
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
