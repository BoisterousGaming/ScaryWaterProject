using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public enum eBZCEManipulationModes
{
    Free,
    SelectAndTransform
}

public enum eBZCENewWaypointMode
{
    SceneCamera,
    LastWaypoint,
    WaypointIndex,
    WorldCenter
}

[CustomEditor(typeof(BZCObjectPath))]
public class BZCObjectPathInspector : Editor
{
    BZCObjectPath mBZCObjectPathScr;
    ReorderableList mRListOfPoint;

    //Editor variables
    bool mbVisualFoldout;
    bool mbManipulationFoldout;
    bool mbShowRawValues;
    float mfTime;
    eBZCEManipulationModes meCameraTranslateMode;
    eBZCEManipulationModes meCameraRotationMode;
    eBZCEManipulationModes meHandlePositionMode;
    eBZCENewWaypointMode meWaypointMode;
    int miWaypointIndex = 1;
    eBZCECurveType meAllCurveType = eBZCECurveType.Custom;
    AnimationCurve mAllAnimationCurve = AnimationCurve.EaseInOut(0,0,1,1);

    //GUIContents
    GUIContent addPointContent = new GUIContent("Add Point", "Adds a waypoint at the scene view camera's position/rotation");
    GUIContent testButtonContent = new GUIContent("Test", "Only available in play mode");
    GUIContent pauseButtonContent = new GUIContent("Pause", "Paused Camera at current Position");
    GUIContent continueButtonContent = new GUIContent("Continue", "Continues Path at current position");
    GUIContent stopButtonContent = new GUIContent("Stop", "Stops the playback");
    GUIContent deletePointContent = new GUIContent("X", "Deletes this waypoint");
    GUIContent gotoPointContent = new GUIContent("Goto", "Teleports the scene camera to the specified waypoint");
    GUIContent relocateContent = new GUIContent("Relocate", "Relocates the specified camera to the current view camera's position/rotation");
    GUIContent alwaysShowContent = new GUIContent("Always show", "When true, shows the curve even when the GameObject is not selected - \"Inactive cath color\" will be used as path color instead");
    GUIContent chainedContent = new GUIContent("o───o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    GUIContent unchainedContent = new GUIContent("o─x─o", "Toggles if the handles of the specified waypoint should be chained (mirrored) or not");
    GUIContent replaceAllPositionContent = new GUIContent("Replace all position lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint position lerp types with the specified values");
    GUIContent replaceAllRotationContent = new GUIContent("Replace all rotation lerps", "Replaces curve types (and curves when set to \"Custom\") of all the waypoint rotation lerp types with the specified values");

    //Serialized Properties
    SerializedObject serializedObjectTarget;
    SerializedProperty useMainCameraProperty;
    SerializedProperty selectedCameraProperty;
    SerializedProperty lookAtTargetProperty;
    SerializedProperty lookAtTargetTransformProperty;
    SerializedProperty playOnAwakeProperty;
    SerializedProperty playOnAwakeTimeProperty;
    SerializedProperty visualPathProperty;
    SerializedProperty visualInactivePathProperty;
    SerializedProperty visualFrustumProperty;
    SerializedProperty visualHandleProperty;
    SerializedProperty loopedProperty;
    SerializedProperty alwaysShowProperty;
    SerializedProperty afterLoopProperty;
    SerializedProperty pickupPointIndexProperty;
    SerializedProperty dropPointIndexProperty;
    SerializedProperty pickupPointProperty;
    SerializedProperty dropPointProperty;

    int miSelectedIndex = -1;
    float mfCurrentTime;
    float mfPreviousTime;
    bool mbHasScrollBar = false;

    void OnEnable()
    {
        EditorApplication.update += Update;
        
        mBZCObjectPathScr = (BZCObjectPath) target;
        if (mBZCObjectPathScr == null) return;

        SetupEditorVariables();
        GetVariableProperties();
        SetupReorderableList();
    }

    void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    void Update()
    {
        if (mBZCObjectPathScr == null) 
            return;
        
        mfCurrentTime = mBZCObjectPathScr.GetCurrentWayPoint() + mBZCObjectPathScr.GetCurrentTimeInWaypoint();
        if (Math.Abs(mfCurrentTime - mfPreviousTime) > 0.0001f)
        {
            Repaint();
            mfPreviousTime = mfCurrentTime;
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObjectTarget.Update();
        DrawPlaybackWindow();
        Rect scale = GUILayoutUtility.GetLastRect();
        mbHasScrollBar = (Screen.width - scale.width <= 12);
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        GUILayout.Space(5);
        DrawBasicSettings();
        GUILayout.Space(5);
        GUILayout.Box("", GUILayout.Width(Screen.width-20), GUILayout.Height(3));
        DrawVisualDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        DrawManipulationDropdown();
        GUILayout.Box("", GUILayout.Width(Screen.width - 20), GUILayout.Height(3));
        GUILayout.Space(10);
        DrawWaypointList();
        GUILayout.Space(10);
        DrawRawValues();
        serializedObjectTarget.ApplyModifiedProperties();
    }

    void OnSceneGUI()
    {
        if (mBZCObjectPathScr._ListOfPoints.Count >= 2)
        {
            for (int i = 0; i < mBZCObjectPathScr._ListOfPoints.Count; i++)
            {
                DrawHandles(i);
                Handles.color = Color.white;
            }
        }
    }

    void SelectIndex(int index)
    {
        miSelectedIndex = index;
        mRListOfPoint.index = index;
        Repaint();
    }

    void SetupEditorVariables()
    {
        meCameraTranslateMode = (eBZCEManipulationModes)PlayerPrefs.GetInt("BZC_CameraTranslateMode", 1);
        meCameraRotationMode = (eBZCEManipulationModes)PlayerPrefs.GetInt("BZC_CameraRotationMode", 1);
        meHandlePositionMode = (eBZCEManipulationModes)PlayerPrefs.GetInt("BZC_HandlePositionMode", 0);
        meWaypointMode = (eBZCENewWaypointMode) PlayerPrefs.GetInt("BZC_WaypointMode", 0);
        mfTime = PlayerPrefs.GetFloat("BZC_Time", 10);
    }

    void GetVariableProperties()
    {
        serializedObjectTarget = new SerializedObject(mBZCObjectPathScr);
        useMainCameraProperty = serializedObjectTarget.FindProperty("_bUseMainCamera");
        selectedCameraProperty = serializedObjectTarget.FindProperty("_SelectedObject");
        lookAtTargetProperty = serializedObjectTarget.FindProperty("_bLookAtTarget");
        lookAtTargetTransformProperty = serializedObjectTarget.FindProperty("_tLookAtTarget");
        visualPathProperty = serializedObjectTarget.FindProperty("BZCVisualScr._PathColor");
        visualInactivePathProperty = serializedObjectTarget.FindProperty("BZCVisualScr._InactivePathColor");
        visualFrustumProperty = serializedObjectTarget.FindProperty("BZCVisualScr._FrustrumColor");
        visualHandleProperty = serializedObjectTarget.FindProperty("BZCVisualScr._HandleColor");
        loopedProperty = serializedObjectTarget.FindProperty("_bLooped");
        alwaysShowProperty = serializedObjectTarget.FindProperty("_bAlwaysShow");
        afterLoopProperty = serializedObjectTarget.FindProperty("_eAfterLoop");
        playOnAwakeProperty = serializedObjectTarget.FindProperty("_bPlayOnAwake");
        playOnAwakeTimeProperty = serializedObjectTarget.FindProperty("_fPlayOnAwakeTime");
        pickupPointIndexProperty = serializedObjectTarget.FindProperty("_iPickupPointIndex");
        dropPointIndexProperty = serializedObjectTarget.FindProperty("_iDropPointIndex");
        pickupPointProperty = serializedObjectTarget.FindProperty("_bPickupPoint");
        dropPointProperty = serializedObjectTarget.FindProperty("_bDropPoint");
    }

    void SetupReorderableList()
    {
        mRListOfPoint = new ReorderableList(serializedObject, serializedObject.FindProperty("_ListOfPoints"), true, true, false, false);

        mRListOfPoint.elementHeight *= 2;

        mRListOfPoint.drawElementCallback = (rect, index, active, focused) =>
        {
            float startRectY = rect.y;
            if (index > mBZCObjectPathScr._ListOfPoints.Count - 1) return;
            rect.height -= 2;
            float fullWidth = rect.width - 16 * (mbHasScrollBar ? 1 : 0);
            rect.width = 40;
            fullWidth -= 40;
            rect.height /= 2;
            GUI.Label(rect, "#" + (index + 1));
            rect.y += rect.height-3;
            rect.x -= 14;
            rect.width += 12;
            if (GUI.Button(rect, mBZCObjectPathScr._ListOfPoints[index]._bChained ? chainedContent : unchainedContent))
            {
                Undo.RecordObject(mBZCObjectPathScr, "Changed chain type");
                mBZCObjectPathScr._ListOfPoints[index]._bChained = !mBZCObjectPathScr._ListOfPoints[index]._bChained;
            }
            rect.x += rect.width+2;
            rect.y = startRectY;
            //Position
            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            eBZCECurveType tempP = (eBZCECurveType)EditorGUI.EnumPopup(rect, mBZCObjectPathScr._ListOfPoints[index]._eCurveTypePosition);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mBZCObjectPathScr, "Changed enum value");
                mBZCObjectPathScr._ListOfPoints[index]._eCurveTypePosition = tempP;
            }
            rect.y += mRListOfPoint.elementHeight / 2 - 4;
            //rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = mBZCObjectPathScr._ListOfPoints[index]._eCurveTypePosition == eBZCECurveType.Custom;
            AnimationCurve tempACP = EditorGUI.CurveField(rect, mBZCObjectPathScr._ListOfPoints[index]._PositionCurve);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mBZCObjectPathScr, "Changed curve");
                mBZCObjectPathScr._ListOfPoints[index]._PositionCurve = tempACP;
            }
            GUI.enabled = true;
            rect.x += rect.width + 2;
            rect.y = startRectY;

