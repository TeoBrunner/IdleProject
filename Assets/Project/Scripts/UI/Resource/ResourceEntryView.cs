using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ResourceEntryView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public ResourceType Resource { get; private set; }

    public void Initialize(ResourceType resource, int initialAmount)
    {
        Resource = resource;

        //icon.sprite  = resource.Icon;
        //icon.enabled = resource.Icon != null;

        UpdateAmount(initialAmount);
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
