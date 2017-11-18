using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlatformHandlerType
{
	Fixed = 0,
	Loose,
	NoSupport,
}


public class PlatformHandler : MonoBehaviour 
{
    public ePlatformHandlerType _eSupportType = ePlatformHandlerType.Fixed;
    public Transform _Transform;

    public Transform GetTransform
    {
        get
        {
            if (_Transform == null)
                _Transform = transform;
            
            return _Transform;
        }
    }

    void Awake()
    {
        _Transform = transform;
    }

    void OnEnable()
    {
        if (_eSupportType == ePlatformHandlerType.Loose)
            transform.GetComponent<MeshRenderer>().enabled = false;

        EnvironmentManager.Instance._listOfPlatformHandler.Add(this);
    }

    void Update()
    {
		if (PlayerManager.Instance._playerHandler.transform.position.z - this.transform.position.z >= 20f)
		{
            if (_eSupportType == ePlatformHandlerType.Loose)
            {
				_eSupportType = ePlatformHandlerType.Fixed;
				transform.GetComponent<MeshRenderer>().enabled = true; 
            }
		}  
    }

    public void PlayRippleParticle(ParticleSystem particle)
    {
        Vector3 tPlatformPos;
        tPlatformPos.x = transform.position.x;
        tPlatformPos.y = transform.position.y - 0.2f;
        tPlatformPos.z = transform.position.z;
        particle.Stop();
        particle.transform.position = tPlatformPos;
        particle.Play();
    }

    void OnDisable()
    {
        EnvironmentManager.Instance._listOfPlatformHandler.Remove(this);
    }
}
