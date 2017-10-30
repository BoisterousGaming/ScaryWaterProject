using UnityEngine;
using System.Collections;

public class GUIStaticAnimationPosition : GUIStaticAnimation {

	public Vector3 _Range = new Vector3(2,2,2);
	public AnimationCurve _Curve = AnimationCurve.Linear(0,1,1,1);
	Vector3 mOriginal;

	RectTransform mTransform;

	public override void StartAnimation(){
		mTransform = GetComponent<RectTransform>();
		mbIsAnimating = true;
		mOriginal = mTransform.anchoredPosition;
	}

	public override void StopAnimating(){
		mbIsAnimating = false;

		if(_BackToOriginalOnStop)
			mTransform.anchoredPosition = mOriginal;
	}

	float mfEval;
	protected override void DoAnimation(float dt){

		mTime += dt;
		if(mTime >= 1)
			mTime -= 1;

		mfEval = _Curve.Evaluate(mTime);
		mTransform.anchoredPosition = mOriginal + _Range * mfEval;;
	}
}
