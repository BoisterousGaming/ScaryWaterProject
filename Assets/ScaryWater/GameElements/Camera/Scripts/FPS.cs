using UnityEngine;
using System.Collections;

public class FPS : MonoBehaviour 
{
	float lastInterval;
	int frames = 0;

	public int _NeededFPS = 50;
	public float updateInterval = 0.5f;
	public float fps;

	void Start()
	{
		Application.targetFrameRate = _NeededFPS;

		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	
 	void Update()
	{
		++frames;
		float timeNow = Time.realtimeSinceStartup;
		
		if (timeNow > lastInterval + updateInterval) 
		{
			fps = frames / (timeNow - lastInterval);
			frames = 0;
			lastInterval = timeNow;
		}
	}

	void OnGUI()
	{
		GUI.depth = -1000;
		GUI.color = Color.cyan;
		GUILayout.Button(fps.ToString ("0"));
	}
}