using UnityEngine;

public class TurnOffWhenPlaying : MonoBehaviour
{
    [SerializeField] bool _destroyThis = false;
    void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;

        if (_destroyThis)
        {
            Destroy(gameObject);
        }
    }
}
