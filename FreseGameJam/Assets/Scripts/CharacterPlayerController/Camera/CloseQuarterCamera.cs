using UnityEngine;

public class CloseQuarterCamera : MonoBehaviour
{
    #region variables
    // public variables:
    public bool closeQuarterCameraIsActive = false;
    public bool inCustscene = false;
    public GameObject firstPersonCamera;

    // local variables:
    private GameObject _lastTriggerHit;
    [SerializeField] GameObject _cameraRigFar;
    [SerializeField] GameObject _cameraRigClose;
    private bool _currentlyInsideTrigger = false;
    #endregion
    void Start()
    {
        if(firstPersonCamera != null)
        {
            _cameraRigClose = firstPersonCamera;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CloseQuarterCameraTrigger") && !_currentlyInsideTrigger)
        {
            _lastTriggerHit = other.gameObject;
            _lastTriggerHit.GetComponent<BoxCollider>().enabled = false;
            Invoke("ReactivateTrigger", 5f);
            _currentlyInsideTrigger = true; // redundant here, as turning on/off the collider solves the same issue.

            Zoom();
        }
    }
    public void Zoom()
    {
        if (!closeQuarterCameraIsActive)
        {
            ZoomIn();
        }else
        {
            ZoomOut();
        }
    }
    public void ZoomIn()
    {
        if (!inCustscene)
        {
            _cameraRigClose.SetActive(true);
            _cameraRigFar.SetActive(false);

            FindObjectOfType<HealthSystem>().UpdateLastUsedCamera(); // this should save currently used main camera setting!

            closeQuarterCameraIsActive = true;
        }
    }
    public void ZoomOut()
    {
        if (!inCustscene)
        {
            _cameraRigFar.SetActive(true);
            _cameraRigClose.SetActive(false);

            FindObjectOfType<HealthSystem>().UpdateLastUsedCamera(); // this should save currently used main camera setting!

            closeQuarterCameraIsActive = false;
        }
    }

    void ReactivateTrigger()
    {
        _currentlyInsideTrigger = false; // redundant here, as turning on/off the collider solves the same issue.
        _lastTriggerHit.GetComponent<BoxCollider>().enabled = true;
    }

    private void Update() // failsafe to enable zones after dying!
    {
        if (GetComponent<HealthSystem>().inCoroutine)
        {
            Debug.Log("I died and am resetting camera");

            ReactivateTrigger();
        }
    }
}
