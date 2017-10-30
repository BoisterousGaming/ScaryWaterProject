using UnityEngine;

namespace CAnimationFramework.CAction
{
	public class CSetPosition : CAction 
	{
		Transform mTransform;
		Vector3 mPosition;
		bool mbLocal = true;

		//CONVENIENT METHODS
		static public CSetPosition SetPositionWith(Transform transform,Vector3 position, bool local = true)
		{
			CSetPosition tSetPos = new CSetPosition();
			tSetPos.actionWith(transform,position,local);
			return tSetPos;
		}

		//SETTING THE CALL BACK
		protected void actionWith(Transform transform,Vector3 position, bool local)
		{
			mTransform = transform;
			mPosition = position;
			mbLocal = local;
		}
		
		//RUN THE ACTION
		override public void runAction()
		{
			if(mbLocal)
				mTransform.localPosition = mPosition;
			else
				mTransform.position = mPosition;
			
			OnActionComplete();
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			if(mbLocal)
				mTransform.localPosition = mPosition;
			else
				mTransform.position = mPosition;
			OnActionComplete();
		}
	}
}
