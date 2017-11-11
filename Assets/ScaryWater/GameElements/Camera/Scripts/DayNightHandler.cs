﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightHandler : MonoBehaviour
{
    float mfSunInitialIntensity = 0f;
    static DayNightHandler mInstance = null;

    public Light _sunLight;
    public float _fSecondsInFullDay = 250f;
    [Range(0, 1)] public float _fCurrentTimeOfTheDay = 0f;
    [Range(0, 5)] public float _fTimeMultiplier = 1f;
    [Range(0, 5)] public float _fSunLightIntensityMultiplier = 1f;
    public bool _bNightTime;

    public static DayNightHandler Instance
    {
        get { return mInstance; }
    }

    private void Awake()
    {
		if (mInstance == null)
			mInstance = this;

		else if (mInstance != this)
			Destroy(this.gameObject);
    }

    private void Start()
    {
        mfSunInitialIntensity = _sunLight.intensity;
        _fCurrentTimeOfTheDay = 0.42f;
    }

    private void Update()
    {
        UpdateSun();

        _fCurrentTimeOfTheDay += (Time.deltaTime/ _fSecondsInFullDay) * _fTimeMultiplier;

        if(_fCurrentTimeOfTheDay >= 1)
        {
            _fCurrentTimeOfTheDay = 0;
        }
    }

    void UpdateSun()
    {
        _sunLight.transform.localRotation = Quaternion.Euler(360 - (_fCurrentTimeOfTheDay * 360), 0, 0);
        float IntensityMultiplier = _fSunLightIntensityMultiplier;

		if (_fCurrentTimeOfTheDay > 0.23f && _fCurrentTimeOfTheDay < 0.73f) // DayTime
		{
            _bNightTime = false;
			_fTimeMultiplier = 1;
            //EnemyAndObstacleManager.Instance.ManipulateElement(eEnemyType.Bat, eEnemyType.DayTime, false);
            CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.Firefly, eFoodType.DayTime, false);
		}

        if(_fCurrentTimeOfTheDay < 0.23f || _fCurrentTimeOfTheDay > 0.73f) // NightTime
        {
			_bNightTime = true;
            IntensityMultiplier = 0;
            _fTimeMultiplier = 2.3f;
			//EnemyAndObstacleManager.Instance.ManipulateElement(eEnemyType.DayTime, eEnemyType.Bat, true);
			CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.DayTime, eFoodType.Firefly, true);
        }

        else if(_fCurrentTimeOfTheDay < 0.23f)
        {
            IntensityMultiplier = Mathf.Clamp01((_fCurrentTimeOfTheDay - 0.23f) * (1 / 0.02f));
        }

        else if(_fCurrentTimeOfTheDay > 0.73f)
        {
            IntensityMultiplier = Mathf.Clamp01(1 - (_fCurrentTimeOfTheDay - 0.73f) * (1 / 0.02f));
        }

        _sunLight.intensity = mfSunInitialIntensity * IntensityMultiplier;
    }
}
