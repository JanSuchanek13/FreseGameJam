using UnityEngine;

public class AttachToLavaTile : MonoBehaviour
{
    Vector2 offset;
    Vector3 oldPos;
    bool firstContact = true;

    private void Update()
    {
        // Die Position und den Radius des SphereCast vorbereiten
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f; // 0.1f Offset, um Kollision mit dem eigenen Collider zu vermeiden
        float sphereRadius = GetComponent<CharacterController>().radius;

        // Ein SphereCast nach unten durchführen
        RaycastHit hit;
        if (Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out hit, 1.0f))
        {
            // Überprüfen, ob das getroffene Objekt das Tag "LavaTile" hat
            if (hit.collider.CompareTag("LavaTile"))
            {
                // Das GameObject zum Kind des "LavaTile" machen
                transform.parent = hit.collider.transform;
                Debug.Log("new parent " + hit.collider.transform.name);
                StartCoroutine(hit.collider.transform.gameObject.GetComponent<LavaTileMovement>().ActivateFire());


                /*
                //neu test
                if (transform.gameObject.GetComponent<InputHandler>().moveValue.magnitude == 0)
                {
                    Debug.Log("Hit : " + hit.collider.name);
                    if (firstContact)
                    {
                        offset.x = transform.position.x - hit.transform.position.x;
                        offset.y = transform.position.z - hit.transform.position.z;
                        firstContact = false;
                    }
                    transform.position = new Vector3(hit.transform.position.x + offset.x, transform.position.y, hit.transform.position.z + offset.y);



                }
                else
                {
                    offset.x = transform.position.x - hit.transform.position.x;
                    offset.y = transform.position.z - hit.transform.position.z;
                    //Debug.Log("Hit : " + m_Hit.collider.name);
                    Vector3 direction = hit.transform.position - oldPos;
                    transform.gameObject.GetComponent<CharacterController>().Move(direction.normalized / 50);
                    oldPos = hit.transform.position;
                }
                */
            }
            else
            {
                // Das GameObject aus der Hierarchie des Vorgängers entfernen
                transform.parent = null;
                Debug.Log("new parent null");
            }
        }
        else
        {
            // Wenn kein Treffer erfolgt, das GameObject aus der Hierarchie des Vorgängers entfernen
            transform.parent = null;
        }
    }
}