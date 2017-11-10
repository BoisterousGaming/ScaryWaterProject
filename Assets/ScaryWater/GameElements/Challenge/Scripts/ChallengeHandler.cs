using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor;
//#endif

//public enum eChallengeType
//{
//    None = -1,
//    CoverDistance,
//}

public class ChallengeHandler : MonoBehaviour 
{
    //public eChallengeType _eChallengeType = eChallengeType.None;
    //public int _iHowManySetsToCover = 6;
}

//// Runtime code here
//#if UNITY_EDITOR
//// Editor specific code here
//[CustomEditor(typeof(MiniGameHandler))]
//public class ChallengeEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        ChallengeHandler mChallengeHandlerScr = (ChallengeHandler)target;

//        mChallengeHandlerScr._eChallengeType = (eChallengeType)EditorGUILayout.EnumPopup("Challenge Type", mChallengeHandlerScr._eChallengeType);

//        switch (mChallengeHandlerScr._eChallengeType)
//        {
//            case eChallengeType.None:
//                break;

//            case eChallengeType.CoverDistance:
//                mChallengeHandlerScr._iHowManySetsToCover = EditorGUILayout.IntField("Sets To Cover", mChallengeHandlerScr._iHowManySetsToCover);
//                break;
//        }
//    }
//}
//#endif
//// Runtime code here
