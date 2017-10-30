using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightHandler : MonoBehaviour
{

                    [SerializeField]            Light           mlSun;
                    [SerializeField]            float           mfSecondsInFullDay              = 120f;
    [Range(0, 1)]   [SerializeField]            float           mfCurrentTimeOfTheDay           = 0f;
    [Range(0, 5)]   [SerializeField]            float           mfTimeMultiplier                = 0f;
    [Range(0, 5)]   [SerializeField]            float           mfSunLightIntensityMultiplier   = 0f;
                                                float           mfSunInitialIntensity           = 0f;
                                        static  DayNightHandler mInstance                       = null;

                                        public  bool            _bNightTime;

    public static DayNightHandler Instance
    {
        get { return mInstance; }
    }

    public float AutoImplementedProperties_CurrentTimeOfTheDay
    {
        get { return mfCurrentTimeOfTheDay; }

        set { mfCurrentTimeOfTheDay = value; }
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
        mfSunInitialIntensity = mlSun.intensity;
        mfCurrentTimeOfTheDay = 0.23f;
    }

    private void Update()
    {
        UpdateSun();

        mfCurrentTimeOfTheDay += (Time.deltaTime/ mfSecondsInFullDay) * mfTimeMultiplier;

        if(mfCurrentTimeOfTheDay >= 1)
        {
            mfCurrentTimeOfTheDay = 0;
        }
    }

    void UpdateSun()
    {
        mlSun.transform.localRotation   = Quaternion.Euler((mfCurrentTimeOfTheDay * 360) -90, 370, 0);
        float IntensityMultiplier       = mfSunLightIntensityMultiplier;

		if (mfCurrentTimeOfTheDay > 0.23f && mfCurrentTimeOfTheDay < 0.73f) // DayTime
		{
            _bNightTime = false;
			mfTimeMultiplier = 1;
            //EnemyAndObstacleManager.Instance.ManipulateElement(eEnemyType.Bat, eEnemyType.DayTime, false);
            CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.Firefly, eFoodType.DayTime, false);
		}

        if(mfCurrentTimeOfTheDay < 0.23f || mfCurrentTimeOfTheDay > 0.73f) // NightTime
        {
			_bNightTime = true;
            IntensityMultiplier = 0;
            mfTimeMultiplier = 2.3f;
			//EnemyAndObstacleManager.Instance.ManipulateElement(eEnemyType.DayTime, eEnemyType.Bat, true);
			CollectableAndFoodManager.Instance.ManipulateElement(eFoodType.DayTime, eFoodType.Firefly, true);
        }

        else if(mfCurrentTimeOfTheDay < 0.23f)
        {
            IntensityMultiplier = Mathf.Clamp01((mfCurrentTimeOfTheDay - 0.23f) * (1 / 0.02f));
        }

        else if(mfCurrentTimeOfTheDay > 0.73f)
        {
            IntensityMultiplier = Mathf.Clamp01(1 - (mfCurrentTimeOfTheDay - 0.73f) * (1 / 0.02f));
        }

        mlSun.intensity = mfSunInitialIntensity * IntensityMultiplier;
    }
}
