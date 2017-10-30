using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace CAnimationFramework.CAction
{
	public class CSequence : CAction 
	{
		// Use this for initialization
		List<CAction> marrActions;
		int miActionIndex = 0;
		int miActionCount = 0;


		//Convenient method
		static public CSequence SequenceWithActions(List<CAction> pActionList)
		{
			CAction[] tActions = new CAction[pActionList.Count];
			int tindex = 0;
			foreach (CAction tAct in pActionList) {
				tActions[tindex++] = tAct;
			}
			return CSequence.SequenceWithActions(tActions);
		}

		//Convenient Method
		static public CSequence SequenceWithActions(params CAction[] actions)
		{
			CSequence tSeq = new CSequence();
			tSeq.actionWithActions(actions);
			return tSeq;
		}

		//Store all action refrences that will come in sequence
		protected void actionWithActions(params CAction[] actions)
		{
			marrActions = actions.ToList();
			miActionCount = marrActions.Count;

			//Assign callback for each sub action
			for(int i = 0; i < miActionCount;i++)
			{
				marrActions[i].InternalCallBack = OnInternalCallBack;
				_Duration += marrActions[i]._Duration;
			}
		}

		public void AddAction(CAction action)
		{
			marrActions.Add(action);
			_Duration += action._Duration;
			action.InternalCallBack = OnInternalCallBack;
			miActionCount = marrActions.Count;
		}
		
		override public void runAction()
		{
			if(marrActions.Count == 0 || _Duration == 0)
				OnActionComplete();
			else
			{
				miActionIndex = 0;
				mfCompletion = 0;
				meState = ActionState.ActionRun;
				marrActions[miActionIndex].runAction();
			}
		}
		
		override public void pauseAction()
		{
			//Send Pause Message to all sub action
			meState = ActionState.ActionPause;
			foreach (CAction tAction in marrActions)
				tAction.pauseAction();
		}
		
		override public void resumeAction()
		{
			//Send resume Message to all sub action
			meState = ActionState.ActionRun;
			foreach (CAction tAction in marrActions)
				tAction.resumeAction();
		}


		override public void skipAction()
		{
			//Set Skip Flag
			mbSkip = true;
			marrActions[miActionIndex].skipAction();
		}

		//Received from sub actions
		protected override void OnInternalCallBack(CAction action)
		{	
			miActionIndex++;
			if(miActionIndex >= miActionCount)
				OnActionComplete();
			else
			{
				if(!mbSkip)
				{
					//Move to next action
					marrActions[miActionIndex].runAction();
				}
				else
				{
					//Move to next action
					marrActions[miActionIndex].skipAction();
					return;
				}
			}
		}

		//Propagate curve to all sub actions
		override public bool setEase(CEaseAction pEase) 
		{
			mEase = pEase;
			foreach (CAction tAction in marrActions)
				tAction.setEase(mEase);
			return true;
		}

		//Propagating the update to each sub action if its in running mode
		public override void Update(float dt)
		{
			if(meState == ActionState.ActionRun)
			{
				if(miActionIndex < miActionCount && miActionIndex >= 0)
				{
					marrActions[miActionIndex].Update(dt);
				}
			}
		}

		//Reverse of all sequence elements and also reverse of the action
		override public CAction reverseAction()
		{
			CAction[] tArray = new CAction[marrActions.Count];
			for(int i = 0; i < miActionCount; i++)
			{
				tArray[i] = marrActions[miActionCount - i - 1].reverseAction();
			}
			CSequence tSeq = CSequence.SequenceWithActions(tArray);
			return tSeq;
		}

		public override void receivedEvent(string pEvent) 
		{
			for(int i = 0; i < marrActions.Count; i++)
			{
				marrActions[i].receivedEvent(pEvent);
			}
		}
	}
}
