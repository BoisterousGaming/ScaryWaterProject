using UnityEngine;
using System;
using System.Collections;

namespace CAnimationFramework.CAction
{
	public class CWaitForEvent : CAction 
	{
		//CALLBACK ACTION
		string mEventString = "Default";
		bool mDidReceiveEvent = false;

		//CONVENIENT METHODS
		static public CWaitForEvent WaitForCall(string eventString)
		{
			CWaitForEvent tCall = new CWaitForEvent();
			tCall.actionWith(eventString);
			return tCall;
		}

		//SETTING THE EVENT
		protected void actionWith(string eventString)
		{
			mEventString = eventString;
		}
			
		public override void runAction()
		{
			meState = ActionState.ActionRun;
		}

		protected override void PerformAction()
		{
			
		}

		public override void receivedEvent(string pEvent) 
		{
			if(pEvent == mEventString && !mDidReceiveEvent)
			{
				mDidReceiveEvent = true;
				OnActionComplete();
			}
		}

		public override void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
	}
}