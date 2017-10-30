using UnityEngine;
using System;
using System.Collections;
using CAnimationFramework.CAction;

public class GUIStaticAnimation : MonoBehaviour {
	[Range(0.01f,5.0f)] public float _Rate = 1;
	public bool _BackToOriginalOnStop = false;
	public bool _FPSDependent = false;

	protected bool mbIsAnimating = false;
	protected float mTime = 0;

	//Override in child class
	public virtual void StartAnimation(){}
	public virtual void StopAnimating(){}
	protected virtual void DoAnimation(float dt){}

	void Update () 
	{
		if(!mbIsAnimating)
			return;
		
		if(_FPSDependent)
			DoAnimation(Time.deltaTime/_Rate);
		else
			DoAnimation(CSharedActionsHandler._AnimationDt/_Rate);
	}

}


//public class GUIStaticAnimation : MonoBehaviour {
//
//	public bool _AnimateScale = false;
//	public bool _AnimatePosition = false;
//	public bool _AnimateRotation = false;
//
//	public AnimationCurve _ScaleCurve = AnimationCurve.Linear(0,1,1,1);
//	public AnimationCurve _PositionCurve = AnimationCurve.Linear(0,1,1,1);
//	public AnimationCurve _RotationCurve = AnimationCurve.Linear(0,1,1,1);
//
//	public Vector3 _ScaleRange = new Vector3(0,0,0);
//	public Vector3 _PositionRange = new Vector3(0,0,0);
//	public Vector3 _RotationRange = new Vector3(0,0,0);
//
//	public bool _BackToOriginalOnStop = false;
//
//	Vector3 mOriginalScale;
//	Vector3 mOriginalPosition;
//	Vector3 mOriginalRotation;
//
//	float mTime = 0;
//	bool mbIsAnimating = false;
//	Transform mTransform;
//	public void StartAnimation()
//	{
//		mTransform = transform;
//		mbIsAnimating = true;
//
//		if(_AnimateScale)
//			mOriginalScale = transform.localScale;
//
//		if(_AnimatePosition)
//			mOriginalPosition = transform.localPosition;
//
//		if(_AnimateRotation)
//			mOriginalRotation = transform.localEulerAngles;
//	}
//
//	public void StopAnimating()
//	{
//		mbIsAnimating = false;
//
//		if(_BackToOriginalOnStop)
//		{
//			if(_AnimateScale)
//				transform.localScale = mOriginalScale;
//
//			if(_AnimatePosition)
//				transform.localPosition = mOriginalPosition;
//
//			if(_AnimateRotation)
//				transform.localEulerAngles = mOriginalRotation;
//		}
//	}
//
//	float mfScaleEval;
//	float mfPosEval;
//	float mfRotEval;
//	// Update is called once per frame
//	void Update () 
//	{
//		if(!mbIsAnimating)
//			return;
//		
//		mTime += Time.deltaTime;
//		if(mTime >= 1)
//			mTime -= 1;
//
//
//		if(_AnimateScale)
//		{
//			mfScaleEval = _ScaleCurve.Evaluate(mTime);
//			mTransform.localScale = mOriginalScale + _ScaleRange * mfScaleEval;
//			Debug.Log("Static animation: "+mTransform.localScale);
//
//		}
//
//		if(_AnimatePosition)
//		{
//			mfPosEval = _PositionCurve.Evaluate(mTime);
//			mTransform.localPosition = mOriginalPosition + _PositionRange * mfPosEval;
//		}
//
//		if(_AnimateRotation)
//		{
//			mfRotEval = _RotationCurve.Evaluate(mTime);
//			mTransform.localEulerAngles = mOriginalRotation + _RotationRange * mfRotEval;
//		}
//	}
//}
