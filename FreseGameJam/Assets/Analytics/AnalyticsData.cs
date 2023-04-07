using System;
using System.Collections.Generic;


[Serializable]
public struct AnalyticsData
{
	public Guid sessionID;
	public List<PathTracker.StepData> track;
}
