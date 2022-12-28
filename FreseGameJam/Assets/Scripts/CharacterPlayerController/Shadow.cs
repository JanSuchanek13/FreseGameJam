using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    // Referenz auf das Spieler-GameObject
    public GameObject player;

    // Referenz auf das GameObject mit dem Decal Projector
    public GameObject shadowPlane;



    void Update()
    {
        Ray ray = new Ray(player.transform.position, Vector3.down);

        // Erstelle eine Variable, um das Ergebnis des Raycasts zu speichern
        RaycastHit hit;

        // Führe den Raycast aus und speichere das Ergebnis in der hit-Variable
        if (Physics.Raycast(ray, out hit))
        {
            // Wenn der Raycast einen Treffer hat, setze die Y-Position der Schattenplane auf den Y-Wert des Treffers
            shadowPlane.transform.position = new Vector3(player.transform.position.x, hit.point.y + 0.02f, player.transform.position.z);

            // Hole den Normalenvektor des Treffers (der zeigt nach oben vom Boden)
            Vector3 normal = hit.normal;

            // Berechne den Winkel des Bodens in Grad
            float angle = Vector3.Angle(normal, Vector3.up);

   


            // Setze die Rotation der Schattenplane auf eine neue Quaternion-Rotation, die auf den Winkel des Bodens abgestimmt ist
            shadowPlane.transform.rotation = Quaternion.Euler(0, 0, angle);

        }
    }
}


