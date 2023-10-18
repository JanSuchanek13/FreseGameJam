using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public GameObject hoverSoundObject;
    public GameObject selectSoundObject;

    private AudioSource audioSource;
    private bool isHovered = false;

    private void Start()
    {
        audioSource = hoverSoundObject.GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isHovered)
        {
            isHovered = true;
            PlaySound(hoverSoundObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (IsControllerInput())
        {
            PlaySound(hoverSoundObject);
        }
    }

    public void OnDeselect(BaseEventData eventData)
    {
        // Hier kannst du bei Bedarf Aktionen für die Deselektion hinzufügen
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsControllerInput())
        {
            PlaySound(selectSoundObject);
        }
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

    private bool IsControllerInput()
    {
        string[] joystickNames = Input.GetJoystickNames();
        foreach (string name in joystickNames)
        {
            if (!string.IsNullOrEmpty(name))
            {
                // Wenn ein Controller erkannt wird, gehe davon aus, dass Controller-Eingaben vorliegen
                return true;
            }
        }
        return true; // Passe dies entsprechend deiner Controller-Logik an
    }
}
