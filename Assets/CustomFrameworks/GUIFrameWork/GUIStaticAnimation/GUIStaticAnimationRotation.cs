using UnityEngine;
using System.Collections;

public class GUIStaticAnimationRotation : GUIStaticAnimation {

	public Vector3 _Range = new Vector3(2,2,2);
	public AnimationCurve _Curve = AnimationCurve.Linear(0,1,1,1);
	Vector3 mOriginal;

	Transform mTransform;

	public override void StartAnimation(){
		mTransform = transform;
		mbIsAnimating = true;
		mOriginal = transform.localEulerAngles;
	}

	public override void StopAnimating(){
		mbIsAnimating = false;

		if(_BackToOriginalOnStop)
			transform.localEulerAngles = mOriginal;
	}

	float mfEval;
	protected override void DoAnimation(float dt){

		mTime += dt;
		if(mTime >= 1)
			mTime -= 1;
		
		mfEval = _Curve.Evaluate(mTime);
		mTransform.localEulerAngles = mOriginal + _Range * mfEval;
	}
}
