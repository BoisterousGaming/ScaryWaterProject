using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CEaseElastic : CEaseAction 
	{
		EaseType meEaseType = EaseType.EaseInOut;
		float mfPeriod = 0.3f;

		//Convenient method
		static public CEaseElastic EaseWith(CAction pAction,EaseType pEaseType)
		{
			CEaseElastic tEase = new CEaseElastic();
			tEase.actionWithAction(pAction,pEaseType,0.3f);
			return tEase;
		}

		static public CEaseElastic EaseWith(CAction pAction,EaseType pEaseType,float pfPeriod)
		{
			CEaseElastic tEase = new CEaseElastic();
			tEase.actionWithAction(pAction,pEaseType,pfPeriod);
			return tEase;
		}
		
		//Save the inner action and the ease curve
		protected void actionWithAction(CAction pAction,EaseType pEaseType,float pfPeriod)
		{
			mInnerAction = pAction;
			mInnerAction.InternalCallBack = OnInternalCallBack;
			meEaseType = pEaseType;
			mfPeriod = pfPeriod;

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


			float tfValue1;
			float tfValue2;
			float tfValue3 = mfPeriod / 4.0f;
			float tEvaluatedValue = 0;
			switch(meEaseType)
			{
				case EaseType.EaseIn:
					tfValue1 = 0;
					tfValue2 = pValue;
					if (pValue == 0 || pValue == 1)
						tfValue1 = pValue;
					else 
					{
						tfValue2 = tfValue2 - 1.0f;
						tfValue1 = -1*Mathf.Pow(2, 10 * tfValue2) * Mathf.Sin( (tfValue2-tfValue3) *(Mathf.PI*2) / mfPeriod);
					}
					tEvaluatedValue = tfValue1;
					break;
				case EaseType.EaseOut:
					tfValue1 = 0;
					tfValue2 = pValue;
					if (pValue == 0 || pValue == 1)
						tfValue1 = pValue;
					
					else 
					{
						tfValue1 = Mathf.Pow(2, -10 * tfValue2) * Mathf.Sin( (tfValue2-tfValue3) *(Mathf.PI*2) / mfPeriod) + 1;
					}
					tEvaluatedValue = tfValue1;
					break;
				case EaseType.EaseInOut:
					if (pValue == 0 || pValue == 1)
						tfValue1 = pValue;
					
					else 
					{
						tfValue2 = 2*pValue;
						tfValue2 = tfValue2 - 1.0f;
						if( tfValue2 < 0 )
							tfValue1 = -0.5f * Mathf.Pow(2, 10 * tfValue2) * Mathf.Sin( (tfValue2-tfValue3) *(Mathf.PI*2) / mfPeriod);
						else
							tfValue1 = Mathf.Pow(2, -10 * tfValue2) * Mathf.Sin( (tfValue2-tfValue3) *(Mathf.PI*2) / mfPeriod)*0.5f + 1;
					}
					tEvaluatedValue = tfValue1;
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
