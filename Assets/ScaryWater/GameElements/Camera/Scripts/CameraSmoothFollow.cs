using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraSmoothFollow : MonoBehaviour 
{
    float mfDistance = 10.0f;
    float mfHeight = 5.0f;
    float mfHeightDamping = 2.0f;
    float mfRotationDamping = 3.0f;
    Transform mTarget;

	[AddComponentMenu("Camera-Control/Smooth Follow")]

	void Start ()
	{
        Invoke("SetTarget", 0.5f);
	}

    void SetTarget()
    {
        mTarget = PlayerManager.Instance._playerHandler.GetComponent<Transform>();
    }

    void LateUpdate ()
	{
		if (!mTarget) 
            return; 

        float WantedRotationAngle = mTarget.eulerAngles.y;
        float WantedHeight = mTarget.position.y + mfHeight;
        float CurrentRotationAngle = transform.eulerAngles.y;
        float CurrentHeight = transform.position.y;
        CurrentRotationAngle = Mathf.LerpAngle(CurrentRotationAngle, WantedRotationAngle, mfRotationDamping * Time.deltaTime);
        CurrentHeight = Mathf.Lerp(CurrentHeight, WantedHeight, mfHeightDamping * Time.deltaTime);
        var CurrentRotation = Quaternion.Euler(0, CurrentRotationAngle, 0);
        transform.position = mTarget.position;
        transform.position -= CurrentRotation * Vector3.forward * mfDistance;
        transform.position = new Vector3(transform.position.x, CurrentHeight, transform.position.z);
        transform.LookAt(mTarget);
	}
}