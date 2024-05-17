using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitButton : MonoBehaviour
{
    [SerializeField]
    private Button m_BaitButton;
    [SerializeField]
    private FishInfo m_FishInfo;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_HighlightText;
    private void Start()
    {
        m_BaitButton.interactable = (IngredientStorage.PeekCount(m_FishInfo) != 0);
        m_BaitButton.GetComponent<Image>().sprite = m_FishInfo.m_FishOrderSprite;
        m_BaitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "x" + IngredientStorage.PeekCount(m_FishInfo).ToString();
        m_HighlightText.color = m_FishInfo.m_TextColor;
    }

    public void CheckButtonState()
    {
        m_BaitButton.interactable = (IngredientStorage.PeekCount(m_FishInfo) != 0);
        m_BaitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "x" + IngredientStorage.PeekCount(m_FishInfo).ToString();
    }
}