            //Rotation

            rect.width = (fullWidth - 22) / 3 - 1;
            EditorGUI.BeginChangeCheck();
            eBZCECurveType temp = (eBZCECurveType)EditorGUI.EnumPopup(rect, mBZCObjectPathScr._ListOfPoints[index]._eCurveTypeRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mBZCObjectPathScr, "Changed enum value");
                mBZCObjectPathScr._ListOfPoints[index]._eCurveTypeRotation = temp;
            }
            rect.y += mRListOfPoint.elementHeight / 2 - 4;
            //rect.height /= 2;
            //rect.x += rect.width + 2;
            EditorGUI.BeginChangeCheck();
            GUI.enabled = mBZCObjectPathScr._ListOfPoints[index]._eCurveTypeRotation == eBZCECurveType.Custom;
            AnimationCurve tempAC = EditorGUI.CurveField(rect, mBZCObjectPathScr._ListOfPoints[index]._RotationCurve);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(mBZCObjectPathScr, "Changed curve");
                mBZCObjectPathScr._ListOfPoints[index]._RotationCurve = tempAC;
            }
            GUI.enabled = true;

            rect.y = startRectY;
            rect.height *= 2;
            rect.x += rect.width + 2;
            rect.width = (fullWidth - 22) / 3;
            rect.height = rect.height / 2 - 1;
            if (GUI.Button(rect, gotoPointContent))
            {
                mRListOfPoint.index = index;
                miSelectedIndex = index;
                SceneView.lastActiveSceneView.pivot = mBZCObjectPathScr._ListOfPoints[mRListOfPoint.index]._vPosition;
                SceneView.lastActiveSceneView.size = 3;
                SceneView.lastActiveSceneView.Repaint();
            }
            rect.y += rect.height + 2;
            if (GUI.Button(rect, relocateContent))
            {
                Undo.RecordObject(mBZCObjectPathScr, "Relocated waypoint");
                mRListOfPoint.index = index;
                miSelectedIndex = index;
                mBZCObjectPathScr._ListOfPoints[mRListOfPoint.index]._vPosition = SceneView.lastActiveSceneView.camera.transform.position;
                mBZCObjectPathScr._ListOfPoints[mRListOfPoint.index]._qRotation = SceneView.lastActiveSceneView.camera.transform.rotation;
                SceneView.lastActiveSceneView.Repaint();
            }
            rect.height = (rect.height + 1) * 2;
            rect.y = startRectY;
            rect.x += rect.width + 2;
            rect.width = 20;

            if (GUI.Button(rect, deletePointContent))
            {
                Undo.RecordObject(mBZCObjectPathScr, "Deleted a waypoint");
                mBZCObjectPathScr._ListOfPoints.Remove(mBZCObjectPathScr._ListOfPoints[index]);
                SceneView.RepaintAll();
            }
        };

        mRListOfPoint.drawHeaderCallback = rect =>
        {
            float fullWidth = rect.width;
            rect.width = 56;
            GUI.Label(rect, "Sum: " + mBZCObjectPathScr._ListOfPoints.Count);
            rect.x += rect.width;
            rect.width = (fullWidth - 78) / 3;
            GUI.Label(rect, "Position Lerp");
            rect.x += rect.width;
            GUI.Label(rect, "Rotation Lerp");
            rect.x += rect.width*2;
            GUI.Label(rect, "Del.");
        };

        mRListOfPoint.onSelectCallback = l =>
        {
            miSelectedIndex = l.index;
            Debug.Log("Selected Index: " + miSelectedIndex);
            SceneView.RepaintAll();
        };
    }

    void DrawPlaybackWindow()
    {
        GUI.enabled = Application.isPlaying;
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(testButtonContent))
        {
            mBZCObjectPathScr.PlayPath(mfTime);
        }

        if (!mBZCObjectPathScr.IsPaused())
        {
            if (Application.isPlaying && !mBZCObjectPathScr.IsPlaying()) GUI.enabled = false;
            if (GUILayout.Button(pauseButtonContent))
            {
                mBZCObjectPathScr.PausePath();
            }
        }
        else if (GUILayout.Button(continueButtonContent))
        {
            mBZCObjectPathScr.ResumePath();
        }

        if (GUILayout.Button(stopButtonContent))
        {
            mBZCObjectPathScr.StopPath();
        }
        GUI.enabled = true;
        EditorGUI.BeginChangeCheck();
        GUILayout.Label("Time (seconds)");
        mfTime = EditorGUILayout.FloatField("", mfTime, GUILayout.MinWidth(5), GUILayout.MaxWidth(50));
        if (EditorGUI.EndChangeCheck())
        {
            mfTime = Mathf.Clamp(mfTime, 0.001f, Mathf.Infinity);
            mBZCObjectPathScr.UpdateTimeInSeconds(mfTime);
            PlayerPrefs.SetFloat("BZC_Time", mfTime);
        }
        GUILayout.EndHorizontal();
        GUI.enabled = Application.isPlaying;
        EditorGUI.BeginChangeCheck();
        mfCurrentTime = EditorGUILayout.Slider(mfCurrentTime, 0, mBZCObjectPathScr._ListOfPoints.Count - ((mBZCObjectPathScr._bLooped) ? 0.01f : 1.01f));
        if (EditorGUI.EndChangeCheck())
        {
            mBZCObjectPathScr.SetCurrentWayPoint(Mathf.FloorToInt(mfCurrentTime));
            mBZCObjectPathScr.SetCurrentTimeInWaypoint(mfCurrentTime % 1);
            mBZCObjectPathScr.RefreshTransform();
        }
        GUI.enabled = false;
        Rect rr = GUILayoutUtility.GetRect(4, 8);
        float endWidth = rr.width - 60;
        rr.y -= 4;
        rr.width = 4;
        int c = mBZCObjectPathScr._ListOfPoints.Count + ((mBZCObjectPathScr._bLooped) ? 1 : 0);
        for (int i = 0; i < c; ++i)
        {
            GUI.Box(rr, "");
            rr.x += endWidth / (c - 1);
        }
        GUILayout.EndVertical();
        GUI.enabled = true;
    }

    void DrawBasicSettings()
    {
        GUILayout.BeginHorizontal();
        useMainCameraProperty.boolValue = GUILayout.Toggle(useMainCameraProperty.boolValue, "Use main camera", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = !useMainCameraProperty.boolValue;
        selectedCameraProperty.objectReferenceValue = (GameObject)EditorGUILayout.ObjectField(selectedCameraProperty.objectReferenceValue, typeof(GameObject), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        lookAtTargetProperty.boolValue = GUILayout.Toggle(lookAtTargetProperty.boolValue, "Look at target", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = lookAtTargetProperty.boolValue;
        lookAtTargetTransformProperty.objectReferenceValue = (Transform)EditorGUILayout.ObjectField(lookAtTargetTransformProperty.objectReferenceValue, typeof(Transform), true);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        loopedProperty.boolValue = GUILayout.Toggle(loopedProperty.boolValue, "Looped", GUILayout.Width(Screen.width/3f));
        GUI.enabled = loopedProperty.boolValue;
        GUILayout.Label("After loop:", GUILayout.Width(Screen.width / 4f));
        afterLoopProperty.enumValueIndex = Convert.ToInt32(EditorGUILayout.EnumPopup((eBZCEAfterLoop)afterLoopProperty.intValue));
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        playOnAwakeProperty.boolValue = GUILayout.Toggle(playOnAwakeProperty.boolValue, "Play on awake", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = playOnAwakeProperty.boolValue;
        GUILayout.Label("Time: ", GUILayout.Width(Screen.width / 4f));
        playOnAwakeTimeProperty.floatValue = EditorGUILayout.FloatField(playOnAwakeTimeProperty.floatValue);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        pickupPointProperty.boolValue = GUILayout.Toggle(pickupPointProperty.boolValue, "Pickup object", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = pickupPointProperty.boolValue;
        GUILayout.Label("Pickup point: ", GUILayout.Width(Screen.width / 4f));
        pickupPointIndexProperty.intValue = EditorGUILayout.IntField(pickupPointIndexProperty.intValue);
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        dropPointProperty.boolValue = GUILayout.Toggle(dropPointProperty.boolValue, "Drop attached object", GUILayout.Width(Screen.width / 3f));
        GUI.enabled = dropPointProperty.boolValue;
        GUILayout.Label("Drop point: ", GUILayout.Width(Screen.width / 4f));
        dropPointIndexProperty.intValue = EditorGUILayout.IntField(dropPointIndexProperty.intValue);
        GUI.enabled = true;
        GUILayout.EndHorizontal();
    }

    void DrawVisualDropdown()
    {
        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        mbVisualFoldout = EditorGUILayout.Foldout(mbVisualFoldout, "Visual");
        alwaysShowProperty.boolValue = GUILayout.Toggle(alwaysShowProperty.boolValue, alwaysShowContent);
        GUILayout.EndHorizontal();
        if (mbVisualFoldout)
        {
            GUILayout.BeginVertical("Box");
            visualPathProperty.colorValue = EditorGUILayout.ColorField("Path color", visualPathProperty.colorValue);
            visualInactivePathProperty.colorValue = EditorGUILayout.ColorField("Inactive path color", visualInactivePathProperty.colorValue);
            visualFrustumProperty.colorValue = EditorGUILayout.ColorField("Frustum color", visualFrustumProperty.colorValue);
            visualHandleProperty.colorValue = EditorGUILayout.ColorField("Handle color", visualHandleProperty.colorValue);
            if (GUILayout.Button("Default colors"))
            {
                Undo.RecordObject(mBZCObjectPathScr, "Reset to default color values");
                mBZCObjectPathScr.BZCVisualScr = new BZCVisual();
            }
            GUILayout.EndVertical();
        }
        if (EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    void DrawManipulationDropdown()
    {
        mbManipulationFoldout = EditorGUILayout.Foldout(mbManipulationFoldout, "Transform manipulation modes");
        EditorGUI.BeginChangeCheck();
        if (mbManipulationFoldout)
        {
            GUILayout.BeginVertical("Box");
            meCameraTranslateMode = (eBZCEManipulationModes)EditorGUILayout.EnumPopup("Waypoint Translation", meCameraTranslateMode);
            meCameraRotationMode = (eBZCEManipulationModes)EditorGUILayout.EnumPopup("Waypoint Rotation", meCameraRotationMode);
            meHandlePositionMode = (eBZCEManipulationModes)EditorGUILayout.EnumPopup("Handle Translation", meHandlePositionMode);
            GUILayout.EndVertical();
        }
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("BZC_CameraTranslateMode", (int)meCameraTranslateMode);
            PlayerPrefs.SetInt("BZC_CameraRotationMode", (int)meCameraRotationMode);
            PlayerPrefs.SetInt("BZC_HandlePositionMode", (int)meHandlePositionMode);
            SceneView.RepaintAll();
        }
    }

    void DrawWaypointList()
    {
        GUILayout.Label("Replace all lerp types");
        GUILayout.BeginVertical("Box");
        GUILayout.BeginHorizontal();
        meAllCurveType = (eBZCECurveType)EditorGUILayout.EnumPopup(meAllCurveType, GUILayout.Width(Screen.width / 3f));
        if (GUILayout.Button(replaceAllPositionContent))
        {
            Undo.RecordObject(mBZCObjectPathScr, "Applied new position");
            foreach (var index in mBZCObjectPathScr._ListOfPoints)
            {
                index._eCurveTypePosition = meAllCurveType;
                if (meAllCurveType == eBZCECurveType.Custom)
                    index._PositionCurve.keys = mAllAnimationCurve.keys;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUI.enabled = meAllCurveType == eBZCECurveType.Custom;
        mAllAnimationCurve = EditorGUILayout.CurveField(mAllAnimationCurve, GUILayout.Width(Screen.width / 3f));
        GUI.enabled = true;
        if (GUILayout.Button(replaceAllRotationContent))
        {
            Undo.RecordObject(mBZCObjectPathScr, "Applied new rotation");
            foreach (var index in mBZCObjectPathScr._ListOfPoints)
            {
                index._eCurveTypeRotation = meAllCurveType;
                if (meAllCurveType == eBZCECurveType.Custom)
                    index._RotationCurve = mAllAnimationCurve;
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.BeginHorizontal();
        GUILayout.Space(Screen.width/2f-20);
        GUILayout.Label("↓");
        GUILayout.EndHorizontal();
        serializedObject.Update();
        mRListOfPoint.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
        Rect r = GUILayoutUtility.GetRect(Screen.width - 16, 18);
        //r.height = 18;
        r.y -= 10;
        GUILayout.Space(-30);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(addPointContent))
        {
            Undo.RecordObject(mBZCObjectPathScr, "Added camera path point");
            switch (meWaypointMode)
            {
                case eBZCENewWaypointMode.SceneCamera:
                    mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(SceneView.lastActiveSceneView.camera.transform.position, SceneView.lastActiveSceneView.camera.transform.rotation));
                    break;
                case eBZCENewWaypointMode.LastWaypoint:
                    if (mBZCObjectPathScr._ListOfPoints.Count > 0)
                        mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(mBZCObjectPathScr._ListOfPoints[mBZCObjectPathScr._ListOfPoints.Count - 1]._vPosition, mBZCObjectPathScr._ListOfPoints[mBZCObjectPathScr._ListOfPoints.Count - 1]._qRotation) { _vHandleNext = mBZCObjectPathScr._ListOfPoints[mBZCObjectPathScr._ListOfPoints.Count - 1]._vHandleNext, _vHandlePrev = mBZCObjectPathScr._ListOfPoints[mBZCObjectPathScr._ListOfPoints.Count - 1]._vHandlePrev });
                    else
                    {
                        mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(Vector3.zero, Quaternion.identity));
                        Debug.LogWarning("No previous waypoint found to place this waypoint, defaulting position to world center");
                    }
                    break;
                case eBZCENewWaypointMode.WaypointIndex:
                    if (mBZCObjectPathScr._ListOfPoints.Count > miWaypointIndex-1 && miWaypointIndex > 0)
                        mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(mBZCObjectPathScr._ListOfPoints[miWaypointIndex-1]._vPosition, mBZCObjectPathScr._ListOfPoints[miWaypointIndex-1]._qRotation) { _vHandleNext = mBZCObjectPathScr._ListOfPoints[miWaypointIndex-1]._vHandleNext, _vHandlePrev = mBZCObjectPathScr._ListOfPoints[miWaypointIndex-1]._vHandlePrev });
                    else
                    {
                        mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(Vector3.zero, Quaternion.identity));
                        Debug.LogWarning("Waypoint index "+miWaypointIndex+" does not exist, defaulting position to world center");
                    }
                    break;
                case eBZCENewWaypointMode.WorldCenter:
                    mBZCObjectPathScr._ListOfPoints.Add(new BZCPoint(Vector3.zero, Quaternion.identity));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            miSelectedIndex = mBZCObjectPathScr._ListOfPoints.Count - 1;
            SceneView.RepaintAll();
        }
        GUILayout.Label("at", GUILayout.Width(20));
        EditorGUI.BeginChangeCheck();
        meWaypointMode = (eBZCENewWaypointMode) EditorGUILayout.EnumPopup(meWaypointMode, meWaypointMode==eBZCENewWaypointMode.WaypointIndex ? GUILayout.Width(Screen.width/4) : GUILayout.Width(Screen.width/2));
        if (meWaypointMode == eBZCENewWaypointMode.WaypointIndex)
        {
            miWaypointIndex = EditorGUILayout.IntField(miWaypointIndex, GUILayout.Width(Screen.width / 4));
        }
        if (EditorGUI.EndChangeCheck())
        {
            PlayerPrefs.SetInt("BZC_WaypointMode", (int)meWaypointMode);
        }
        GUILayout.EndHorizontal();
    }

    void DrawHandles(int i)
    {
        DrawHandleLines(i);
        Handles.color = mBZCObjectPathScr.BZCVisualScr._HandleColor;
        DrawNextHandle(i);
        DrawPrevHandle(i);
        DrawWaypointHandles(i);
        DrawSelectionHandles(i);
    }

    void DrawHandleLines(int i)
    {
        Handles.color = mBZCObjectPathScr.BZCVisualScr._HandleColor;
        if (i < mBZCObjectPathScr._ListOfPoints.Count - 1 || mBZCObjectPathScr._bLooped == true)
            Handles.DrawLine(mBZCObjectPathScr._ListOfPoints[i]._vPosition, mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext);
        if (i > 0 || mBZCObjectPathScr._bLooped == true)
            Handles.DrawLine(mBZCObjectPathScr._ListOfPoints[i]._vPosition, mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev);
        Handles.color = Color.white;
    }

    void DrawNextHandle(int i)
    {
        if (i < mBZCObjectPathScr._ListOfPoints.Count - 1 || loopedProperty.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 posNext = Vector3.zero;
            float size = HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext) * 0.1f;
            if (meHandlePositionMode == eBZCEManipulationModes.Free)
            {
#if UNITY_5_5_OR_NEWER
                posNext = Handles.FreeMoveHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext, Quaternion.identity, size, Vector3.zero, Handles.SphereHandleCap);
#else
                posNext = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handlenext, Quaternion.identity, size, Vector3.zero, Handles.SphereCap);
#endif
            }
            else
            {
                if (miSelectedIndex == i)
                {
#if UNITY_5_5_OR_NEWER
                    Handles.SphereHandleCap(0, mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext, Quaternion.identity, size, EventType.Repaint);
#else
                    Handles.SphereCap(0, t.points[i].position + t.points[i].handlenext, Quaternion.identity, size);
#endif
                    posNext = Handles.PositionHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext, Quaternion.identity);
                }
                else if (Event.current.button != 1)
                {
#if UNITY_5_5_OR_NEWER
                    if (Handles.Button(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext, Quaternion.identity, size, size, Handles.CubeHandleCap))
                    {
                        SelectIndex(i);
                    }
#else
                    if (Handles.Button(t.points[i].position + t.points[i].handlenext, Quaternion.identity, size, size, Handles.CubeCap))
                    {
                        SelectIndex(i);
                    }
#endif
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Handle Position");
                mBZCObjectPathScr._ListOfPoints[i]._vHandleNext = posNext - mBZCObjectPathScr._ListOfPoints[i]._vPosition;
                if (mBZCObjectPathScr._ListOfPoints[i]._bChained)
                    mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev = mBZCObjectPathScr._ListOfPoints[i]._vHandleNext * -1;
            }
        }

    }

    void DrawPrevHandle(int i)
    {
        if (i > 0 || loopedProperty.boolValue)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 posPrev = Vector3.zero;
            float size = HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev) * 0.1f;
            if (meHandlePositionMode == eBZCEManipulationModes.Free)
            {
#if UNITY_5_5_OR_NEWER
                posPrev = Handles.FreeMoveHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev), Vector3.zero, Handles.SphereHandleCap);
#else
                posPrev = Handles.FreeMoveHandle(t.points[i].position + t.points[i].handleprev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handleprev), Vector3.zero, Handles.SphereCap);
