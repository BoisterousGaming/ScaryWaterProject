using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdInitiate : MonoBehaviour 
{
    bool mbShouldDestroy = false;

	public delegate void InitiateBird();
	public InitiateBird _InitiateBird;

    void Update()
    {
        if (mbShouldDestroy)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_InitiateBird != null)
                _InitiateBird();

            mbShouldDestroy = true;
        }
    }
}
