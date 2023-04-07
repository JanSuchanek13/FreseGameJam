using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTracker : MonoBehaviour
{
	//Settings
	[SerializeField]
	float stepIntervall = 0.5f;
	[SerializeField]
	float sendIntervall = 10.0f;
	[SerializeField]
	bool trackingEnabled = true;


	//Globals
	private float nextStep;
	private float nextSend;
	private List<StepData> track;
	public List<StepData> Track { get => track; }


	//Defenitions
	public struct StepData
	{
		public Vector3 pos;
	}


	//Functions
	void Start()
	{
		track = new List<StepData>();

		if (trackingEnabled)
		{
			StartTracking();
		}
	}


	void Update()
	{
		if (trackingEnabled)
		{
			if (Time.time >= nextStep)
			{
				RecordStep();
				nextStep = Time.time + stepIntervall;
			}

			if (Time.time >= nextSend)
			{
				SendTrack();
				nextSend = Time.time + sendIntervall;
			}
		}
	}


	private void RecordStep()
	{
		track.Add(new StepData
		{
			pos = transform.position,
		});
	}


	private void SendTrack()
	{
		AnalyticsManager.Instance.SendTrackToServer(track);
		track = new List<StepData>();
	}


	public void StartTracking()
	{
		nextStep = Time.time; //start with a step
		nextSend = Time.time + sendIntervall; //don't start with sending data

		trackingEnabled = true;
	}


	public void StopTracking()
	{
		SendTrack();

		trackingEnabled = false;
	}
}