#endif
            }
            else
            {
                if (miSelectedIndex == i)
                {
#if UNITY_5_5_OR_NEWER
                    Handles.SphereHandleCap(0, mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev, Quaternion.identity, 0.1f * HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandleNext), EventType.Repaint);
#else
                    Handles.SphereCap(0, t.points[i].position + t.points[i].handleprev, Quaternion.identity,
                        0.1f * HandleUtility.GetHandleSize(t.points[i].position + t.points[i].handlenext));
#endif
                    posPrev = Handles.PositionHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev, Quaternion.identity);
                }
                else if (Event.current.button != 1)
                {
#if UNITY_5_5_OR_NEWER
                    if (Handles.Button(mBZCObjectPathScr._ListOfPoints[i]._vPosition + mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev, Quaternion.identity, size, size, Handles.CubeHandleCap))
                    {
                        SelectIndex(i);
                    }
#else
                    if (Handles.Button(t.points[i].position + t.points[i].handleprev, Quaternion.identity, size, size,
                        Handles.CubeCap))
                    {
                        SelectIndex(i);
                    }
#endif
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Handle Position");
                mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev = posPrev - mBZCObjectPathScr._ListOfPoints[i]._vPosition;
                if (mBZCObjectPathScr._ListOfPoints[i]._bChained)
                    mBZCObjectPathScr._ListOfPoints[i]._vHandleNext = mBZCObjectPathScr._ListOfPoints[i]._vHandlePrev * -1;
            }
        }
    }

    void DrawWaypointHandles(int i)
    {
        if (Tools.current == Tool.Move)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 pos = Vector3.zero;
            if (meCameraTranslateMode == eBZCEManipulationModes.SelectAndTransform)
            {
                if (i == miSelectedIndex) pos = Handles.PositionHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition, (Tools.pivotRotation == PivotRotation.Local) ? mBZCObjectPathScr._ListOfPoints[i]._qRotation : Quaternion.identity);
            }
            else
            {
#if UNITY_5_5_OR_NEWER
                pos = Handles.FreeMoveHandle(mBZCObjectPathScr._ListOfPoints[i]._vPosition, (Tools.pivotRotation == PivotRotation.Local) ? mBZCObjectPathScr._ListOfPoints[i]._qRotation : Quaternion.identity, HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition) * 0.2f, Vector3.zero, Handles.RectangleHandleCap);
