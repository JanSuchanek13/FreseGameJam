using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using System;
using static PathTracker;

public class AnalyticsManager : MonoBehaviour
{
    //Settings
    [SerializeField]
    private string serverAddress;
	[SerializeField]
	private string surveyResultsFolder;


	//Globals
	static private AnalyticsManager instance = null;
    static public AnalyticsManager Instance
    {
        get
		{
            if (instance == null)
			{
                instance = Instantiate(Resources.Load("SingeltonPrefab/AnalyticsManager") as GameObject).GetComponent<AnalyticsManager>();
            }
            return instance;
		}
        private set => instance = value;
    }
    private Guid sessionID;
    //private SurveyData data;


	//Functions
	void Awake()
	{
        DontDestroyOnLoad(gameObject);
        sessionID = Guid.NewGuid();
	}


    public void SendTrackToServer(List<StepData> track)
	{
		AnalyticsData data = new AnalyticsData
		{
			sessionID = sessionID,
			track = track
		};
        StartCoroutine(SendDataToServer(data));
    }


    private IEnumerator SendDataToServer(AnalyticsData data)
    {
        WWWForm form = new WWWForm();
        form.AddField("data", ObjectToXml(data));

        UnityWebRequest www = UnityWebRequest.Post(serverAddress, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Analytics uploaded sucessfully.");
        }
	}


	public IEnumerator CheckServerAvailability(System.Action<bool> callback)
	{
		UnityWebRequest www = UnityWebRequest.Get(serverAddress);
		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success)
		{
			//Server is unavailable
			Debug.LogError(www.result);
			callback(false);
		}
		else
		{
			//Server is available
			callback(true);
		}
	}


	public List<AnalyticsData> ReadAnalyticsFromFolder()
	{
		List<AnalyticsData> dataObjects = new List<AnalyticsData>();

		foreach (string filePath in Directory.EnumerateFiles(surveyResultsFolder, "*.txt"))
		{
			string fileContent = File.ReadAllText(filePath, System.Text.Encoding.Unicode);
			dataObjects.Add(XmlToObject<AnalyticsData>(fileContent));
		}

		return dataObjects;
	}


	private static string ObjectToXml(object dataObject)
    {
        StringWriter stringWriter = new StringWriter();
        System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(dataObject.GetType());
        xmlSerializer.Serialize(stringWriter, dataObject);
        //stringWriter.Close();
        return stringWriter.ToString();
    }


	private static T XmlToObject<T>(string xml)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(T));
		StringReader stringReader = new StringReader(xml);
		T dataObject = (T)serializer.Deserialize(stringReader);
		stringReader.Close();
		return dataObject;
	}
}
