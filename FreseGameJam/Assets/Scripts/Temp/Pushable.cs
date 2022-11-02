using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pushable : MonoBehaviour
{
    [Header("REFERENCES")]
    private Rigidbody myRigid;

    public void Awake()
    {
        myRigid = this.GetComponent<Rigidbody>();
    }

    public void OnEnable()
    {
        myRigid.isKinematic = true;
    }

    /// <summary>
    /// We push the Object
    /// </summary>
    /// <param name="_pushPos"></param>
    /// <param name="_pushForce"></param>
    public void PushObject(Transform _pusherTrans, float _pushForce)
    {
        myRigid.isKinematic = false;
        Vector3 direction = (this.transform.position - _pusherTrans.position).normalized;
        myRigid.AddRelativeForce(direction * _pushForce, ForceMode.Impulse);

        Debug.Log("getting pushed");
    }
}
