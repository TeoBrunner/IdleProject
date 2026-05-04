using Mono.Cecil;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ResourceEntryView : LocalizedComponent
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI resourceText;
    [SerializeField] private TextMeshProUGUI amountText;

    public ResourceType Resource { get; private set; }

    public void Initialize(ResourceType resource, float initialAmount)
    {
        Resource = resource;

        //icon.sprite  = resource.Icon;
        //icon.enabled = resource.Icon != null;
        resourceText.text = Localization.GetString(Resource.ToString().ToLower());

        UpdateAmount(initialAmount);
    }

    public void UpdateAmount(float amount)
    {
        amountText.text = amount.ToString();
    }

    protected override void RefreshLocalization()
    {
        resourceText.text = Localization.GetString(Resource.ToString().ToLower());
    }
}
