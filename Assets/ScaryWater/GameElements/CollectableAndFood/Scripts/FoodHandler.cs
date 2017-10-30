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

    public eFoodType _eFoodType = eFoodType.None;

    void OnEnable()
    {
        CollectableAndFoodManager.Instance._listOfFoodHandlers.Add(this);
    }

    void OnDestroy()
    {
        CollectableAndFoodManager.Instance._listOfFoodHandlers.Remove(this);
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
            if (!mbSkipChecking)
            {
                mbSkipChecking = true;
				CollectableAndFoodManager.Instance.ApplyHealthBasedOnFood(_eFoodType);
            }
            Destroy(this.gameObject);
		}
	}
}
