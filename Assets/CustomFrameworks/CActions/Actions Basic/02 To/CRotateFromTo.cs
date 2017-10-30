using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CRotateFromTo : CAction
	{
		Transform mTargetTransform = null;
		Vector3 mvFinalRot = new Vector3(0,0,0);
		Vector3 mvStartRot = new Vector3(0,0,0);
		Vector3 mvDiffRot = new Vector3(0,0,0);
		Vector3 mvCurRot = new Vector3(0,0,0);
		bool mbLocal = true;
		
		static public CRotateFromTo Rotate(Transform pTargetObject,Vector3 pStartRot, Vector3 pEndRot,float pDuration)
		{
			//Default move local position
			return CRotateFromTo.Rotate(pTargetObject,pStartRot,pEndRot,pDuration,true);
		}
		
		static public CRotateFromTo Rotate(Transform pTargetObject,Vector3 pStartRot, Vector3 pEndRot,float pDuration,bool pbLocal)
		{
			CRotateFromTo tRotate = new CRotateFromTo();
			tRotate.actionWith(pTargetObject,pStartRot,pEndRot,pDuration,pbLocal);
			return tRotate;
		}
		
		protected void actionWith(Transform pTargetObject,Vector3 pStartRot, Vector3 pEndRot,float pDuration,bool pbLocal)
		{
			mvStartRot.Set(pStartRot.x,pStartRot.y,pStartRot.z);
			mvFinalRot.Set(pEndRot.x,pEndRot.y,pEndRot.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
		
		override public void runAction()
		{

			if(mbLocal)
				mTargetTransform.localEulerAngles = mvStartRot;
			else
				mTargetTransform.eulerAngles = mvStartRot;
			
			mvDiffRot.Set(mvFinalRot.x - mvStartRot.x,mvFinalRot.y - mvStartRot.y,mvFinalRot.z - mvStartRot.z);
			
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
				mvCurRot.Set(mvDiffRot.x*pfCompletion,mvDiffRot.y*pfCompletion,mvDiffRot.z*pfCompletion);
				mvCurRot.Set(mvStartRot.x+mvCurRot.x,mvStartRot.y+mvCurRot.y,mvStartRot.z+mvCurRot.z);
			}
			else
			{
				mvCurRot.Set(mvFinalRot.x,mvFinalRot.y,mvFinalRot.z);
			}

			if(mbLocal)
				mTargetTransform.localEulerAngles = mvCurRot;
			else
				mTargetTransform.eulerAngles = mvCurRot;
		}
		
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CRotateFromTo tRotate = CRotateFromTo.Rotate(mTargetTransform,mvStartRot,mvFinalRot * -1, _Duration);
			return tRotate;
		}
	}
}