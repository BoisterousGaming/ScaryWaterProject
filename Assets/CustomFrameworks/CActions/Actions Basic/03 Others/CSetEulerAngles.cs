using UnityEngine;

namespace CAnimationFramework.CAction
{
	public class CSetEulerAngles : CAction 
	{
		Transform mTransform;
		Vector3 mEulerAngles;
		bool mbLocal = true;

		//CONVENIENT METHODS
		static public CSetEulerAngles SetEulerAnglesWith(Transform transform,Vector3 eulerAngles, bool local = true)
		{
			CSetEulerAngles tSetPos = new CSetEulerAngles();
			tSetPos.actionWith(transform,eulerAngles,local);
			return tSetPos;
		}

		//SETTING THE CALL BACK
		protected void actionWith(Transform transform,Vector3 eulerAngles, bool local)
		{
			mTransform = transform;
			mEulerAngles = eulerAngles;
			mbLocal = local;
		}
		
		//RUN THE ACTION
		override public void runAction()
		{
			if(mbLocal)
				mTransform.localEulerAngles = mEulerAngles;
			else
				mTransform.eulerAngles = mEulerAngles;
			
			OnActionComplete();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			if(mbLocal)
				mTransform.localEulerAngles = mEulerAngles;
			else
				mTransform.eulerAngles = mEulerAngles;
			OnActionComplete();
		}
	}
}
