using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeatmapSettings = HeatmapVisualization.HeatmapTextureGenerator.Settings;


namespace HeatmapVisualization
{
	public class Heatmap : MonoBehaviour
	{
		#region Settings
		[SerializeField]
		private ComputeShader gaussianComputeShader;
		[SerializeField]
		public Vector3Int resolution = new Vector3Int(64, 64, 64);
		[SerializeField]
		[Range(0.0f, 1.0f)]
		public float cutoffPercentage = 1.0f;
		[SerializeField]
		public float gaussStandardDeviation = 1.0f;
		[SerializeField]
		private Gradient colormap;
		[SerializeField]
		private bool renderOnTop = false;
		[SerializeField]
		private FilterMode textureFilterMode = FilterMode.Bilinear;

		private const int colormapTextureResolution = 256;
		#endregion


		#region Globals
		private MeshRenderer ownRenderer;
		private MeshRenderer OwnRenderer { get { if (ownRenderer == null) { ownRenderer = GetComponent<MeshRenderer>(); } return ownRenderer; } }
		public Bounds BoundsFromTransform { get => new Bounds { center = transform.position, size = transform.localScale }; }
		private float maxHeatFromTexture;
		#endregion


		#region Functions
		public void GenerateHeatmap(List<Vector3> points)
		{
			if (points == null)
			{
				return;
			}

			//calculate heatmap texture
			HeatmapSettings settings = new HeatmapSettings(BoundsFromTransform, resolution, gaussStandardDeviation);
			HeatmapTextureGenerator heatmapTextureGenerator = new HeatmapTextureGenerator(gaussianComputeShader);
			float[] heats = heatmapTextureGenerator.CalculateHeatTexture(points, settings);

			//create texture object
			Texture3D heatTexture = new Texture3D(settings.resolution.x, settings.resolution.y, settings.resolution.z, TextureFormat.RFloat, false);
			heatTexture.SetPixelData(heats, 0);
			heatTexture.wrapMode = TextureWrapMode.Clamp;
			heatTexture.filterMode = textureFilterMode;
			heatTexture.Apply();

			maxHeatFromTexture = GetMaxValue(heats);

			//apply To material
			SetHeatTexture(heatTexture);
			SetColormap();
			SetMaxHeat();
			SetRenderOnTop();
			SetTextureFilterMode();
		}


		private void SetHeatTexture(Texture3D heatTexture)
		{
			Material material = new Material(OwnRenderer.sharedMaterial); //not edit the material asset
			material.SetTexture("_DataTex", heatTexture);
			OwnRenderer.sharedMaterial = material;
		}


		public void SetColormap(Gradient colormap)
		{
			this.colormap = colormap;
			SetColormap();
		}


		public void SetColormap()
		{
			OwnRenderer.sharedMaterial.SetTexture("_GradientTex", GradientToTexture(colormap, colormapTextureResolution));
		}


		public void SetMaxHeat()
		{
			OwnRenderer.sharedMaterial.SetFloat("_MaxHeat", maxHeatFromTexture * cutoffPercentage);
		}


		public void SetRenderOnTop(bool renderOnTop)
		{
			this.renderOnTop = renderOnTop;
			SetRenderOnTop();
		}


		public void SetRenderOnTop()
		{
			if (renderOnTop)
			{
				OwnRenderer.sharedMaterial.DisableKeyword("USE_SCENE_DEPTH");
			}
			else
			{
				OwnRenderer.sharedMaterial.EnableKeyword("USE_SCENE_DEPTH");
			}
		}


		public void SetTextureFilterMode(FilterMode textureFilterMode)
		{
			this.textureFilterMode = textureFilterMode;
			SetTextureFilterMode();
		}


		public void SetTextureFilterMode()
		{
			OwnRenderer.sharedMaterial.GetTexture("_DataTex").filterMode = textureFilterMode;
		}


		/// <summary>
		/// Get the maximum value from an array.
		/// </summary>
		private static float GetMaxValue(float[] heats)
		{
			float maxHeat = 0.0f;

			for (int i = 0; i < heats.Length; i++)
			{
				if (heats[i] > maxHeat)
				{
					maxHeat = heats[i];
				}
			}
			
			return maxHeat;
		}


		private static Texture2D GradientToTexture(Gradient gradient, int resolution)
		{
			Texture2D texture = new Texture2D(resolution, 1);

			for (int i = 0; i < resolution; i++)
			{
				texture.SetPixel(i, 1, gradient.Evaluate(((float)i) / (resolution - 1)));
			}

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Bilinear;
			texture.Apply();
			return texture;
		}
		#endregion
	}
}