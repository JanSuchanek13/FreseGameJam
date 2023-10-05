using UnityEngine;

public class AttachToLavaTile : MonoBehaviour
{
    Vector2 offset;
    Vector3 oldPos;
    bool firstContact = true;

    private bool isOnMovingPlatform = false; // Gibt an, ob der Spieler sich auf einer bewegenden Plattform befindet
    private Transform currentPlatform = null; // Referenz auf die aktuelle bewegende Plattform
    private Vector3 lastPlatformPosition; // Die Position der Plattform im vorherigen Frame

    private void FixedUpdate()
    {
        // Die Position und den Radius des SphereCast vorbereiten
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // 0.1f Offset, um Kollision mit dem eigenen Collider zu vermeiden
        float sphereRadius = GetComponent<CharacterController>().radius;

        // Ein SphereCast nach unten durchführen
        RaycastHit hit;
        if (Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out hit, 1.0f))
        {
            if (hit.collider.CompareTag("LavaTile"))
            {
                if (transform.gameObject.GetComponent<InputHandler>().moveValue.magnitude == 0)
                {
                    if(hit.transform.gameObject.GetComponent<LavaTileMovement>() != null)
                    {
                        StartCoroutine(hit.collider.transform.gameObject.GetComponent<LavaTileMovement>().ActivateFire());
                    }

                    if (firstContact)
                    {
                        offset.x = transform.position.x - hit.transform.position.x;
                        offset.y = transform.position.z - hit.transform.position.z;
                        firstContact = false;
                    }

                    //Debug.Log(offset);
                    transform.position = new Vector3(hit.transform.position.x + offset.x, transform.position.y, hit.transform.position.z + offset.y);
                }
                else
                {
                    offset.x = transform.position.x - hit.transform.position.x;
                    offset.y = transform.position.z - hit.transform.position.z;
                    Vector3 direction = hit.transform.position - oldPos;
                    transform.gameObject.GetComponent<CharacterController>().Move(direction.normalized / 50);
                    oldPos = hit.transform.position;
                }


            }
            else
            {
                transform.parent = null;
                
            }

            /*
            // Überprüfen, ob das getroffene Objekt das Tag "LavaTile" hat
            if (hit.collider.CompareTag("LavaTile"))
            {
                // Das GameObject zum Kind des "LavaTile" machen
                transform.parent = hit.collider.transform;
                //Debug.Log("new parent " + hit.collider.transform.name);
                StartCoroutine(hit.collider.transform.gameObject.GetComponent<LavaTileMovement>().ActivateFire());


            }
            else
            {
                // Das GameObject aus der Hierarchie des Vorgängers entfernen
                transform.parent = null;
                //Debug.Log("new parent null");
            }
            */
        }
        else
        {
            // Wenn kein Treffer erfolgt, das GameObject aus der Hierarchie des Vorgängers entfernen
            transform.parent = null;
        }
    }
}