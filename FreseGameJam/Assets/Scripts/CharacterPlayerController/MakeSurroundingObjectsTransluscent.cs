using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MakeSurroundingObjectsTransluscent : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private float _transluscentAlpha = 0.5f;
    [SerializeField] private float _detectionRadius = 5f;

    [SerializeField] Material _boxMaterial;
    [SerializeField] Material _woodMaterial;


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
                }*/


                // NEW: switching to a set material, as having them generally transparent leads to really weird see-through issues!
                // create a temporary copy of the materials:
                Material[] copiedMaterials = new Material[materials.Length];
                for (int j = 0; j < materials.Length; j++)
                {
                    // check the type of fragment and replace with the correct alpha-material:
                    Material _mat = null;
                    switch (_colliders[j].GetComponent<MakeTraversable>().typeOfObject)
                    {
                        case 0: // default value
                            //do nothing
                            break;

                        case 1: // box fragment
                            _mat = _boxMaterial;
                            break;

                        case 2: // wooden fragment
                            _mat = _woodMaterial;
                            break;

                        case 3: // not yet used!
                            //do nothing
                            break;

                        default:
                            break;
                    }

                    copiedMaterials[j] = _mat;
                    //copiedMaterials[j] = _boxMaterial;
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
}