using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuScript : MonoBehaviour
{
    [Header("UI Settings:")]
    [SerializeField] float _displayEpilepsyWarningTime = 5.0f;
    [SerializeField] GameObject _epilepsyWarningUI;

    /// <summary>
    /// Call the coroutine to close the epilepsy warning automatically, if the player has chosen a language before,
    /// thus never reaching the continue button anymore.
    /// </summary>
    private void Start()
    {
        if(PlayerPrefs.GetInt("PlayerPickedInitialLanguage", 0) == 1 && PlayerPrefs.GetInt("GameLaunched", 0) == 0)
        {
            PlayerPrefs.SetInt("GameLaunched", 1);
            CallEpilepsyWarning();
        }else
        {
            // start background music after closing warning:
            FindAnyObjectByType<BackgroundSoundPlayer>().PlayTrack();
        }
    }

    /// <summary>
    /// Reset the trigger for the epilepsy-warning UI.
    /// </summary>
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("GameLaunched", 0);
    }

    /// <summary>
    /// This gets called by the cofirm button of the initial language selection UI. This way
    /// the warning will be localized for the player.
    /// </summary>
    public void CallEpilepsyWarning()
    {
        StartCoroutine("DisplayEpilepsyWarning");
    }

    IEnumerator DisplayEpilepsyWarning()
    {
        // open epilepsy warning on load/call:
        _epilepsyWarningUI.SetActive(true);


        yield return new WaitForSeconds(_displayEpilepsyWarningTime);

        // close epilepsy warning after x seconds:
        _epilepsyWarningUI.SetActive(false);

        // start background music after closing warning:
        FindAnyObjectByType<BackgroundSoundPlayer>().PlayTrack();
    }


    public void PlayGameLevel()
    {
        SceneManager.LoadScene(1);
    }


    public void Quit()
    {
        Debug.Log ("Winners never Quit");
        Application.Quit();
    }
}
