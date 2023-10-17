using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    //Vector3 reticlePosition;
    GameObject _player;
    Camera _mainCamera;
    [SerializeField] bool uiInterface = false;
    [SerializeField] GameObject lookAtObject;

    private void Start()
    {
        if (!uiInterface && lookAtObject == null)
        {
            _player = GameObject.Find("Third Person Player");
        }
        _mainCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        if (!uiInterface && lookAtObject == null)
        {
            transform.LookAt(_player.transform.position);
        }else if (lookAtObject != null)
        {
            transform.LookAt(lookAtObject.transform.position);
        }else
        {
            transform.LookAt(_mainCamera.transform.position);
        }
    }
}
