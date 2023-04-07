using HeatmapVisualization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class DataReader : MonoBehaviour
{
	//Globals
	private Heatmap OwnHeatmap { get => GetComponent<Heatmap>(); }


	//Functions

	public void GenerateHeatmap()
	{
		List<Vector3> tracks =  LoadTracks();

		OwnHeatmap.GenerateHeatmap(tracks);
	}


	private List<Vector3> LoadTracks()
	{
		List<AnalyticsData> datas = AnalyticsManager.Instance.ReadAnalyticsFromFolder();

#if UNITY_EDITOR
		if (!Application.isPlaying)
		{
			//if not in playmode remove the Surveymanager to not acumulate them in the scene.
			DestroyImmediate(AnalyticsManager.Instance.gameObject);
		}
#endif

		List<Vector3> tracks = new List<Vector3>();
		foreach (AnalyticsData data in datas)
		{
			tracks.AddRange(data.track.Select((step) => step.pos));
		}

		return tracks;
	}
}
