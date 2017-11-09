using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonHandler : MonoBehaviour
{
    Transform mtTransform;

    public PlayerHandler _playerHandler = null;
    public float[] _arrPoisonThrowingSpeed;
    public float[] _arrPoisonLifeDuration;

    public void Initialize()
    {
        mtTransform = transform;
        mtTransform.GetComponent<Rigidbody>().useGravity = true;
		mtTransform.position = _playerHandler._tPlayerTransform.position + _playerHandler._tPlayerTransform.forward * 2f;

        if (DataManager.GetPoisonRange() != -1)
        {
            int tIndex = DataManager.GetPoisonRange();
			mtTransform.GetComponent<Rigidbody>().AddForce(mtTransform.forward * _arrPoisonThrowingSpeed[tIndex]);
			Destroy(this.gameObject, _arrPoisonLifeDuration[tIndex]);
        }

        else
        {
			mtTransform.GetComponent<Rigidbody>().AddForce(mtTransform.forward * 150f);
			Destroy(this.gameObject, 1f);
        }
        //Debug.Log("PoisonCount: " + DataManager.GetPoisonAmount());
    }
}
