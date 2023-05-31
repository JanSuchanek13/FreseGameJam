using UnityEngine;

public class PulsatingObject : MonoBehaviour
{
    public float pulsationSpeed = 1f; // Geschwindigkeit der Pulsation
    public float scaleFactor = 0.01f; // Prozentsatz der Größenänderung (1%)

    private Vector3 startingScale; // Ursprüngliche Skalierung des Objekts

    private void Start()
    {
        startingScale = transform.localScale;
    }

    private void Update()
    {
        // Berechne die aktuelle Skalierungsfaktoren
        float scaleAmount = Mathf.Sin(Time.time * pulsationSpeed) * scaleFactor;
        Vector3 newScale = new Vector3(startingScale.x + startingScale.x * scaleAmount,
                                       startingScale.y + startingScale.y * scaleAmount,
                                       transform.localScale.z);

        // Wende die Skalierung auf das Objekt an
        transform.localScale = newScale;
    }
}
