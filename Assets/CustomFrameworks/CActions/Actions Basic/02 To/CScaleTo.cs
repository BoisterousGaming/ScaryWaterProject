using UnityEngine;

namespace CAnimationFramework.CAction
{
	public class CScaleTo : CAction 
	{
		Transform mTargetTransform = null;
		Vector3 mvStartScale = new Vector3(0,0,0);
		Vector3 mvFinalScale = new Vector3(0,0,0);
		Vector3 mvDiffScale = new Vector3(0,0,0);
		Vector3 mvCurScale = new Vector3(0,0,0);
		bool mbLocal = true;

		static public CScaleTo Scale(Transform pTargetObject, Vector3 pFinalScale,float pDuration)
		{
			//Default move local scale
			return CScaleTo.Scale(pTargetObject,pFinalScale,pDuration,true);
		}
		
		static public CScaleTo Scale(Transform pTargetObject, Vector3 pFinalScale,float pDuration,bool pbLocal)
		{
			CScaleTo tScale = new CScaleTo();
			tScale.actionWith(pTargetObject,pFinalScale,pDuration,pbLocal);
			return tScale;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pFinalScale,float pDuration,bool pbLocal)
		{
			mvFinalScale.Set(pFinalScale.x,pFinalScale.y,pFinalScale.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
			
		override public void runAction()
		{				
			mvStartScale = mTargetTransform.localScale;

			if(mbLocal)
				mvStartScale = mTargetTransform.localScale;
			
			mvDiffScale.Set(mvFinalScale.x - mvStartScale.x,mvFinalScale.y - mvStartScale.y,mvFinalScale.z - mvStartScale.z);
			
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

			mvCurScale.Set(mvDiffScale.x*pfCompletion,mvDiffScale.y*pfCompletion,mvDiffScale.z*pfCompletion);
			mvCurScale.Set(mvStartScale.x+mvCurScale.x,mvStartScale.y+mvCurScale.y,mvStartScale.z+mvCurScale.z);
			if(mbLocal)
				mTargetTransform.localScale = mvCurScale;
			else
				mTargetTransform.localScale = mvCurScale;
		}
		
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CScaleTo tScale = CScaleTo.Scale(mTargetTransform,mvFinalScale * -1, _Duration);
			return tScale;
		}
	}
}