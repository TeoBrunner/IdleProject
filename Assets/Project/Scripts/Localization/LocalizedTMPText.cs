using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedTMPText : MonoBehaviour
{
    [SerializeField] private string localizationKey;
    private LocalizationProvider localizationProvider;

    private TMP_Text textComponent;

    void Awake()
    {
        textComponent = GetComponent<TMP_Text>();
    }

    void Start()
    {
        localizationProvider = ServiceLocator.Get<LocalizationProvider>();
        if(localizationProvider)
        {
            UpdateText();
            localizationProvider.LocalizationUpdated += UpdateText;
        }
    }

    void OnEnable()
    {
        if (!localizationProvider)
            return;

        localizationProvider.LocalizationUpdated += UpdateText;
        UpdateText();
    }

    void OnDisable()
    {
        if (!localizationProvider)
            return;

        localizationProvider.LocalizationUpdated -= UpdateText;
    }

    private void UpdateText()
    {
        if (!textComponent)
            return;

        if (string.IsNullOrEmpty(localizationKey))
            return;

        if(!localizationProvider)
            localizationProvider = ServiceLocator.Get<LocalizationProvider>();

        textComponent.text = localizationProvider.GetString(localizationKey);
    }
}