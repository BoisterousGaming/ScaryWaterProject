using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightHandler : MonoBehaviour
{
    float mfSunInitialIntensity = 0f;
    static DayNightHandler mInstance = null;

    public delegate void DayNightChanged(bool state);
    public DayNightChanged _DayNightChangedCallback;
    public Light _sunLight;
    public float _fSecondsInFullDay = 250f;
    [Range(0, 1)] public float _fCurrentTimeOfTheDay = 0f;
    [Range(0, 5)] public float _fTimeMultiplier = 1f;
    [Range(0, 5)] public float _fSunLightIntensityMultiplier = 1f;
    public bool _bNightTime;
    public bool _bEnableDayNightHandler = false;

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
        _fCurrentTimeOfTheDay = 0.65f;
    }

    private void Update()
    {
        if (!_bEnableDayNightHandler)
            return;
        
        UpdateSun();

        _fCurrentTimeOfTheDay += (Time.deltaTime/ _fSecondsInFullDay) * _fTimeMultiplier;

        if(_fCurrentTimeOfTheDay >= 1)
            _fCurrentTimeOfTheDay = 0;

        //Debug.Log("CurrentTimeOfTheDay: " + _fCurrentTimeOfTheDay);
    }

    void UpdateSun()
    {
        _sunLight.transform.localRotation = Quaternion.Euler(360 - (_fCurrentTimeOfTheDay * 360), 0, 0);
        float IntensityMultiplier = _fSunLightIntensityMultiplier;

		if (_fCurrentTimeOfTheDay > 0.23f & _fCurrentTimeOfTheDay < 0.73f) // DayTime
		{
			_fTimeMultiplier = 1;
            CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.Firefly, eFoodType.DayTime, false);
		}

        if(_fCurrentTimeOfTheDay < 0.23f | _fCurrentTimeOfTheDay > 0.73f) // NightTime
        {
            IntensityMultiplier = 0;
            _fTimeMultiplier = 2.3f;
			CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.DayTime, eFoodType.Firefly, true);
        }

        else if(_fCurrentTimeOfTheDay < 0.23f)
            IntensityMultiplier = Mathf.Clamp01((_fCurrentTimeOfTheDay - 0.23f) * (1 / 0.02f));

        else if(_fCurrentTimeOfTheDay > 0.73f)
            IntensityMultiplier = Mathf.Clamp01(1 - (_fCurrentTimeOfTheDay - 0.73f) * (1 / 0.02f));

        if (_fCurrentTimeOfTheDay < 0f & _fCurrentTimeOfTheDay > -1f)
            _bNightTime = true;
        else
            _bNightTime = false;
        
        _sunLight.intensity = mfSunInitialIntensity * IntensityMultiplier;
    }
}
