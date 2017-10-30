using UnityEngine;
using System.Collections;

namespace CAnimationFramework.CAction
{
	public class CChangeShaderParamTo : CAction
	{
		Material mTargetMat = null;
		string mParameter = "CutOff";
		float mfTargetValue = 1;
		float mfValueDiff = 0;
		float mfStartValue = 0;
		static public CChangeShaderParamTo ChangeParameterOf(Material pTargetMaterial,string pParameter, float pfTo,float pDuration)
		{
			CChangeShaderParamTo tChange = new CChangeShaderParamTo();
			tChange.actionWith(pTargetMaterial,pParameter,pfTo,pDuration);
			return tChange;
		}
		
		protected void actionWith(Material pTargetMaterial,string pParameter, float pfTo,float pDuration)
		{
			mTargetMat = pTargetMaterial;
			mParameter = pParameter;
			mfTargetValue = pfTo;
			_Duration = pDuration;
		}
		
		override public void runAction()
		{
			mfStartValue = mTargetMat.GetFloat(mParameter);
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				meState = ActionState.ActionRun;
				mfCompletion = 0.0f;
				mfRate = 1.0f/_Duration;
				mfValueDiff = mfTargetValue - mfStartValue;
			}
		}	
		
		override public void setCompletion(float pfCompletion)
		{
			if(mTargetMat == null)
			{
				cancelAction();
				return;
			}
			mTargetMat.SetFloat(mParameter,mfStartValue + mfValueDiff * pfCompletion);
		}
		
		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
	}
}