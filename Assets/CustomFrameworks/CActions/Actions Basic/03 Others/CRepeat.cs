namespace CAnimationFramework.CAction
{
	public class CRepeat : CAction 
	{
		CAction mInnerAction;
		int miRepeatCount = 0;
		int miRepeatCountRequired = 0;

		//Convenience method
		static public CRepeat Repeat(CAction pAction,int pLoops)
		{
			CRepeat tRepeat = new CRepeat();
			tRepeat.actionWithAction(pAction,pLoops);
			return tRepeat;
		}

		//Initializer
		protected void actionWithAction(CAction pAction,int pLoops)
		{
			mInnerAction = pAction;
			mInnerAction.InternalCallBack = OnInternalCallBack;
			miRepeatCountRequired = pLoops;
		}

		//Start running the Action 
		override public void runAction()
		{
			miRepeatCount = miRepeatCountRequired;
			meState = ActionState.ActionRun;

			if(miRepeatCountRequired != 0)
			{
				if(mInnerAction != null)
					mInnerAction.runAction();
			}
			else
			{
				//No Repeats required
				OnActionComplete();
			}
		}

		//Pause the action
		override public void pauseAction()
		{
			meState = ActionState.ActionPause;
			if(mInnerAction != null)
				mInnerAction.pauseAction();
		}

		//Resume the action
		override public void resumeAction()
		{
			meState = ActionState.ActionRun;
			if(mInnerAction != null)
				mInnerAction.resumeAction();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			mInnerAction.skipAction();
		}
		
		protected override void OnInternalCallBack(CAction action)
		{
			//Reduce repeat count till it becomes zero
			if(miRepeatCount > 0)
				miRepeatCount--;

			//Animation completed when no more repeats are required
			if(miRepeatCount == 0)
			{
				OnActionComplete();
				return;
			}
			mInnerAction.runAction();
		}

		public override void Update(float dt)
		{
			//Propagating the update to target action
			if(mInnerAction.State == ActionState.ActionRun)
				mInnerAction.Update(dt);
		}
	}
}