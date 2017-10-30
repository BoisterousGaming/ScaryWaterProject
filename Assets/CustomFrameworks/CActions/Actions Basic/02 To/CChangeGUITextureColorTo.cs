using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CChangeGUITextureColorTo : CAction
	{
		GUITexture _TargetTexture = null;
		Color mStartColor = new Color(0,0,0,0);
		Color mEndColor = new Color(0,0,0,0);
		Color mCurColor = new Color(0,0,0,0);
		Vector4 mColorDiff = new Color(0,0,0,0);
		
		static public CChangeGUITextureColorTo ChangeColor(GUITexture pGUITexture, Color pEndColor,float pDuration)
		{
			CChangeGUITextureColorTo tChange = new CChangeGUITextureColorTo();
			tChange.actionWith(pGUITexture,pEndColor,pDuration);
			return tChange;
		}
		
		protected void actionWith(GUITexture pGUITexture, Color pEndColor,float pDuration)
		{
			_TargetTexture = pGUITexture;
			mEndColor = pEndColor;
			_Duration = pDuration;
		}
		
		override public void runAction()
		{
			//Start the action with targets current color
			mStartColor = _TargetTexture.color;
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				meState = ActionState.ActionRun;
				mfCompletion = 0.0f;
				mfRate = 1.0f/_Duration;
				mColorDiff.Set(mEndColor.r - mStartColor.r,mEndColor.g - mStartColor.g,mEndColor.b - mStartColor.b,mEndColor.a - mStartColor.a);
			}
		}	
		
		override public void setCompletion(float pfCompletion)
		{
			if(_TargetTexture == null)
			{
				cancelAction();
				return;
			}

			mCurColor.r = mStartColor.r + mColorDiff.x * pfCompletion;
			mCurColor.g = mStartColor.g + mColorDiff.y * pfCompletion;
			mCurColor.b = mStartColor.b + mColorDiff.z * pfCompletion;
			mCurColor.a = mStartColor.a + mColorDiff.w * pfCompletion;
			_TargetTexture.color = mCurColor;
		}
		
		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
	}
}