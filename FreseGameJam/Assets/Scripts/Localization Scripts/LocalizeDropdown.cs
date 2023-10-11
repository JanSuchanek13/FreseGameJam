using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocalizeDropdown : MonoBehaviour
{
    // references:
    // https://docs.unity3d.com/Packages/com.unity.localization@1.0/manual/LocalizedPropertyVariants.html
    // https://forum.unity.com/threads/localizing-ui-dropdown-options.896951/

    [SerializeField] private List<LocalizedString> dropdownOptions;
    [Tooltip("Add the 'Item Label' from the Template inside the dropdown here:")]
    [SerializeField] TextMeshProUGUI _TemplateTMP; // used to change fonts

    private TMP_Dropdown tmpDropdown;
    Locale currentLocale;

    private void Awake()
    {
        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }

        if (!tmpDropdown) tmpDropdown = GetComponent<TMP_Dropdown>();
        tmpDropdown.options = tmpDropdownOptions;

        ChangeFont();
    }

    private void ChangedLocale(Locale newLocale)
    {
        if (currentLocale == newLocale) return;

        ChangeFont();
        currentLocale = newLocale;

        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }
        tmpDropdown.options = tmpDropdownOptions;
    }

    void ChangeFont()
    {
        // change fonts of option items:
        string _currentLocaleIdentifier = LocalizationSettings.SelectedLocale.Identifier.Code;
        _TemplateTMP.font = FindAnyObjectByType<LocalizeFonts>().GetFontForLocale(_currentLocaleIdentifier);
    }

    private void Update()
    {
        LocalizationSettings.SelectedLocaleChanged += ChangedLocale;
    }
}