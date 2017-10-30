using UnityEngine;

namespace CAnimationFramework.CAction
{
	public class CSetScale : CAction 
	{
		Transform mTransform;
		Vector3 mScale;
		bool mbLocal = true;

		//CONVENIENT METHODS
		static public CSetScale SetScaleWith(Transform transform,Vector3 scale, bool local = true)
		{
			CSetScale tSetPos = new CSetScale();
			tSetPos.actionWith(transform,scale,local);
			return tSetPos;
		}

		//SETTING THE CALL BACK
		protected void actionWith(Transform transform,Vector3 scale, bool local)
		{
			mTransform = transform;
			mScale = scale;
			mbLocal = local;
		}

		//RUN THE ACTION
		override public void runAction()
		{
			if(mbLocal)
				mTransform.localScale = mScale;
			else
				mTransform.localScale = mScale;

			OnActionComplete();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			if(mbLocal)
				mTransform.localScale = mScale;
			else
				mTransform.localScale = mScale;
			OnActionComplete();
		}
	}
}