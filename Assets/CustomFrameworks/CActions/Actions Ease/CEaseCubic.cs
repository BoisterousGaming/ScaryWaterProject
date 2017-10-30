using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CEaseCubic : CEaseAction 
	{
		EaseType meEaseType = EaseType.EaseInOut;
		float mfInverseRate = 0.0f;

		//Convenient method
		static public CEaseCubic EaseWith(CAction pAction,EaseType pEaseType)
		{
			CEaseCubic tEase = new CEaseCubic();
			tEase.actionWithAction(pAction,pEaseType);
			return tEase;
		}
		
		//Save the inner action and the ease curve
		protected void actionWithAction(CAction pAction,EaseType pEaseType)
		{
			mInnerAction = pAction;
			mInnerAction.InternalCallBack = OnInternalCallBack;
			meEaseType = pEaseType;

			if(mInnerAction != null)
				_Duration = mInnerAction._Duration;

		}
		
		override public void runAction()
		{
			if(_Duration <= 0)
			{
				skipAction();
			}
			else if(mInnerAction != null)
			{
				mfRate = 1.0f/_Duration;
				mfInverseRate = 1.0f/mfRate;

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
		
		override protected void OnInternalCallBack(CAction pAction)
		{	
			mInnerAction.setEase(null);
			OnActionComplete();	
		}
		
		override public void Update(float dt)
		{
			if(meState == ActionState.ActionRun)
			{
				//Propagating the update to the internal action if it has a curve action
				//Only works for CSpawn and CSequence actions
				mInnerAction.Update(dt);
			}
		}
		
		override public float Evaluate(float pValue)
		{
			if(_Duration <= 0)
				return 1;

			float tfTempRatio = 0;
			float tEvaluatedValue = 0;
			switch(meEaseType)
			{
				case EaseType.EaseIn:
					tEvaluatedValue =  Mathf.Pow(mfTimeRatio,mfRate);
					break;
				case EaseType.EaseOut:
					tEvaluatedValue =  Mathf.Pow(mfTimeRatio,mfInverseRate);
					break;
				case EaseType.EaseInOut:
					tfTempRatio = 2*pValue;
					if (pValue <= 0.5f) 
						tEvaluatedValue =  0.5f * Mathf.Pow(tfTempRatio,mfRate);
					else
						tEvaluatedValue =  1.0f - (0.5f * Mathf.Pow(2 - tfTempRatio,mfRate));
					break;
				default:
					break;
			}

			if(mEase != null)
				tEvaluatedValue = mEase.Evaluate(tEvaluatedValue);

			return tEvaluatedValue;
		}
	}
}