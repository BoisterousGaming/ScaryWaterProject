using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CJumpBy : CAction {
		
		// Use this for initialization
		Transform mTargetTransform = null;
		Vector3 mvPosDiff = new Vector3(0,0,0);
		Vector3 mvCurPosDiff = new Vector3(0,0,0);
		float mfPreviousCompletion = 0.0f;
		float mfPreviousJumpMagnitude = 0;
		float mfHeight = 0;
		int miJumps = 1;
		bool mbLocal = true;
		
		static public CJumpBy Jump(Transform pTargetObject, Vector3 pPosDiff,float pDuration,float pfHt)
		{
			//Default move local position
			return CJumpBy.Jump(pTargetObject,pPosDiff,pDuration,pfHt,1,true);
		}
		
		static public CJumpBy Jump(Transform pTargetObject, Vector3 pPosDiff,float pDuration,float pfHt,int piJumps,bool pbLocal)
		{
			CJumpBy tJump = new CJumpBy();
			tJump.actionWith(pTargetObject,pPosDiff,pDuration,pfHt,piJumps,pbLocal);
			return tJump;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pPosDiff,float pDuration,float pfHt,int piJumps,bool pbLocal)
		{
			mvPosDiff.Set(pPosDiff.x,pPosDiff.y,pPosDiff.z);
			mfHeight = pfHt;
			miJumps = piJumps;
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
		
		override public void runAction()
		{
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				mfRate = 1.0f/_Duration;		
				mfCompletion = 0.0f;
				mfPreviousCompletion = 0.0f;
				mfPreviousJumpMagnitude = 0;
				meState = ActionState.ActionRun;
			}
		}
		
		override public void setCompletion(float pfCompletion)
		{
			if(mTargetTransform == null)
			{
				cancelAction();
				return;
			}

			float tfCompletionDiff = pfCompletion - mfPreviousCompletion;
			mfPreviousCompletion = pfCompletion;

			float tJumpMagnitude = Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad *  pfCompletion * (miJumps * 180)))  * mfHeight * (1 - pfCompletion);
			float tJumpDiff = tJumpMagnitude - mfPreviousJumpMagnitude;
			mfPreviousJumpMagnitude = tJumpMagnitude;

			mvCurPosDiff.Set(mvPosDiff.x*tfCompletionDiff,mvPosDiff.y*tfCompletionDiff,mvPosDiff.z*tfCompletionDiff);
			mvCurPosDiff.y += tJumpDiff;

			if(mbLocal)
				mTargetTransform.Translate(mvCurPosDiff,Space.Self);
			else
				mTargetTransform.Translate(mvCurPosDiff,Space.World);
		}
		
		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CJumpBy tJump = CJumpBy.Jump(mTargetTransform,mvPosDiff*-1,_Duration,mfHeight,miJumps,mbLocal);
			return tJump;
		}
	}
}
