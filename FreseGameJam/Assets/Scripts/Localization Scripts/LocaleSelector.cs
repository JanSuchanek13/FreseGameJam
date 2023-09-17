using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocaleSelector : MonoBehaviour
{
    // references:
    // https://docs.unity3d.com/Packages/com.unity.localization@1.0/manual/LocalizedPropertyVariants.html
    // https://forum.unity.com/threads/localizing-ui-dropdown-options.896951/

    [Tooltip("This UI will only be called if the player has never picked an initial language.")]
    [SerializeField] GameObject _languageSelectorUI;

    [SerializeField] TMP_Dropdown[] _arrayOfLanguageDropDowns;

    private bool _changingLocale = false;

    private void Start()
    {
        // if this is the first time loading the game, have player select their preferred language:
        if(PlayerPrefs.GetInt("PlayerPickedInitialLanguage", 0) != 0 || _languageSelectorUI == null)
        {
            CloseLanguageSelectorUI();
        }

        int _savedLanguageID = PlayerPrefs.GetInt("LanguageID", 0); // language/locale ID nr 0 is English currently.
        ChangeLocale(_savedLanguageID);
        Debug.Log("current language ID: " + _savedLanguageID);


        if (_arrayOfLanguageDropDowns == null || _arrayOfLanguageDropDowns.Length == 0)
        {
            Debug.LogWarning("Dropdowns array is null or empty!");
            return;
        }else
        {
            foreach (TMP_Dropdown dropdown in _arrayOfLanguageDropDowns)
            {
                dropdown.value = _savedLanguageID;
            }
        }
    }

    public void ChangeLocale(int localeID)
    {
        if(_changingLocale)
        {
            return;
        }
        StartCoroutine(SetLocale(localeID));
    }

    IEnumerator SetLocale(int _localeID)
    {
        _changingLocale = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        PlayerPrefs.SetInt("LanguageID", _localeID);
        _changingLocale = false;

        Debug.Log("changed language ID: " + _localeID);
    }

    void CloseLanguageSelectorUI()
    {
        if(_languageSelectorUI != null)
        {
            _languageSelectorUI.SetActive(false);
        }
    }

    /// <summary>
    /// By having the player pick a language and then hitting continue we avoid issues of players closing the game
    /// while in the selection UI etc. So the primary selection UI will be called every time, until the player
    /// makes a concious decision and picks a language.
    /// --> Called by button in the UI.
    /// </summary>
    public void PreferredLanguageSelected()
    {
        PlayerPrefs.SetInt("PlayerPickedInitialLanguage", 1);
    }

    /// <summary>
    /// Reset to test the initial language selection UI:
    /// </summary>
    public void ResetLanguageUI()
    {
        //StartCoroutine(SetLocale(0));
        PlayerPrefs.SetInt("PlayerPickedInitialLanguage", 0);
    }
}
