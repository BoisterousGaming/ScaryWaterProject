namespace CAnimationFramework.CAction
{
	//All functions are same as basic CAction
	public class CDelayTime : CAction 
	{
		static public CDelayTime Delay(float pDuration)
		{
			CDelayTime tDelay = new CDelayTime();
			tDelay.actionWith(pDuration);
			return tDelay;
		}

		protected void actionWith(float pDuration)
		{
			_Duration = pDuration;
		}

		public override void runAction()
		{
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				meState = ActionState.ActionRun;
				mfRate = 1.0f/_Duration;
				mfCompletion = 0.0f;
			}
		}

		override public CAction reverseAction()
		{
			CDelayTime tDelay = CDelayTime.Delay(_Duration);
			return tDelay;
		}
	}
}