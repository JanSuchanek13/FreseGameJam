using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ScreenshotExporter))]
public class ScreenshotExporterEditor : Editor
{
	//Globals
	private new ScreenshotExporter target;


	private void Awake()
	{
		target = (ScreenshotExporter)base.target;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Take Screenshot"))
		{
			target.TakeGameImage();
		}

		if (GUILayout.Button("Take Screenshot (Scene View)"))
		{
			target.TakeSceneImage();
		}
	}
}