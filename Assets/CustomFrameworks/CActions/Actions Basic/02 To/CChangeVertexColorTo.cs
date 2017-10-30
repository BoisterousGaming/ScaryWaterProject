using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CChangeVertexColorTo : CAction
	{
		Mesh mTargetMesh = null;
		Color[] mVertexColors = new Color[0];
		Color mStartColor = new Color(0,0,0,0);
		Color mEndColor = new Color(0,0,0,0);
		Color mCurColor = new Color(0,0,0,0);
		Vector4 mColorDiff = new Color(0,0,0,0);

		static public CChangeVertexColorTo ChangeColor(GameObject pGameObject, Color pEndColor,float pDuration)
		{
			CChangeVertexColorTo tChange = new CChangeVertexColorTo();
			tChange.actionWith(pGameObject,pEndColor,pDuration);
			return tChange;
		}

		protected void actionWith(GameObject pGameObject, Color pEndColor,float pDuration)
		{
			mTargetMesh = pGameObject.GetComponent<MeshFilter>().mesh;
			mVertexColors = mTargetMesh.colors;
			mEndColor = pEndColor;
			_Duration = pDuration;
		}
		
		override public void runAction()
		{
			//Start the action with targets current color
			mStartColor = mTargetMesh.colors[0];
			mVertexColors = new Color[mTargetMesh.colors.Length];
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
			if(mTargetMesh == null)
			{
				cancelAction();
				return;
			}

			mCurColor.r = mStartColor.r + mColorDiff.x * pfCompletion;
			mCurColor.g = mStartColor.g + mColorDiff.y * pfCompletion;
			mCurColor.b = mStartColor.b + mColorDiff.z * pfCompletion;
			mCurColor.a = mStartColor.a + mColorDiff.w * pfCompletion;
			for(int i = 0; i < mVertexColors.Length;i++)
			{
				mVertexColors[i] = mCurColor;
			}
			mTargetMesh.colors = mVertexColors;
		}
		
		//Send Skip message to inner actions
		override public void skipAction()
		{
			setCompletion(1);
			OnActionComplete();
		}
	}
}