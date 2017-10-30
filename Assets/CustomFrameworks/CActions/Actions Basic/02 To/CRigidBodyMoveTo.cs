using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CRigidBodyMoveTo : CAction
	{
		Transform mTargetTransform = null;
		Rigidbody mTargetBody = null;
		Vector3 mvDestinationPos = new Vector3(0,0,0);
		Vector3 mvStartPos = new Vector3(0,0,0);
		Vector3 mvDiffPos = new Vector3(0,0,0);
		Vector3 mvCurPos = new Vector3(0,0,0);
		Vector3 mvTransformOriginalPos = new Vector3(0,0,0);
		bool mbLocal = true;

		static public CRigidBodyMoveTo Move(Transform pTargetObject, Vector3 pEndPos,float pDuration)
		{
			//Default move local position
			return CRigidBodyMoveTo.Move(pTargetObject,pEndPos,pDuration,true);
		}

		static public CRigidBodyMoveTo Move(Transform pTargetObject, Vector3 pEndPos,float pDuration,bool pbLocal)
		{
			CRigidBodyMoveTo tMove = new CRigidBodyMoveTo();
			tMove.actionWith(pTargetObject,pEndPos,pDuration,pbLocal);
			return tMove;
		}

		protected void actionWith(Transform pTargetObject, Vector3 pEndPos,float pDuration,bool pbLocal)
		{
			mvDestinationPos.Set(pEndPos.x,pEndPos.y,pEndPos.z);
			mTargetTransform = pTargetObject;
			mTargetBody = mTargetTransform.GetComponent<Rigidbody>();
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
			
		override public void runAction()
		{
			mvTransformOriginalPos = mTargetTransform.position;
			mvStartPos = mTargetTransform.position;
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
			
		override public void setCompletion(float pfCompletion)
		{
			if(mTargetTransform == null)
			{
				cancelAction();
				return;
			}

			mvCurPos.Set(mvDiffPos.x*pfCompletion,mvDiffPos.y*pfCompletion,mvDiffPos.z*pfCompletion);
			mvCurPos.Set(mvStartPos.x+mvCurPos.x,mvStartPos.y+mvCurPos.y,mvStartPos.z+mvCurPos.z);
			if(mbLocal)
				mTargetBody.MovePosition(mvTransformOriginalPos+mvCurPos);
			else
				mTargetBody.MovePosition(mvCurPos);
		}

		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CRigidBodyMoveTo tMove = CRigidBodyMoveTo.Move(mTargetTransform,mvDestinationPos * -1, _Duration);
			return tMove;
		}
	}
}
