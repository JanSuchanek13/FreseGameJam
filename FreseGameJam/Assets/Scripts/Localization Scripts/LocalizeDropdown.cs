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
    private TMP_Dropdown tmpDropdown;

    private void Awake()
    {
        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }
        if (!tmpDropdown) tmpDropdown = GetComponent<TMP_Dropdown>();
        tmpDropdown.options = tmpDropdownOptions;
    }

    Locale currentLocale;
    private void ChangedLocale(Locale newLocale)
    {
        if (currentLocale == newLocale) return;
        currentLocale = newLocale;
        List<TMP_Dropdown.OptionData> tmpDropdownOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < dropdownOptions.Count; i++)
        {
            tmpDropdownOptions.Add(new TMP_Dropdown.OptionData(dropdownOptions[i].GetLocalizedString()));
        }
        tmpDropdown.options = tmpDropdownOptions;
    }

    private void Update()
    {
        LocalizationSettings.SelectedLocaleChanged += ChangedLocale;
    }
}


/*using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Utilities.Localization
{
    [RequireComponent(typeof(TMP_Dropdown))]
    [AddComponentMenu("Localization/Localize dropdown")]
    public class LocalizeDropdown : MonoBehaviour
    {
        public List<LocalizedDropdownOption> options;
        public int selectedOptionIndex = 0;

        private TMP_Dropdown Dropdown => GetComponent<TMP_Dropdown>();

        private IEnumerator Start()
        {
            yield return PopulateDropdown();
        }

        private void OnEnable() => LocalizationSettings.SelectedLocaleChanged += UpdateDropdownOptions;
        private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;
        private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;

        private IEnumerator PopulateDropdown()
        {
            Dropdown.ClearOptions();
            Dropdown.onValueChanged.RemoveListener(UpdateSelectedOptionIndex);

            foreach (var option in options)
            {
                // Giving localizedText a default value.
                string localizedText = string.Empty;


                if (!option.text.IsEmpty && !string.IsNullOrEmpty(option.text.TableReference.TableCollectionName))
                {
                    var stringTask = option.text.GetLocalizedStringAsync();
                    yield return new WaitUntil(() => stringTask.IsDone);  // Wait until the asynchronous operation completes.
                    localizedText = stringTask.Result;
                }


                Sprite localizedSprite = null;

                if (option.sprite != null)
                {
                    var localizedSpriteHandle = option.sprite.LoadAssetAsync();
                    yield return localizedSpriteHandle;

                    localizedSprite = localizedSpriteHandle.Result;
                }

                Dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText, localizedSprite));

                if (options.IndexOf(option) == selectedOptionIndex)
                {
                    UpdateSelectedText(localizedText);
                    if (localizedSprite != null)
                    {
                        UpdateSelectedSprite(localizedSprite);
                    }
                }
            }

            Dropdown.value = selectedOptionIndex;
            Dropdown.onValueChanged.AddListener(UpdateSelectedOptionIndex);
        }

        private void UpdateDropdownOptions(Locale locale)
        {
            for (int i = 0; i < Dropdown.options.Count; i++)
            {
                var option = options[i];
                Dropdown.options[i].text = option.text.GetLocalizedString(locale);

                if (i == selectedOptionIndex)
                {
                    UpdateSelectedText(Dropdown.options[i].text);
                }

                // If there's a sprite associated, it's assumed the sprite won't change during runtime localization.
                // If this assumption is wrong, we'd need a similar mechanism to fetch localized sprites asynchronously.
            }
        }

        private void UpdateSelectedOptionIndex(int index) => selectedOptionIndex = index;
        private void UpdateSelectedText(string text) => Dropdown.captionText.text = text;
        private void UpdateSelectedSprite(Sprite sprite) => Dropdown.captionImage.sprite = sprite;
    }

    [Serializable]
    public class LocalizedDropdownOption
    {
        public LocalizedString text;
        public LocalizedSprite sprite;
    }
}*/


