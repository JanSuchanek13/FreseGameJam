using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MakeSurroundingObjectsTransluscent : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _transluscentAlpha = 0.5f;
    [SerializeField] private float _detectionRadius = 5f;

    [SerializeField] Material _material;

    private Collider[] _colliders;
    private Renderer[] _originalRenderers;
    private Dictionary<Renderer, Material[]> _originalMaterialsDict;

    private void Update()
    {
        // Get all the colliders within the detection radius
        _colliders = Physics.OverlapSphere(transform.position, _detectionRadius, _targetLayer);

        // Iterate through each collider and update the material transparency
        for (int i = 0; i < _colliders.Length; i++)
        {
            Renderer renderer = _colliders[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                if (!_originalMaterialsDict.ContainsKey(renderer))
                {
                    // Store the original materials for the renderer
                    Material[] originalMaterials = renderer.materials;
                    _originalMaterialsDict.Add(renderer, originalMaterials);
                }

                Material[] materials = renderer.materials;

                /* // this worked! But using default-transparent mats leads to really crappy artifacts on GO's
                // Create a temporary copy of the materials
                Material[] copiedMaterials = new Material[materials.Length];
                for (int j = 0; j < materials.Length; j++)
                {
                    copiedMaterials[j] = new Material(materials[j]);
                    copiedMaterials[j].color = new Color(materials[j].color.r, materials[j].color.g, materials[j].color.b, _transluscentAlpha);
                }

                // Apply the copied materials to the renderer
                renderer.materials = copiedMaterials;*/

                // NEW: switching to a set material, as having them generally transparent leads to really weird see-through issues!
                // Create a temporary copy of the materials
                Material[] copiedMaterials = new Material[materials.Length];
                for (int j = 0; j < materials.Length; j++)
                {
                    copiedMaterials[j] = _material;
                    //copiedMaterials[j].color = new Color(materials[j].color.r, materials[j].color.g, materials[j].color.b, _transluscentAlpha);
                }

                // Apply the copied materials to the renderer
                renderer.materials = copiedMaterials;
            }
        }

        // Iterate through each previously detected renderer
        // and restore their original materials if they are no longer within the detection radius
        foreach (var kvp in _originalMaterialsDict)
        {
            Renderer renderer = kvp.Key;
            Collider rendererCollider = renderer.GetComponent<Collider>();

            bool colliderFound = false;
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i] == rendererCollider)
                {
                    colliderFound = true;
                    break;
                }
            }

            if (renderer != null && !colliderFound)
            {
                Material[] originalMaterials = kvp.Value;
                renderer.materials = originalMaterials;
            }
        }
    }

    private void OnEnable()
    {
        // Initialize the original materials dictionary
        _originalMaterialsDict = new Dictionary<Renderer, Material[]>();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }


    //this worked!:
    /*
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _transluscentAlpha = 0.5f;
    [SerializeField] private float _detectionRadius = 5f;

    private Collider[] _colliders;
    private Renderer[] _originalRenderers;
    private Material[][] _originalMaterials;

    private void Update()
    {
        // Get all the colliders within the detection radius
        _colliders = Physics.OverlapSphere(transform.position, _detectionRadius, _targetLayer);

        // Iterate through each collider and update the material transparency
        for (int i = 0; i < _colliders.Length; i++)
        {
            Renderer renderer = _colliders[i].GetComponent<Renderer>();

            if (renderer != null)
            {
                Material[] materials = renderer.materials;

                // Create a temporary copy of the materials
                Material[] copiedMaterials = new Material[materials.Length];
                for (int j = 0; j < materials.Length; j++)
                {
                    copiedMaterials[j] = new Material(materials[j]);
                }

                // Apply translucency to the copied materials
                /*for (int j = 0; j < copiedMaterials.Length; j++)
                {
                    Color color = copiedMaterials[j].color;
                    color.a = _transluscentAlpha;
                    copiedMaterials[j].color = color;
                } // here was an end to a commentary
                // Apply translucency to the copied materials
                for (int j = 0; j < copiedMaterials.Length; j++)
                {
                    Color color = copiedMaterials[j].color;
                    color.a = _transluscentAlpha;
                    copiedMaterials[j].color = color;

                    /*
                    // Set the rendering mode to Transparent
                    copiedMaterials[j].SetOverrideTag("RenderType", "Transparent");
                    copiedMaterials[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    copiedMaterials[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    copiedMaterials[j].SetInt("_ZWrite", 0);
                    copiedMaterials[j].DisableKeyword("_ALPHATEST_ON");
                    copiedMaterials[j].EnableKeyword("_ALPHABLEND_ON");
                    copiedMaterials[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    copiedMaterials[j].renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;// here was an end to an commentary
                }


                // Assign the copied materials to the renderer
                renderer.materials = copiedMaterials;
            }
        }

        // Iterate through each previously detected renderer
        // and restore their original materials if they are no longer within the detection radius
        for (int i = 0; i < _originalRenderers.Length; i++)
        {
            if (!ArrayContainsRenderer(_colliders, _originalRenderers[i]))
            {
                Renderer renderer = _originalRenderers[i];

                if (renderer != null)
                {
                    renderer.materials = _originalMaterials[i];
                }
            }
        }
    }

    private bool ArrayContainsRenderer(Collider[] array, Renderer renderer)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Renderer arrayRenderer = array[i].GetComponent<Renderer>();

            if (arrayRenderer == renderer)
                return true;
        }

        return false;
    }

    private void OnEnable()
    {
        // Store the original renderers and materials
        _colliders = new Collider[0];
        _originalRenderers = GetComponentsInChildren<Renderer>();
        _originalMaterials = new Material[_originalRenderers.Length][];

        for (int i = 0; i < _originalRenderers.Length; i++)
        {
            _originalMaterials[i] = _originalRenderers[i].materials;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wire sphere to visualize the detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }*/
}