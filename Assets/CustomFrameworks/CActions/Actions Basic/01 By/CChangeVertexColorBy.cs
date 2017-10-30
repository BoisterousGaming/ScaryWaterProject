using UnityEngine;
namespace CAnimationFramework.CAction
{
	public class CChangeVertexColorBy : CAction
	{
		GameObject mObject;
		Mesh mTargetMesh = null;
		Color[] mVertexColors = new Color[0];
		Color mCurColorDiff = new Color(0,0,0,0);
		Color mColorDiff = new Color(0,0,0,0);
		float mfPreviousCompletion = 0.0f;

		static public CChangeVertexColorBy ChangeColor(GameObject pGameObject, Color pDiffColor,float pDuration)
		{
			CChangeVertexColorBy tChange = new CChangeVertexColorBy();
			tChange.actionWith(pGameObject,pDiffColor,pDuration);
			return tChange;
		}
		
		protected void actionWith(GameObject pGameObject, Color pDiffColor,float pDuration)
		{
			mObject = pGameObject;
			mTargetMesh = pGameObject.GetComponent<MeshFilter>().mesh;
			mVertexColors = mTargetMesh.colors;
			mColorDiff = pDiffColor;
			_Duration = pDuration;
		}
		
		override public void runAction()
		{
			//Start the action with targets current color
			mVertexColors = mTargetMesh.colors;
			if(_Duration <= 0)
			{
				skipAction();
			}
			else
			{
				meState = ActionState.ActionRun;
				mfCompletion = 0.0f;
				mfPreviousCompletion = 0.0f;
				mfRate = 1.0f/_Duration;
			}
		}	
		
		override public void setCompletion(float pfCompletion)
		{
			if(mObject == null)
			{
				cancelAction();
				return;
			}

			float tfCompletionDiff = pfCompletion - mfPreviousCompletion;
			mfPreviousCompletion = pfCompletion;
			mCurColorDiff = mColorDiff*tfCompletionDiff;
			for(int i = 0; i < mVertexColors.Length;i++)
			{
				mVertexColors[i] = mVertexColors[i] + mCurColorDiff;
				mVertexColors[i].r -= (int)mVertexColors[i].r;
				mVertexColors[i].g -= (int)mVertexColors[i].g;
				mVertexColors[i].b -= (int)mVertexColors[i].b;
				mVertexColors[i].a -= (int)mVertexColors[i].a;
				if(mVertexColors[i].r < 0)
					mVertexColors[i].r *= -1;
				if(mVertexColors[i].g < 0)
					mVertexColors[i].g *= -1;
				if(mVertexColors[i].b < 0)
					mVertexColors[i].b *= -1;
				if(mVertexColors[i].a < 0)
					mVertexColors[i].a *= -1;
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