using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eObstacleTypes
{
    Mosquito,
    Flower,
    Branch,
}

public class Obstacle : MonoBehaviour {

    public float _DamageToPlayer = 10;
    public GameObject _ParticlePrefab;
    public bool _DidHitWithPlayer = false;
	
    void OnPlayerHit()
    {
        if (!_DidHitWithPlayer)
        {
            _DidHitWithPlayer = true;
            GameObject Particle = Instantiate(_ParticlePrefab, transform.position, Quaternion.identity) as GameObject;
            Destroy(this.gameObject);
        }
    }	
}
