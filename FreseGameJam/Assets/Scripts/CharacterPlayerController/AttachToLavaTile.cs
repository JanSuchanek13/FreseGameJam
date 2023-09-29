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

        // Ein SphereCast nach unten durchf�hren
        RaycastHit hit;
        if (Physics.SphereCast(rayOrigin, sphereRadius, Vector3.down, out hit, 1.0f))
        {
            // �berpr�fen, ob das getroffene Objekt das Tag "LavaTile" hat
            if (hit.collider.CompareTag("LavaTile"))
            {
                // Das GameObject zum Kind des "LavaTile" machen
                transform.parent = hit.collider.transform;
                //Debug.Log("new parent " + hit.collider.transform.name);
                StartCoroutine(hit.collider.transform.gameObject.GetComponent<LavaTileMovement>().ActivateFire());


            }
            else
            {
                // Das GameObject aus der Hierarchie des Vorg�ngers entfernen
                transform.parent = null;
                //Debug.Log("new parent null");
            }
        }
        else
        {
            // Wenn kein Treffer erfolgt, das GameObject aus der Hierarchie des Vorg�ngers entfernen
            transform.parent = null;
        }
    }
}