using TMPro;
using UnityEngine;

public class LocalizedTMPText : LocalizedComponent
{
    [SerializeField] private string localizationKey;
    private TMP_Text textComponent;

    private void Awake() => textComponent = GetComponent<TMP_Text>();
    private void Start()
    {
        RefreshLocalization();
    }

    protected override void RefreshLocalization()
    {
        if (textComponent != null && !string.IsNullOrEmpty(localizationKey))
            textComponent.text = Localization.GetString(localizationKey);
    }
}