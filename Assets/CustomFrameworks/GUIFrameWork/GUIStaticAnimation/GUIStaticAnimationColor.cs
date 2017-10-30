using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIStaticAnimationColor : GUIStaticAnimation {

	public Vector4 _Range = new Vector4(1,1,1,1);
	public AnimationCurve _Curve = AnimationCurve.Linear(0,1,1,1);
	Color mOriginal;

	Image mImage;

	public override void StartAnimation(){
		mImage = GetComponent<Image>();
		mbIsAnimating = true;
		mOriginal = mImage.color;
	}

	public override void StopAnimating(){
		mbIsAnimating = false;

		if(_BackToOriginalOnStop)
			mImage.color = mOriginal;
	}

	float mfEval;
	Color mTempColor = new Color (1, 1, 1, 1);
	Vector4 mVec4 = new Vector4 (0, 0, 0, 0);
	protected override void DoAnimation(float dt){

		mTime += dt;
		if(mTime >= 1)
			mTime -= 1;
		
		mfEval = _Curve.Evaluate(mTime);
		mVec4 = _Range * mfEval;
		mTempColor.r = mOriginal.r + mVec4.x;
		mTempColor.g = mOriginal.g + mVec4.y;
		mTempColor.b = mOriginal.b + mVec4.z;
		mTempColor.a = mOriginal.a + mVec4.w;
		mImage.color = mTempColor;
	}
}
