using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CJumpTo : CAction {

		// Use this for initialization
		Transform mTargetTransform = null;
		Vector3 mvDestinationPos = new Vector3(0,0,0);
		Vector3 mvStartPos = new Vector3(0,0,0);
		Vector3 mvDiffPos = new Vector3(0,0,0);
		Vector3 mvCurPos = new Vector3(0,0,0);
		float mfHeight = 0;
		int miJumps = 1;
		bool mbLocal = true;
		
		static public CJumpTo Jump(Transform pTargetObject, Vector3 pEndPos,float pDuration,float pfHt)
		{
			//Default move local position
			return CJumpTo.Jump(pTargetObject,pEndPos,pDuration,pfHt,1,true);
		}
		
		static public CJumpTo Jump(Transform pTargetObject, Vector3 pEndPos,float pDuration,float pfHt,int piJumps,bool pbLocal)
		{
			CJumpTo tJump = new CJumpTo();
			tJump.actionWith(pTargetObject,pEndPos,pDuration,pfHt,piJumps,pbLocal);
			return tJump;
		}
		
		protected void actionWith(Transform pTargetObject, Vector3 pEndPos,float pDuration,float pfHt,int piJumps,bool pbLocal)
		{
			mfHeight = pfHt;
			miJumps = piJumps;
			mvDestinationPos.Set(pEndPos.x,pEndPos.y,pEndPos.z);
			mTargetTransform = pTargetObject;
			_Duration = pDuration;
			mbLocal = pbLocal;
		}
		
		override public void runAction()
		{
			if(mbLocal)
				mvStartPos = mTargetTransform.localPosition;
			else
				mvStartPos = mTargetTransform.position;
			
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				mfRate = 1.0f/_Duration;		
				mvDiffPos.Set(mvDestinationPos.x - mvStartPos.x,mvDestinationPos.y - mvStartPos.y,mvDestinationPos.z - mvStartPos.z);
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

			float tJumpMagnitude = Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad *  pfCompletion * (miJumps * 180)))  * mfHeight * (1 - pfCompletion);
			mvCurPos.Set(mvDiffPos.x*pfCompletion,mvDiffPos.y*pfCompletion,mvDiffPos.z*pfCompletion);
			mvCurPos.Set(mvStartPos.x+mvCurPos.x,mvStartPos.y+mvCurPos.y,mvStartPos.z+mvCurPos.z);
			mvCurPos.y += tJumpMagnitude;

			if(mbLocal)
				mTargetTransform.localPosition = mvCurPos;
			else
				mTargetTransform.position = mvCurPos;
		}
		
		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}

		override public CAction reverseAction()
		{
			CJumpTo tJump = CJumpTo.Jump(mTargetTransform,mvDestinationPos*-1,_Duration,mfHeight,miJumps,mbLocal);
			return tJump;
		}
	}
}