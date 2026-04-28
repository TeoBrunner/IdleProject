using UnityEngine;

public class RainbowCrystall : MonoBehaviour, IInteractable
{
    public void OnPlayerEnter()
    {
        transform.localScale += Vector3.one * 0.5f;
    }
    public void OnPlayerExit()
    {
        transform.localScale -= Vector3.one * 0.5f;
    }
    public void OnInteract()
    {
        float newHSV = Random.Range(0f, 1f);
        Color newColor = Color.HSVToRGB(newHSV, 1f, 1f);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = newColor;
    }
    public void OnExamine() { }
}
