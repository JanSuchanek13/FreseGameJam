using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    Vector3 reticlePosition;
    GameObject _player;

    private void Start()
    {
            _player = GameObject.Find("Third Person Player");
    }
    private void Update()
    {
        /*   if (_player.GetComponent<HealthSystem>().isDead == false)
           {
               //this layer will be ignored by the Raycast
               //int layerMask = LayerMask.GetMask("Player");
               //layerMask = ~layerMask;
               int layerMask = (1 << 7) | (1 << 2);
               layerMask = ~layerMask;

               Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
               RaycastHit hit;
               if (Physics.Raycast(screenRay, out hit, Mathf.Infinity, layerMask))
               {
                   reticlePosition = hit.point;
               }
               transform.LookAt(reticlePosition); // this will only work with a CharacterController component
           } */

        transform.LookAt(_player.transform.position);





    }
}
