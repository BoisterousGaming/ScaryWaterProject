using UnityEngine;
using UnityEngine.UI;
namespace CAnimationFramework.CAction
{
	public class CChangeImageColorTo : CAction
	{
		Image _Target = null;
		Color mStartColor = new Color(0,0,0,0);
		Color mEndColor = new Color(0,0,0,0);
		Color mCurColor = new Color(0,0,0,0);
		Vector4 mColorDiff = new Color(0,0,0,0);

		static public CChangeImageColorTo ChangeColor(Image spriteRenderer, Color endColor,float duration)
		{
			CChangeImageColorTo tChange = new CChangeImageColorTo();
			tChange.actionWith(spriteRenderer,endColor,duration);
			return tChange;
		}

		protected void actionWith(Image spriteRenderer, Color endColor,float duration)
		{
			_Target = spriteRenderer;
			mEndColor = endColor;
			_Duration = duration;
		}

		override public void runAction()
		{
			//Start the action with targets current color
			mStartColor = _Target.color;
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
			if(_Target == null)
			{
				cancelAction();
				return;
			}

			mCurColor.r = mStartColor.r + mColorDiff.x * pfCompletion;
			mCurColor.g = mStartColor.g + mColorDiff.y * pfCompletion;
			mCurColor.b = mStartColor.b + mColorDiff.z * pfCompletion;
			mCurColor.a = mStartColor.a + mColorDiff.w * pfCompletion;
			_Target.color = mCurColor;
		}

		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
	}
}