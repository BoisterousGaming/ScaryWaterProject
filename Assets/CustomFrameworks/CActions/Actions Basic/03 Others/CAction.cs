using UnityEngine;

namespace CAnimationFramework.CAction
{
	public enum ActionState
	{
		ActionNotRunning,
		ActionRun,
		ActionPause,
	};

	public class CAction 
	{
		//ANIMATION DELEGATE
		public delegate void internalCallBack(CAction pAction);
		protected internalCallBack internalAnimationCallBack = null;

		//PUBLIC VARIABLES
		public float _Duration = 1.0f;
		public bool _Cancelled = false;

		//PROTECTED VARIABLES
		protected CEaseAction  mEase;
		protected float mfRate = 1.0f;
		protected float mfCompletion = 0.0f;
		protected float mfTimeModifier = 1.0f;
		protected float mfDefaultFrameRate = 0.02f;
		protected float mfEvaluatedValue;
		protected ActionState meState = ActionState.ActionNotRunning;
		protected bool mbReverse = false;
		protected bool mbSkip = false;

		//GETTERS AND SETTERS
		//-----------------------------------------------------------------------------------------------------

		public ActionState State {get {return meState;}}
		public internalCallBack InternalCallBack {set {internalAnimationCallBack = value;}}

		//-----------------------------------------------------------------------------------------------------
		//PUBLIC VIRTUAL METHODS
		//-----------------------------------------------------------------------------------------------------

		public virtual void Update(float dt)
		{
			//Normal self update according to duration
			if(meState == ActionState.ActionRun)
			{
				mfCompletion += dt * mfRate;
				PerformAction();
			}
		}
		public virtual void runAction()
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
		public virtual void pauseAction()
		{
			if(meState == ActionState.ActionRun)
				meState = ActionState.ActionPause;
		}
		public virtual void resumeAction()
		{
			if(meState == ActionState.ActionPause)
				meState = ActionState.ActionRun;
		}
		public virtual void skipAction() 
		{
			setCompletion(1);
			OnActionComplete();
		}

		public virtual void cancelAction()
		{
			_Cancelled = true;
			OnActionComplete();
		}

		public virtual void receivedEvent(string pEvent) {}
		public virtual void setCompletion(float pfCompletion) {}
		public virtual void setTimeModifier(float pfModifier) {mfTimeModifier = pfModifier;}
		public virtual CAction reverseAction() { return new CAction();}
		public virtual bool setEase(CEaseAction pEase) {mEase = pEase; return true;}
		//-----------------------------------------------------------------------------------------------------
		//PROTECTED VIRTUAL METHODS
		//-----------------------------------------------------------------------------------------------------


		protected virtual void PerformAction()
		{
			if(mfCompletion >= 0 && mfCompletion < 1.0f)
			{
				mfEvaluatedValue = mfCompletion;
				if(mEase != null)
					mfEvaluatedValue = mEase.Evaluate(mfEvaluatedValue);

				setCompletion(mfEvaluatedValue);
			}
			else if(mfCompletion >= 1.0f)
			{
				mfCompletion = 1.0f;
				mfEvaluatedValue = mfCompletion;
				if(mEase != null)
					mfEvaluatedValue = mEase.Evaluate(mfEvaluatedValue);

				setCompletion(mfEvaluatedValue);
				OnActionComplete();
			}
		}
		protected virtual void OnActionComplete()
		{
			meState = ActionState.ActionNotRunning;
			if(internalAnimationCallBack != null)
				internalAnimationCallBack(this);
		}
		protected virtual void OnInternalCallBack(CAction pAction){	OnActionComplete();	}

	}
}