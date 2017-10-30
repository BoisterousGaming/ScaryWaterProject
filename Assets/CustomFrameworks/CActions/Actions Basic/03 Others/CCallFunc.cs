using System;

namespace CAnimationFramework.CAction
{
	public class CCallFunc : CAction 
	{
		//CALLBACK ACTION
		Action animationCompleteCallBack = null;
			
		//CONVENIENT METHODS
		static public CCallFunc CallBackWith(Action pCompletionCallBack)
		{
			CCallFunc tCallBack = new CCallFunc();
			tCallBack.actionWithCallBack(pCompletionCallBack);
			return tCallBack;
		}

		//SETTING THE CALL BACK
		protected void actionWithCallBack(Action pCompletionCallBack)
		{
			animationCompleteCallBack = pCompletionCallBack;
		}
		
		//RUN THE ACTION
		override public void runAction()
		{
			if(animationCompleteCallBack != null)
				animationCompleteCallBack();
			OnActionComplete();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			if(animationCompleteCallBack != null)
				animationCompleteCallBack();
			OnActionComplete();
		}
	}
}
