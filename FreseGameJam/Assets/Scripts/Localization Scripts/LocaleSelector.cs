using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LocaleSelector : MonoBehaviour
{

    private bool _changingLocale = false;

    private void Start()
    {
        int _savedLanguageID = PlayerPrefs.GetInt("LanguageID", 3); // language/locale ID nr 3 is English currently.
        ChangeLocale(_savedLanguageID);
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
    }
}
