namespace CAnimationFramework.CAction
{
	public enum EaseType
	{
		EaseIn,
		EaseOut,
		EaseInOut,
	}

	public enum EaseActionType
	{
		EaseNone,
		EaseNormal,
		EaseCubic,
		EaseExponential,
		EaseElastic,
	}

	public class CEaseAction : CAction 
	{
		// Use this for initialization
		public EaseType meEasetype = EaseType.EaseIn;
		
		protected CAction mInnerAction;
		protected float mfInnerActionCompletion = 0.0f;
		protected float mfTime = 0.0f;
		protected float mfTimeRatio = 0.0f;

		override public void setCompletion(float pCompletion)
		{
			mInnerAction.setCompletion(pCompletion);
		}

		public void setInnerAction(CAction pInnerAction)
		{
			mInnerAction = pInnerAction;
			_Duration = mInnerAction._Duration;
		}

		override public void skipAction() 
		{
			setCompletion(1);
			OnActionComplete();
		}

		public virtual float Evaluate(float pValue)
		{
			return 1;
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

		public override void receivedEvent(string pEvent) 
		{
			if(mInnerAction != null)
				mInnerAction.receivedEvent(pEvent);
		}
	}
}
