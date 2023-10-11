using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization;          // This is the necessary namespace.
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Components;  // Namespace for LocalizeStringEvent

public class LocalizeFonts : MonoBehaviour
{
    [System.Serializable]
    public class LocaleFontPair
    {
        [Tooltip("Name of the language (e.g., English, Chinese, etc.)")]
        public string LanguageName;       // Descriptive name for the language.)]
        public string LocaleIdentifier;  // E.g., "en", "zh", etc.
        public TMP_FontAsset Font;       // Drag & drop the desired font for this locale in the Inspector.
    }

    [SerializeField] private TMP_FontAsset _defaultFont;  // The default font.
    [SerializeField] private List<LocaleFontPair> fontMapping;

    private List<TMP_Text> localizedTextFields;

    private void Start()
    {
        // Fetching TMP_Text components with LocalizeStringEvent
        var localizedStringEvents = Resources.FindObjectsOfTypeAll<LocalizeStringEvent>();
        localizedTextFields = new List<TMP_Text>();
        foreach (var localizeEvent in localizedStringEvents)
        {
            TMP_Text tmpText = localizeEvent.GetComponent<TMP_Text>();
            if (tmpText)
            {
                localizedTextFields.Add(tmpText);
            }
        }

        UpdateFontsForCurrentLocale();

        // Listen for changes in selected locale.
        LocalizationSettings.SelectedLocaleChanged += UpdateFontsForChangedLocale;
    }

    private void UpdateFontsForCurrentLocale()
    {
        string currentLocaleIdentifier = LocalizationSettings.SelectedLocale.Identifier.Code;
        TMP_FontAsset desiredFont = GetFontForLocale(currentLocaleIdentifier);

        foreach (var textField in localizedTextFields)
        {
            Debug.Log("Setting font: " + desiredFont.name);

            textField.font = desiredFont;
        }
    }

    public TMP_FontAsset GetFontForLocale(string localeIdentifier)
    {
        foreach (var pair in fontMapping)
        {
            if (pair.LocaleIdentifier == localeIdentifier)
            {
                return pair.Font ? pair.Font : _defaultFont; // If font is assigned, use that. Else, use default font.
            }
        }
        return _defaultFont;  // If no match is found, return the default font.
    }

    private void UpdateFontsForChangedLocale(Locale newLocale)
    {
        Debug.Log("Locale changed to: " + newLocale.Identifier.Code);

        UpdateFontsForCurrentLocale();
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= UpdateFontsForChangedLocale; // Cleanup.
    }
}
