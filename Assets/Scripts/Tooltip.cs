using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    private Text tooltipText;
    private RectTransform backgroundTransform;

    private void Awake()
    {
        backgroundTransform = transform.Find("Background").GetComponent<RectTransform>();
        tooltipText = transform.Find("text").GetComponent<Text>();

        ShowTooltip("Random Tooltip Text");
    }
    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);

        tooltipText.text = tooltipString;
        float textPadddingSize = 4f;
        Vector2 backgroundSize = new Vector2(tooltipText.preferredWidth + textPadddingSize * 2f, tooltipText.preferredHeight + textPadddingSize * 2f);
        backgroundTransform.sizeDelta = backgroundSize;
    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
