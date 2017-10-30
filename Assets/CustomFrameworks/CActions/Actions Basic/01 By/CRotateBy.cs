using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CRotateBy : CAction 
	{
		Transform mTargetTransform = null;
		Vector3 mvRotDiff = new Vector3(0,0,0);
		Vector3 mvCurRotDiff = new Vector3(0,0,0);
		float mfPreviousCompletion = 0.0f;
		bool mbLocal = true;
		
		static public CRotateBy Rotate(Transform pTargetObject, Vector3 pRotDiff,float pDuration)
		{
			//Default move local position
			return CRotateBy.Rotate(pTargetObject,pRotDiff,pDuration,true);
		}
		
		static public CRotateBy Rotate(Transform pTargetObject, Vector3 pRotDiff,float pDuration,bool pbLocal)
		{
			CRotateBy tRotate = new CRotateBy();
			tRotate.actionWith(pTargetObject,pRotDiff,pDuration,pbLocal);
			return tRotate;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pRotDiff,float pDuration,bool pbLocal)
		{
			mvRotDiff.Set(pRotDiff.x,pRotDiff.y,pRotDiff.z);
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
			
			mvCurRotDiff.Set(mvRotDiff.x*tfCompletionDiff,mvRotDiff.y*tfCompletionDiff,mvRotDiff.z*tfCompletionDiff);
			if(mbLocal)
				mTargetTransform.Rotate(mvCurRotDiff,Space.Self);
			else
				mTargetTransform.Rotate(mvCurRotDiff,Space.World);
		}
		
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CRotateBy tRotate = CRotateBy.Rotate(mTargetTransform,mvRotDiff * -1, _Duration);
			return tRotate;
		}
	}
}
