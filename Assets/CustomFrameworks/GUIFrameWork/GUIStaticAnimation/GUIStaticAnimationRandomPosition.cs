using UnityEngine;
using System.Collections;

public class GUIStaticAnimationRandomPosition : GUIStaticAnimation {

	public Vector2 _RangeX = new Vector2(-2,2);
	public Vector2 _RangeY = new Vector2(-2,2);
	public Vector2 _RangeZ = new Vector2(-2,2);
	public float _Speed = 1;

	Vector3 mTargetPosition = new Vector3(0,0,0);
	Vector3 mOriginal;
	RectTransform mTransform;
	float mfRate;

	public override void StartAnimation(){
		mTransform = GetComponent<RectTransform>();
		mOriginal = mTransform.anchoredPosition;
		NewTargetPosition();
		mbIsAnimating = true;
	}

	public override void StopAnimating()
	{
		mbIsAnimating = false;

		if(_BackToOriginalOnStop)
			mTransform.anchoredPosition = mOriginal;
	}

	float mfCompletion;
	float mfTimeRequired;
	Vector3 mPositionDiff = new Vector3(0,0,0);
	protected override void DoAnimation(float dt){

		if (mfCompletion >= 1) {
			mTime = 0;
			NewTargetPosition ();
		} 

		mTime += dt;
		mfCompletion = mTime / mfTimeRequired;
		if(mfCompletion > 1)
			mfCompletion = 1;

		mTransform.anchoredPosition = (mNewOriginal + mPositionDiff*mfCompletion);
	}

	Vector3 mNewOriginal;
	void NewTargetPosition()
	{
		mTime = 0;
		mfCompletion = 0;

		mNewOriginal =  mTransform.anchoredPosition;
		mTargetPosition = new Vector3(0,0,0);
		mTargetPosition.x = Random.Range(_RangeX.x,_RangeX.y);
		mTargetPosition.y = Random.Range(_RangeY.x,_RangeY.y);
		mTargetPosition.z = Random.Range(_RangeZ.x,_RangeZ.y);
		mTargetPosition += mOriginal;

		mPositionDiff = mTargetPosition - mNewOriginal;
		mfTimeRequired = Vector3.Magnitude(mPositionDiff)/_Speed;

		if (mfTimeRequired == 0)
			NewTargetPosition ();
	}
}
