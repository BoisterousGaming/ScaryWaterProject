using UnityEngine;
using System;

namespace CAnimationFramework.CAction
{
	public enum eActionUpdateMode
	{
		FixedUpdate,
		Update,
		LateUpdate,
	}

	public enum eActionTimeMode
	{
		FPSDependant,
		FPSIndependant,
	}

	public class CActionSet 
	{
		public delegate void internalCallBack(CAction pAction);
		protected internalCallBack internalAnimationCallBack = null;
		public Action ActionCompleteCallBack = null;

		//PRIVATE VARIABLES
		CAction mMainAction;
		eActionUpdateMode meUpdateMode = eActionUpdateMode.Update;
		eActionTimeMode meTimeMode = eActionTimeMode.FPSDependant;
		float mAnimationRate = 0.02f;

		//PUBLIC CONVENIENT METHODS
		//-----------------------------------------------------------------------------------------------------

		static public CActionSet CreateWithMainAction(CAction action,Action callback = null,eActionUpdateMode updateMode = eActionUpdateMode.Update,eActionTimeMode timeMode = eActionTimeMode.FPSDependant)
		{
			CActionSet tActionSet = new CActionSet();
			tActionSet.SetAction(action,callback,updateMode,timeMode);
			return tActionSet;
		}

		//-----------------------------------------------------------------------------------------------------
		//PUBLIC METHODS
		//-----------------------------------------------------------------------------------------------------

		public void SetAnimationRate(float rate)
		{
			mAnimationRate = rate;
		}

		public void Play()
		{
			//Run the animation
			mMainAction.runAction();
		}
		
		public void Resume()
		{
			//Resume the animation if paused
			mMainAction.resumeAction();
		}
		
		public void Pause()
		{
			//Pause the animation if running
			mMainAction.pauseAction();
		}

		public void Skip()
		{
			mMainAction.skipAction();
		}

		public void Stop()
		{
			ActionCompleteCallBack = null;
		}
		
		public void setTimeModifier(float pTimeModifier)
		{
			//Set time modifier parameter in all animations
			mMainAction.setTimeModifier(pTimeModifier);
		}

		//-----------------------------------------------------------------------------------------------------
		//PRIVATE METHODS
		//-----------------------------------------------------------------------------------------------------

		//Assign the main action Call Play after this to run the action
		void SetAction(CAction action,Action callback,eActionUpdateMode updateMode = eActionUpdateMode.Update,eActionTimeMode timeMode = eActionTimeMode.FPSDependant)
		{
			ActionCompleteCallBack = callback;
			mMainAction = action;
			mMainAction.InternalCallBack = AllAnimationsComplete;
			meUpdateMode = updateMode;
			meTimeMode = timeMode;
		}
		
		//Called after all animations have been completed
		void AllAnimationsComplete(CAction pAction)
		{
			CSharedActionsHandler.Instance.CompletedActionSet(this);
			if(ActionCompleteCallBack != null)
				ActionCompleteCallBack();
		}

		//Update is called once per frame
		public void Update () 
		{
			if (meUpdateMode == eActionUpdateMode.Update)
			{
				if (mMainAction != null) 
				{
					//Main action will propagate it to all sub actions
					//all sub action need not be running at the moment
					if (mMainAction.State == ActionState.ActionRun) 
					{
						if (meTimeMode == eActionTimeMode.FPSDependant)
							mMainAction.Update (Time.deltaTime);
						else
							mMainAction.Update (mAnimationRate);
					}
				}
			}
		}

		public void FixedUpdate () 
		{
			if (meUpdateMode == eActionUpdateMode.FixedUpdate)
			{
				if (mMainAction != null) 
				{
					//Main action will propagate it to all sub actions
					//all sub action need not be running at the moment
					if (mMainAction.State == ActionState.ActionRun) 
					{
						if (meTimeMode == eActionTimeMode.FPSDependant)
							mMainAction.Update (Time.deltaTime);
						else
							mMainAction.Update (mAnimationRate);
					}
				}
			}
		}

		public void LateUpdate () 
		{
			if (meUpdateMode == eActionUpdateMode.LateUpdate)
			{
				if (mMainAction != null) 
				{
					//Main action will propagate it to all sub actions
					//all sub action need not be running at the moment
					if (mMainAction.State == ActionState.ActionRun) 
					{
						if (meTimeMode == eActionTimeMode.FPSDependant)
							mMainAction.Update (Time.deltaTime);
						else
							mMainAction.Update (mAnimationRate);
					}
				}
			}
		}

		public void ReceivedEvent(string pEvent)
		{
			mMainAction.receivedEvent(pEvent);
		}
	}
}
