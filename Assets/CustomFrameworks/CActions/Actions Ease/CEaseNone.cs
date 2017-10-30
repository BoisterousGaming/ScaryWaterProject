using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CEaseNone : CEaseAction
	{
		//Convenient method
		static public CEaseNone EaseWith(CAction pAction)
		{
			CEaseNone tEase = new CEaseNone();
			tEase.actionWithAction(pAction);
			return tEase;
		}

		//Save the inner action and the ease curve
		protected void actionWithAction(CAction pAction)
		{
			mInnerAction = pAction;
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
			float tEvaluatedValue = pValue;
			if(mEase != null)
				tEvaluatedValue = mEase.Evaluate(tEvaluatedValue);
			
			return tEvaluatedValue;
		}
	}
}
