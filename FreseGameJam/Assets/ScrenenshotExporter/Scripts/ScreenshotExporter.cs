using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;

public class ScreenshotExporter : MonoBehaviour
{
#if UNITY_EDITOR

	[SerializeField]
	private Camera gameCamera;
	[SerializeField]
	private Camera sceneCamera;
	[SerializeField]
	private string outputDirectory = @"%UserProfile%\Desktop";
	[SerializeField]
	private Vector2Int imageSize = new Vector2Int(1920, 1080);
	[SerializeField]
	private bool clearBackground = false;


	public void TakeGameImage()
	{
		SaveImage(gameCamera);
	}


	public void TakeSceneImage()
	{
		Camera sceneViewCamera = SceneView.lastActiveSceneView.camera;
		sceneCamera.transform.position = sceneViewCamera.transform.position;
		sceneCamera.transform.rotation = sceneViewCamera.transform.rotation;

		SaveImage(sceneCamera);
	}


	private void SaveImage(Camera camera)
	{
		Texture2D image = TakeImage(camera);
		
		//clear background
		if (clearBackground)
		{
			Texture2D alphaImage = TakeImage(camera, CameraType.Game);

			for (int y = 0; y < image.height; y++)
			{
				for (int x = 0; x < image.width; x++)
				{
					Color color = image.GetPixel(x, y);
					color.a = alphaImage.GetPixel(x, y).a;
					image.SetPixel(x, y, color);
				}
			}
		}

		//save image to file system
		var Bytes = image.EncodeToPNG();
		DestroyImmediate(image);
		string path = GetFileName();
		File.WriteAllBytes(path, Bytes);
		Debug.Log($"Exported Screenshot to \"{path}\"");
	}


	private Texture2D TakeImage(Camera camera, CameraType cameraType = CameraType.Game)
	{
		//set cameras render texture
		RenderTexture prevousRenderTexture = camera.targetTexture;
		RenderTexture renderTexture = new RenderTexture(imageSize.x, imageSize.y, 32);
		camera.targetTexture = renderTexture;

		CameraType prevousCameraType = camera.cameraType;
		camera.cameraType = cameraType;

		//render image
		camera.Render();
		RenderTexture.active = renderTexture;
		Texture2D image = new Texture2D(imageSize.x, imageSize.y);
		image.ReadPixels(new Rect(0, 0, imageSize.x, imageSize.y), 0, 0);
		image.Apply();

		//reset cameras render texture
		camera.targetTexture = prevousRenderTexture;

		return image;
	}


	private string GetFileName()
	{
		string path = Path.Combine(outputDirectory, DateTime.Now.ToString("yyyy_MM_dd - HH_mm_ss_fff") + ".png");
		path = Environment.ExpandEnvironmentVariables(path);
		path = Path.GetFullPath(path);
		return path;
	}

#endif 
}
