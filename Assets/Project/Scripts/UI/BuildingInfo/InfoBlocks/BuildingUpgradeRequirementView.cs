using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradeRequirementView : MonoBehaviour
{
    [SerializeField] private Image checkmark;
    [SerializeField] private TMP_Text requirementText;

    public void UpdateDisplay(string text, bool met)
    {
        if (requirementText != null)
            requirementText.text = text;

        if (checkmark != null)
            checkmark.gameObject.SetActive(met);
    }
}