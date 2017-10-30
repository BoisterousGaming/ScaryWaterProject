using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CScaleBy : CAction 
	{
		Transform mTargetTransform = null;
		Vector3 mvScaleDiff = new Vector3(0,0,0);
		Vector3 mvCurScaleDiff = new Vector3(0,0,0);
		float mfPreviousCompletion = 0.0f;
		bool mbLocal = true;
		
		static public CScaleBy Scale(Transform pTargetObject, Vector3 pScaleDiff,float pDuration)
		{
			//Default move local position
			return CScaleBy.Scale(pTargetObject,pScaleDiff,pDuration,true);
		}
		
		static public CScaleBy Scale(Transform pTargetObject, Vector3 pScaleDiff,float pDuration,bool pbLocal)
		{
			CScaleBy tScale = new CScaleBy();
			tScale.actionWith(pTargetObject,pScaleDiff,pDuration,pbLocal);
			return tScale;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pScaleDiff,float pDuration,bool pbLocal)
		{
			mvScaleDiff.Set(pScaleDiff.x,pScaleDiff.y,pScaleDiff.z);
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
			
			mvCurScaleDiff.Set(mvScaleDiff.x*tfCompletionDiff,mvScaleDiff.y*tfCompletionDiff,mvScaleDiff.z*tfCompletionDiff);
			if(mbLocal)
				mTargetTransform.localScale = mTargetTransform.localScale + mvCurScaleDiff;
			else
				mTargetTransform.localScale = mTargetTransform.localScale + mvCurScaleDiff;
		}
		
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
		
		override public CAction reverseAction()
		{
			CScaleBy tScale = CScaleBy.Scale(mTargetTransform,mvScaleDiff * -1, _Duration);
			return tScale;
		}
	}
}
