using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eFoodType
{
    None = 0,
	Firefly,
	Bug,
	Worm,
	Insect,
    DayTime
}

public class FoodHandler : MonoBehaviour 
{
    bool mbSkipChecking;
    bool mbShouldDestroy = false;

    public eFoodType _eFoodType = eFoodType.None;

    void OnEnable()
    {
        CollectableAndFoodManager.Instance._listOfFoodHandlers.Add(this);
    }

    void Update()
    {
        if (mbShouldDestroy)
            Destroy(this.gameObject);
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
                //CEffectsPlayer.Instance.Play("FoodCollection");
				CollectableAndFoodManager.Instance.ApplyHealthBasedOnFood(_eFoodType);
            }
            mbShouldDestroy = true;
		}
	}

    void OnDisable()
    {
        CollectableAndFoodManager.Instance._listOfFoodHandlers.Remove(this);
    }
}
