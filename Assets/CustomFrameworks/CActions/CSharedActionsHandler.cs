using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CAnimationFramework.CAction
{
	public class CSharedActionsHandler : MonoBehaviour {

		static CSharedActionsHandler mInstance;
		List<CActionSet> mActionSetList = new List<CActionSet>();
		public static float _AnimationDt = 0.02f;

		public static CSharedActionsHandler Instance
		{
			get
			{
				if(mInstance == null)
				{
					GameObject tObj = new GameObject();
					tObj.name = "CSharedActionsHandler";
					mInstance = tObj.AddComponent<CSharedActionsHandler>();
					DontDestroyOnLoad(tObj);
				}
				return mInstance;
			}
		}

		public void RunActionSet(CActionSet pSet)
		{
			if(!mActionSetList.Contains(pSet))
			{
				mActionSetList.Add(pSet);
				pSet.Play();
				pSet.SetAnimationRate(_AnimationDt);
			}
			else
			{
				Debug.LogWarning("Trying to add the action set again");
			}
		}

		public void CompletedActionSet(CActionSet pSet)
		{
			mActionSetList.Remove(pSet);
		}

		public void StopActionSet(CActionSet pSet)
		{
			if(mActionSetList.Contains(pSet))
				mActionSetList.Remove(pSet);
		}

		public void TriggerEvent(string pEvent)
		{
			for(int i = 0; i < mActionSetList.Count;i++)
			{
				mActionSetList[i].ReceivedEvent(pEvent);
			}
		}

		// Update is called once per frame
		void Update () {
			for(int i = 0; i < mActionSetList.Count;i++)
			{
				mActionSetList[i].Update();
			}
		}


		// Update is called once per frame
		void FixedUpdate () {
			for(int i = 0; i < mActionSetList.Count;i++)
			{
				mActionSetList[i].FixedUpdate();
			}
		}

		// Update is called once per frame
		void LateUpdate () {
			for(int i = 0; i < mActionSetList.Count;i++)
			{
				mActionSetList[i].LateUpdate();
			}
		}
	}
}
