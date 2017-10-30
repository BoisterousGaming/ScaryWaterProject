using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CCompletionFromTo : CAction
	{
		//CALLBACK DELEGATE
		public delegate void CallBack(float pCompletion);
		CallBack animationCompleteCallBack = null;
		float miInitialValue = 0;
		float miFinalValue = 0;
		float miDiffValue = 0;
		float miCurValue = 0;

		static public CCompletionFromTo Completion(float pDuration,float pfInitialValue, float pfFinalValue,CallBack pCompletionCallBack)
		{
			CCompletionFromTo tCompletion = new CCompletionFromTo();
			tCompletion.actionWith(pDuration,pfInitialValue,pfFinalValue,pCompletionCallBack);
			return tCompletion;
		}

		void actionWith(float pDuration,float pfInitialValue, float pfFinalValue, CallBack pCompletionCallBack)
		{
			_Duration = pDuration;
			mfCompletion = 0;
			animationCompleteCallBack = pCompletionCallBack;
		}

		override public void runAction()
		{
			miDiffValue = miFinalValue - miInitialValue;
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				mfRate = 1.0f/_Duration;		
				mfCompletion = 0.0f;
				meState = ActionState.ActionRun;
			}
		}
		
		override public void setCompletion(float pfCompletion)
		{
			if(animationCompleteCallBack == null)
			{
				cancelAction();
				return;
			}

			miCurValue = miDiffValue * pfCompletion;
			miCurValue += miInitialValue;
			animationCompleteCallBack(miCurValue);
		}

		override public CAction reverseAction()
		{
			CCompletionFromTo tCompletion = CCompletionFromTo.Completion(_Duration,miFinalValue,miInitialValue,animationCompleteCallBack);
			return tCompletion;
		}
	}
}