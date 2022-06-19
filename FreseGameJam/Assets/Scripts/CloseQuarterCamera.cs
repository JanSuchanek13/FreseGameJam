using UnityEngine;

public class CloseQuarterCamera : MonoBehaviour
{
    #region variables
    private GameObject _lastTriggerHit;
    private GameObject _cameraRigFar;
    private GameObject _cameraRigClose;
    private bool _closeQuarterCameraIsActive = false;
    private bool _currentlyInsideTrigger = false;
    #endregion
    void Start()
    {
        _cameraRigFar = this.transform.GetChild(0).gameObject;
        _cameraRigClose = this.transform.GetChild(1).gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CloseQuarterCameraTrigger") && !_currentlyInsideTrigger)
        {
            _lastTriggerHit = other.gameObject;
            _lastTriggerHit.GetComponent<BoxCollider>().enabled = false;
            Invoke("ReactivateTrigger", 1f);
            _currentlyInsideTrigger = true; // redundant here, as turning on/off the collider solves the same issue.

            if (!_closeQuarterCameraIsActive)
            {
                ZoomIn();
            }else
            {
                ZoomOut();
            }
        }
    }
    void ZoomIn()
    {
        _cameraRigFar.SetActive(false);
        _cameraRigClose.SetActive(true);
        _closeQuarterCameraIsActive = true;
        Debug.Log("zoom in");
    }
    void ZoomOut()
    {
        _cameraRigFar.SetActive(true);
        _cameraRigClose.SetActive(false);
        _closeQuarterCameraIsActive = false;
        Debug.Log("zoom out");
    }

    void ReactivateTrigger()
    {
        _currentlyInsideTrigger = false; // redundant here, as turning on/off the collider solves the same issue.
        _lastTriggerHit.GetComponent<BoxCollider>().enabled = true;
    }
    private void Update() // failsafe to enable zones after dying!
    {
        if (GetComponent<HealthSystem>().inUse)
        {
            Debug.Log("I died and am resetting camera");
            ZoomOut();
            ReactivateTrigger();
        }
    }
}

    //this was beautifull but i made one mistake... sitting on a trigger they always thought they were the first one hit...
    /*
    #region variables
    private GameObject _player;
    private GameObject _cameraRigFar;
    //private GameObject _cameraRigClose; // not needed as its hirarchy placement seems to be enough to fit in snug with camera logic (far, if far is available, otherwise close!)
    private bool _closeQuarterCameraIsActive = false;
    private bool _currentlyInsideTrigger = false;
    #endregion

    void Start()
    {
        _player = GameObject.Find("Third Person Player");
        _cameraRigFar = _player.transform.GetChild(0).gameObject;
        //_cameraRigClose = _player.transform.GetChild(1).gameObject; // redundancy *see above*
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_currentlyInsideTrigger)
        {
            GetComponent<BoxCollider>().enabled = false;
            Invoke("ReactivateTrigger", 1f);
            _currentlyInsideTrigger = true; // redundant here, as turning on/off the collider solves the same issue.

            if (!_closeQuarterCameraIsActive)
            {
                ZoomIn();
            }
            else
            {
                ZoomOut();
            }
        }
    }

    /*private void OnTriggerExit(Collider other) // this is much more elegant but does not seem to work with a sphere/mesh-collider that is on the Player Avatar (box should work with this, but to much extra work)
    {
        if (other.CompareTag("Player"))
        {
            _currentlyInsideTrigger = false;
        }
    }
    void ZoomIn()
    {
        _cameraRigFar.SetActive(false);
        _closeQuarterCameraIsActive = true;
        Debug.Log("zoom in");
    }
    void ZoomOut()
    {
        _cameraRigFar.SetActive(true);
        _closeQuarterCameraIsActive = false;
        Debug.Log("zoom out");
    }

    void ReactivateTrigger()
    {
        _currentlyInsideTrigger = false; // redundant here, as turning on/off the collider solves the same issue.
        GetComponent<BoxCollider>().enabled = true;
    }

    private void Update() // failsafe to enable zones after dying!
    {
        if (_player.GetComponent<HealthSystem>().inUse)
        {
            Debug.Log("I died");
            ZoomOut();
            _currentlyInsideTrigger = false; // redundant here, as turning on/off the collider solves the same issue.
            GetComponent<BoxCollider>().enabled = true;
        }
    }
}*/
