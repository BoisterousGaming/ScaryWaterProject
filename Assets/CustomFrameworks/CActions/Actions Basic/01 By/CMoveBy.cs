using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CMoveBy : CAction 
	{
		Transform mTargetTransform = null;
		Vector3 mvPosDiff = new Vector3(0,0,0);
		Vector3 mvCurPosDiff = new Vector3(0,0,0);
		float mfPreviousCompletion = 0.0f;
		bool mbLocal = true;
		
		static public CMoveBy Move(Transform pTargetObject, Vector3 pPosDiff,float pDuration)
		{
			//Default move local position
			return CMoveBy.Move(pTargetObject,pPosDiff,pDuration,true);
		}
		
		static public CMoveBy Move(Transform pTargetObject, Vector3 pPosDiff,float pDuration,bool pbLocal)
		{
			CMoveBy tMove = new CMoveBy();
			tMove.actionWith(pTargetObject,pPosDiff,pDuration,pbLocal);
			return tMove;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pPosDiff,float pDuration,bool pbLocal)
		{
			mvPosDiff.Set(pPosDiff.x,pPosDiff.y,pPosDiff.z);
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

			mvCurPosDiff.Set(mvPosDiff.x*tfCompletionDiff,mvPosDiff.y*tfCompletionDiff,mvPosDiff.z*tfCompletionDiff);
			if(mbLocal)
				mTargetTransform.Translate(mvCurPosDiff,Space.Self);
			else
				mTargetTransform.Translate(mvCurPosDiff,Space.World);
		}

		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}

		override public CAction reverseAction()
		{
			CMoveBy tMove = CMoveBy.Move(mTargetTransform,mvPosDiff * -1, _Duration);
			return tMove;
		}
	}
}
