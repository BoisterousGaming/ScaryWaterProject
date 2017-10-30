using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdInitiate : MonoBehaviour 
{
	public delegate void InitiateBird();
	public InitiateBird _InitiateBird;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_InitiateBird != null)
                _InitiateBird();
            
            Destroy(this.gameObject, 0.2f);
        }
    }
}