/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Utilities.Localization
{
    [RequireComponent(typeof(TMP_Dropdown))]
    [AddComponentMenu("Localization/Localize dropdown")]
    public class LocalizeDropdown : MonoBehaviour
    {
        // Fields
        // =======
        public List<LocalizedDropdownOption> options;

        public int selectedOptionIndex = 0;

        // Properties
        // ===========
        private TMP_Dropdown Dropdown => GetComponent<TMP_Dropdown>();

        // Methods
        // ========
        private IEnumerator Start()
        {
            yield return PopulateDropdown();
        }

        private void OnEnable() => LocalizationSettings.SelectedLocaleChanged += UpdateDropdownOptions;

        private void OnDisable() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;

        private void OnDestroy() => LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;

        private IEnumerator PopulateDropdown()
        {
            // Clear any options that might be present
            Dropdown.ClearOptions();
            Dropdown.onValueChanged.RemoveListener(UpdateSelectedOptionIndex);

            for (var i = 0; i < options.Count; ++i)
            {
                var option = options[i];
                var localizedText = string.Empty;
                Sprite localizedSprite = null;

                // If the option has text, fetch the localized version
                if (!option.text.IsEmpty)
                {
                    var localizedTextHandle = option.text.GetLocalizedString();
                    yield return localizedTextHandle;

                    localizedText = localizedTextHandle.Result;

                    // If this is the selected item, also update the caption text
                    if (i == selectedOptionIndex)
                    {
                        UpdateSelectedText(localizedText);
                    }
                }

                // If the option has a sprite, fetch the localized version
                if (!option.sprite.IsEmpty)
                {
                    var localizedSpriteHandle = option.sprite.LoadAssetAsync();
                    yield return localizedSpriteHandle;

                    localizedSprite = localizedSpriteHandle.Result;

                    // If this is the selected item, also update the caption text
                    if (i == selectedOptionIndex)
                    {
                        UpdateSelectedSprite(localizedSprite);
                    }
                }

                // Finally add the option with the localized content
                Dropdown.options.Add(new TMP_Dropdown.OptionData(localizedText, localizedSprite));
            }

            // Update selected option, to make sure the correct option can be displayed in the caption
            Dropdown.value = selectedOptionIndex;
            Dropdown.onValueChanged.AddListener(UpdateSelectedOptionIndex);
        }

        private void UpdateDropdownOptions(Locale locale)
        {
            // Updating all options in the dropdown
            // Assumes that this list is the same as the options passed on in the inspector window
            for (var i = 0; i < Dropdown.options.Count; ++i)
            {
                var optionI = i;
                var option = options[i];

                // Update the text
                if (!option.text.IsEmpty)
                {
                    var localizedTextHandle = option.text.GetLocalizedString(locale);
                    localizedTextHandle.Completed += (handle) =>
                    {
                        Dropdown.options[optionI].text = handle.Result;

                        // If this is the selected item, also update the caption text
                        if (optionI == selectedOptionIndex)
                        {
                            UpdateSelectedText(handle.Result);
                        }
                    };
                }

                // Update the sprite
                if (!option.sprite.IsEmpty)
                {
                    var localizedSpriteHandle = option.sprite.LoadAssetAsync();
                    localizedSpriteHandle.Completed += (handle) =>
                    {
                        Dropdown.options[optionI].image = localizedSpriteHandle.Result;

                        // If this is the selected item, also update the caption sprite
                        if (optionI == selectedOptionIndex)
                        {
                            UpdateSelectedSprite(localizedSpriteHandle.Result);
                        }
                    };
                }
            }
        }

        private void UpdateSelectedOptionIndex(int index) => selectedOptionIndex = index;

        private void UpdateSelectedText(string text)
        {
            if (Dropdown.captionText != null)
            {
                Dropdown.captionText.text = text;
            }
        }

        private void UpdateSelectedSprite(Sprite sprite)
        {
            if (Dropdown.captionImage != null)
            {
                Dropdown.captionImage.sprite = sprite;
            }
        }
    }

    [Serializable]
    public class LocalizedDropdownOption
    {
        public LocalizedString text;

        public LocalizedSprite sprite;
    }
}*/