using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CMoveTranslateTo : CAction
	{
		Transform mTargetTransform = null;
		Vector3 mvDestinationPos = new Vector3(0,0,0);
		Vector3 mvStartPos = new Vector3(0,0,0);
		Vector3 mvDiffPos = new Vector3(0,0,0);
		Vector3 mvCurPos = new Vector3(0,0,0);
		bool mbLocal = true;

		static public CMoveTranslateTo Move(Transform pTargetObject, Vector3 pEndPos,float pDuration)
		{
			//Default move local position
			return CMoveTranslateTo.Move(pTargetObject,pEndPos,pDuration,true);
		}

		static public CMoveTranslateTo Move(Transform pTargetObject, Vector3 pEndPos,float pDuration,bool pbLocal)
		{
			CMoveTranslateTo tMove = new CMoveTranslateTo();
			tMove.actionWith(pTargetObject,pEndPos,pDuration,pbLocal);
			return tMove;
		}

		protected void actionWith(Transform pTargetObject, Vector3 pEndPos,float pDuration,bool pbLocal)
		{
			mvDestinationPos.Set(pEndPos.x,pEndPos.y,pEndPos.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
			
		override public void runAction()
		{
			mvStartPos = mTargetTransform.position;
			mvCurPos = mvStartPos;

			if(mbLocal)
				mvStartPos = mTargetTransform.localPosition;

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

		Vector3 tTargetPos = new Vector3(0,0,0);
		Vector3 tTranslatePos = new Vector3(0,0,0);
		override public void setCompletion(float pfCompletion)
		{
			if(mTargetTransform == null)
			{
				cancelAction();
				return;
			}

			if(mbLocal)
				mvCurPos = mTargetTransform.localPosition;
			else
				mvCurPos = mTargetTransform.position;

			tTargetPos.Set(mvDiffPos.x*pfCompletion,mvDiffPos.y*pfCompletion,mvDiffPos.z*pfCompletion);
			tTargetPos.Set(mvStartPos.x+tTargetPos.x,mvStartPos.y+tTargetPos.y,mvStartPos.z+tTargetPos.z);
			tTranslatePos.Set(tTargetPos.x - mvCurPos.x,tTargetPos.y - mvCurPos.y,tTargetPos.z - mvCurPos.z);

			if(mbLocal)
				mTargetTransform.Translate(tTranslatePos,Space.Self);
			else
				mTargetTransform.Translate(tTranslatePos,Space.World);
		}

		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CMoveTranslateTo tMove = CMoveTranslateTo.Move(mTargetTransform,mvDestinationPos * -1, _Duration);
			return tMove;
		}
	}
}
