using UnityEngine;
using System;
using System.Collections;
using CAnimationFramework.CAction;

public class MenuItemExitAnim : MonoBehaviour 
{
	public AnimationStateData _StartState;
	public AnimationStateData _EndState;
	public AnimationCurve _EaseCurve = AnimationCurve.Linear(0,0,1,1);
	public float _StartDelay = 0.0f;
	public float _Duration = 1.0f;

	GUIItem mGUIItem;
	Action mCallBack;
	CActionSet mActionSet = null;
	Transform mTransform;
	RectTransform mRectTransform;

	public void AnimationWithCallBack(Action pCallBack)
	{
		GUIStaticAnimation[] staticAnimations = GetComponents<GUIStaticAnimation>();
		for(int i = 0 ; i < staticAnimations.Length;i++)
			staticAnimations[i].StopAnimating();

		mGUIItem = GetComponent<GUIItem>();
		StopAllActions();
		mCallBack = pCallBack;
		mTransform = transform;
		mRectTransform = GetComponent<RectTransform> ();

		if(_StartDelay > 0)
		{
			CDelayTime tDelay = CDelayTime.Delay(_StartDelay);
			mActionSet = CActionSet.CreateWithMainAction(tDelay,BeginActualAnimation,eActionUpdateMode.Update,eActionTimeMode.FPSIndependant);
			CSharedActionsHandler.Instance.RunActionSet(mActionSet);
		}
		else
		{
			BeginActualAnimation();
		}
	}

	public void BeginActualAnimation()
	{
		//STOP ALL ACTIONS FIRST
		if(_Duration != 0)
		{
			CSpawnAction spw = CSpawnAction.SpawnWithActions();

			if(Vector3.Magnitude(_EndState._Position - _StartState._Position) > 0){
				CRTMoveTo tMove = CRTMoveTo.Move(mRectTransform,_EndState._Position,_Duration);
				spw.AddAction(tMove);
			}

			if(Vector3.Magnitude(_EndState._Scale - _StartState._Scale) > 0){
				CScaleTo tScale = CScaleTo.Scale(mTransform,_EndState._Scale,_Duration);
				spw.AddAction(tScale);
			}

			if(Vector3.Magnitude(_EndState._Rotation - _StartState._Rotation) > 0){
				CRotateTo tRotate = CRotateTo.Rotate(mTransform,_EndState._Rotation,_Duration);
				spw.AddAction(tRotate);
			}

			if(mGUIItem.HasColorParameter()){
				CChangeGUIItemColorTo tColorChange = CChangeGUIItemColorTo.ChangeColor(mGUIItem,_EndState._Color,_Duration);
				spw.AddAction(tColorChange);
			}


			CEaseWRTCurve tEase = CEaseWRTCurve.EaseWith(spw,_EaseCurve);
			CCallFunc tCall = CCallFunc.CallBackWith(CompletedAllAnimations);
			CSequence tSeq = CSequence.SequenceWithActions(tEase,tCall);

			mActionSet = CActionSet.CreateWithMainAction(tSeq,null,eActionUpdateMode.Update,eActionTimeMode.FPSIndependant);
			CSharedActionsHandler.Instance.RunActionSet(mActionSet);
		}
		else
		{
			mTransform.localPosition = _EndState._Position;
			mTransform.localScale = _EndState._Scale;
			mTransform.localEulerAngles = _EndState._Rotation;
			mGUIItem.SetColor(_EndState._Color);
			CompletedAllAnimations();
		}
	}

	public void CompletedAllAnimations()
	{
		StopAllActions();
		if(mCallBack != null)
			mCallBack();
	}

	public void StopAllActions()
	{
		if(mActionSet != null)
		{
			CSharedActionsHandler.Instance.StopActionSet(mActionSet);
			mActionSet = null;
		}
	}

	void OnDestroy()
	{
		StopAllActions ();
	}
}