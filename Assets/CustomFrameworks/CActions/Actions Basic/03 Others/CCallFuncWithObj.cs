using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CCallFuncWithObj : CAction 
	{
		//CALLBACK DELEGATE
	    public delegate void CallBack(GameObject obj);
		CallBack animationCompleteCallBack = null;

		//PRIVATE VARIABLE
		GameObject mCallBackGameObject;


		//SETTING CALL BACK DELEGATE
		protected void actionWithCallBack(CallBack pCompletionCallBack,GameObject pGameObject)
		{
			_Duration = 0;
			mfCompletion = 0;
			animationCompleteCallBack = pCompletionCallBack;
			mCallBackGameObject = pGameObject;
		}
		
		//RUN THE ACTION
		override public void runAction()
		{
			if(animationCompleteCallBack != null)
				animationCompleteCallBack(mCallBackGameObject);
			OnActionComplete();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			if(animationCompleteCallBack != null)
				animationCompleteCallBack(mCallBackGameObject);
			OnActionComplete();
		}
	}
}