using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public GameObject hoverSoundObject;
    public GameObject selectSoundObject;

    private AudioSource audioSource;
    private bool isHovered = false;
    private bool isSelected = false;

    private void Start()
    {
        // Hier wird die AudioSource-Komponente des hoverSoundObject abgerufen
        audioSource = hoverSoundObject.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovered)
        {
            isHovered = true;
            // Hier wird das Audiofeedback beim Hovern abgespielt
            PlaySound(hoverSoundObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!isSelected)
        {
            isSelected = true;
            // Hier wird das Audiofeedback bei der Auswahl abgespielt
            PlaySound(selectSoundObject);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
    }

    private void PlaySound(GameObject soundObject)
    {
        if (soundObject != null)
        {
            AudioSource soundSource = soundObject.GetComponent<AudioSource>();
            if (soundSource != null && soundSource.clip != null)
            {
                soundSource.PlayOneShot(soundSource.clip);
            }
        }
    }
}
