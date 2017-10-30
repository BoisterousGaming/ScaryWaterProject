using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CEaseExponential : CEaseAction
	{
		EaseType meEaseType = EaseType.EaseInOut;

		//Convenient method
		static public CEaseExponential EaseWith(CAction pAction,EaseType pEaseType)
		{
			CEaseExponential tEase = new CEaseExponential();
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
					tEvaluatedValue = (pValue==0) ? 0 :(Mathf.Pow(2,10*(pValue/1.0f-1.0f))-1 * 0.001f);
					break;
				case EaseType.EaseOut:
					tEvaluatedValue = (pValue==1) ? 1 :(-1*Mathf.Pow(2,-10*(pValue/1.0f))+1);
					break;
				case EaseType.EaseInOut:
					tfTempRatio = 2*pValue;
					if (tfTempRatio <= 1) 
						tEvaluatedValue =  0.5f * Mathf.Pow(2,10*(tfTempRatio-1));
					else
						tEvaluatedValue =  0.5f * (-1*Mathf.Pow(2,-10*(tfTempRatio-1))+2);
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