#else
                pos = Handles.FreeMoveHandle(t.points[i].position, (Tools.pivotRotation == PivotRotation.Local) ? t.points[i].rotation : Quaternion.identity, HandleUtility.GetHandleSize(t.points[i].position) * 0.2f, Vector3.zero, Handles.RectangleCap);
#endif
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Moved Waypoint");
                mBZCObjectPathScr._ListOfPoints[i]._vPosition = pos;
            }
        }
        else if (Tools.current == Tool.Rotate)
        {

            EditorGUI.BeginChangeCheck();
            Quaternion rot = Quaternion.identity;
            if (meCameraRotationMode == eBZCEManipulationModes.SelectAndTransform)
            {
                if (i == miSelectedIndex) rot = Handles.RotationHandle(mBZCObjectPathScr._ListOfPoints[i]._qRotation, mBZCObjectPathScr._ListOfPoints[i]._vPosition);
            }
            else
            {
                rot = Handles.FreeRotateHandle(mBZCObjectPathScr._ListOfPoints[i]._qRotation, mBZCObjectPathScr._ListOfPoints[i]._vPosition, HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition) * 0.2f);
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Rotated Waypoint");
                mBZCObjectPathScr._ListOfPoints[i]._qRotation = rot;
            }
        }
    }

    void DrawSelectionHandles(int i)
    {
        if (Event.current.button != 1 && miSelectedIndex != i)
        {
            if (meCameraTranslateMode == eBZCEManipulationModes.SelectAndTransform && Tools.current == Tool.Move
                || meCameraRotationMode == eBZCEManipulationModes.SelectAndTransform && Tools.current == Tool.Rotate)
            {
                float size = HandleUtility.GetHandleSize(mBZCObjectPathScr._ListOfPoints[i]._vPosition) * 0.2f;
#if UNITY_5_5_OR_NEWER
                if (Handles.Button(mBZCObjectPathScr._ListOfPoints[i]._vPosition, Quaternion.identity, size, size, Handles.CubeHandleCap))
                {
                    SelectIndex(i);
                }
#else
                if (Handles.Button(t.points[i].position, Quaternion.identity, size, size, Handles.CubeCap))
                {
                    SelectIndex(i);
                }
#endif
            }
        }
    }

    void DrawRawValues()
    {
        if (GUILayout.Button(mbShowRawValues ? "Hide raw values" : "Show raw values"))
            mbShowRawValues = !mbShowRawValues;

        if (mbShowRawValues)
        {
            foreach (var i in mBZCObjectPathScr._ListOfPoints)
            {
                EditorGUI.BeginChangeCheck();
                GUILayout.BeginVertical("Box");
                Vector3 pos = EditorGUILayout.Vector3Field("Waypoint Position", i._vPosition);
                Quaternion rot = Quaternion.Euler(EditorGUILayout.Vector3Field("Waypoint Rotation", i._qRotation.eulerAngles));
                Vector3 posp = EditorGUILayout.Vector3Field("Previous Handle Offset", i._vHandlePrev);
                Vector3 posn = EditorGUILayout.Vector3Field("Next Handle Offset", i._vHandleNext);
                GUILayout.EndVertical();
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(mBZCObjectPathScr, "Changed waypoint transform");
                    i._vPosition = pos;
                    i._qRotation = rot;
                    i._vHandlePrev = posp;
                    i._vHandleNext = posn;
                    SceneView.RepaintAll();
                }
            }
        }
    }
}