using UnityEngine;
using UnityEngine.UI;

public class TextAlphaSync : MonoBehaviour
{
    private Text textComponent;
    private Image parentImage;

    void Awake()
    {
        textComponent = GetComponent<Text>();
        parentImage = GetComponentInParent<Image>();
    }

    void Update()
    {
        if (textComponent != null && parentImage != null)
        {
            Color textColor = textComponent.color;
            textColor.a = parentImage.color.a;
            textComponent.color = textColor;
        }
    }
}
