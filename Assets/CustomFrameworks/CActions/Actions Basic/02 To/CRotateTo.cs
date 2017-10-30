using UnityEngine;

namespace CAnimationFramework.CAction
{
	public class CRotateTo : CAction
	{
		Transform mTargetTransform = null;
		Vector3 mvFinalRot = new Vector3(0,0,0);
		Vector3 mvStartRot = new Vector3(0,0,0);
		Vector3 mvDiffRot = new Vector3(0,0,0);
		Vector3 mvCurRot = new Vector3(0,0,0);
		bool mbLocal = true;
		bool mbReverseAngleCorrection = false;
		
		static public CRotateTo Rotate(Transform pTargetObject, Vector3 pEndRot,float pDuration)
		{
			//Default move local position
			return CRotateTo.Rotate(pTargetObject,pEndRot,pDuration,true,false);
		}
		
		static public CRotateTo Rotate(Transform pTargetObject, Vector3 pEndRot,float pDuration,bool pbLocal,bool reverseAngleCorrection = false)
		{
			CRotateTo tRotate = new CRotateTo();
			tRotate.actionWith(pTargetObject,pEndRot,pDuration,pbLocal,reverseAngleCorrection);
			return tRotate;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pEndRot,float pDuration,bool pbLocal,bool reverseAngleCorrection = false)
		{
			mbReverseAngleCorrection = reverseAngleCorrection;
			mvFinalRot.Set(pEndRot.x,pEndRot.y,pEndRot.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
		
		override public void runAction()
		{
			mvStartRot = mTargetTransform.eulerAngles;
			if(mbLocal)
				mvStartRot = mTargetTransform.localEulerAngles;

			if(mbReverseAngleCorrection)
			{
				if(mvStartRot.x > 180)
					mvFinalRot.x = mvFinalRot.x+360;
				if(mvStartRot.y > 180)
					mvFinalRot.y = mvFinalRot.y+360;
				if(mvStartRot.z > 180)
					mvFinalRot.y = mvFinalRot.z+360;
			}
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
			CRotateTo tRotate = CRotateTo.Rotate(mTargetTransform,mvFinalRot * -1, _Duration);
			return tRotate;
		}
	}
}