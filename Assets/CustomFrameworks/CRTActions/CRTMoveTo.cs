using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CRTMoveTo : CAction 
	{
		RectTransform mTargetTransform = null;
		Vector3 mvDestinationPos = new Vector3(0,0,0);
		Vector3 mvStartPos = new Vector3(0,0,0);
		Vector3 mvDiffPos = new Vector3(0,0,0);
		Vector3 mvCurPos = new Vector3(0,0,0);

		static public CRTMoveTo Move(RectTransform pTargetObject, Vector3 pEndPos,float pDuration)
		{
			CRTMoveTo tMove = new CRTMoveTo();
			tMove.actionWith(pTargetObject,pEndPos,pDuration);
			return tMove;
		}

		protected void actionWith(RectTransform pTargetObject, Vector3 pEndPos,float pDuration)
		{
			mvDestinationPos.Set(pEndPos.x,pEndPos.y,pEndPos.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
		}

		override public void runAction()
		{
			mvStartPos = mTargetTransform.anchoredPosition;
			mvDiffPos.Set(mvDestinationPos.x - mvStartPos.x,mvDestinationPos.y - mvStartPos.y,mvDestinationPos.z - mvStartPos.z);

			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				mfRate = 1.0f/_Duration;		
				mfCompletion = 0.0f;
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

			if(pfCompletion != 1)
			{
				mvCurPos.Set(mvDiffPos.x*pfCompletion,mvDiffPos.y*pfCompletion,mvDiffPos.z*pfCompletion);
				mvCurPos.Set(mvStartPos.x+mvCurPos.x,mvStartPos.y+mvCurPos.y,mvStartPos.z+mvCurPos.z);
			}
			else
			{
				mvCurPos.Set(mvDestinationPos.x,mvDestinationPos.y,mvDestinationPos.z);
			}

			mTargetTransform.anchoredPosition = mvCurPos;
		}

		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}

		override public CAction reverseAction()
		{
			CRTMoveTo tMove = CRTMoveTo.Move(mTargetTransform,mvDestinationPos * -1, _Duration);
			return tMove;
		}
	}
}