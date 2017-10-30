using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace CAnimationFramework.CAction
{
	public class CSpawnAction : CAction 
	{
		// Use this for initialization
		List<CAction> marrActions;
		int miActionIndex = 0;
		int miActionCount = 0;

		//Convenient method
		static public CSpawnAction SpawnWithActions(List<CAction> pActionList)
		{
			CAction[] tActions = new CAction[pActionList.Count];
			int tindex = 0;
			foreach (CAction tAct in pActionList) {
				tActions[tindex++] = tAct;
			}
			return CSpawnAction.SpawnWithActions(tActions);
		}

		//Convenient method
		static public CSpawnAction SpawnWithActions(params CAction[] actions)
		{
			CSpawnAction tSpawnAction = new CSpawnAction();
			tSpawnAction.actionWithActions(actions);
			return tSpawnAction;
		}

		//Assign all internal actions
		protected void actionWithActions(params CAction[] actions)
		{
			_Duration = 0;
			marrActions = actions.ToList();
			miActionCount = marrActions.Count;
			float tfActionDuration = _Duration;
			for(int i = 0; i < miActionCount;i++)
			{
				tfActionDuration = marrActions[i]._Duration;
				if(tfActionDuration >= _Duration)
					_Duration = tfActionDuration;
			}

			mfRate = 1.0f/_Duration;
			foreach (CAction action in actions)
	        	action.InternalCallBack = OnInternalCallBack;
		}

		//Add new action into the list
		public void AddAction(CAction action)
		{
			marrActions.Add(action);
			miActionCount = marrActions.Count;

			if(action._Duration >= _Duration)
				_Duration = action._Duration;

			mfRate = 1.0f/_Duration;
			action.InternalCallBack = OnInternalCallBack;
		}
		
		override public void runAction()
		{
			if(marrActions.Count == 0 || _Duration == 0)
				OnActionComplete();
			else
			{
				mfCompletion = 0;
				miActionIndex = 0;
				meState = ActionState.ActionRun;
				foreach (CAction action in marrActions)
					action.runAction();
			}
		}
		
		override public void pauseAction()
		{
			meState = ActionState.ActionPause;
			foreach (CAction action in marrActions)
	        	action.pauseAction();
		}
		
		override public void resumeAction()
		{
			meState = ActionState.ActionRun;
			foreach (CAction action in marrActions)
	        	action.resumeAction();
		}

		//Send Skip message to all sub actions
		override public void skipAction()
		{
			foreach(CAction tAction in marrActions)
			{
				tAction.skipAction();
			}
		}

		//Received from sub actions
		protected override void OnInternalCallBack(CAction action)
		{
			miActionIndex++;
			if(miActionIndex >= miActionCount)
			{
				OnActionComplete();
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


		//Update all subactions
		override public void Update(float dt)
		{
			if(meState == ActionState.ActionRun)
			{
				foreach(CAction tAction in marrActions)
				{
					tAction.Update(dt);
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
			CSpawnAction tSpw = CSpawnAction.SpawnWithActions(tArray);
			return tSpw;
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