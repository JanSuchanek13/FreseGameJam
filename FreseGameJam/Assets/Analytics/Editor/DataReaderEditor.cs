using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(DataReader))]
public class DataReaderEditor : Editor
{
	//Globals
	private new DataReader target;


	//Functions
	private void Awake()
	{
		target = (DataReader)base.target;
	}


	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("Generate Heatmap"))
		{
			target.GenerateHeatmap();
		}
	}
}
