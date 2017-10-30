using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CEaseWRTCurve : CEaseAction
	{
		AnimationCurve mCurve;

		//Convenient method
		static public CEaseWRTCurve EaseWith(CAction pAction,AnimationCurve pCurve)
		{
			CEaseWRTCurve tEase = new CEaseWRTCurve();
			tEase.actionWithAction(pAction,pCurve);
			return tEase;
		}

		//Save the inner action and the ease curve
		protected void actionWithAction(CAction pAction,AnimationCurve pCurve)
		{
			mInnerAction = pAction;
			mCurve = pCurve;
			if(mInnerAction != null)
				_Duration = mInnerAction._Duration;
		}

		override public void runAction()
		{
			if(mInnerAction != null)
			{
				mfCompletion = 0.0f;
				mfInnerActionCompletion = 0.0f;
				mInnerAction.setEase(this);
				mInnerAction.InternalCallBack = OnInternalCallBack;
				meState = ActionState.ActionRun;
				mInnerAction.runAction();
			}
		}	

		override public bool setEase(CEaseAction pEase) 
		{
			mEase = pEase;
			return true;
		}

		override public float Evaluate(float pValue)
		{
			float tEvaluatedValue = mCurve.Evaluate(pValue);
			if(mEase != null)
				tEvaluatedValue = mEase.Evaluate(tEvaluatedValue);

			return tEvaluatedValue;
		}
	}
}