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
    private void Start()
    {
        m_BaitButton.interactable = (IngredientStorage.PeekCount(m_FishInfo) != 0);
        m_BaitButton.GetComponent<Image>().sprite = m_FishInfo.m_FishOrderSprite;
        m_BaitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = IngredientStorage.PeekCount(m_FishInfo).ToString();
    }

    public void CheckButtonState()
    {
        m_BaitButton.interactable = (IngredientStorage.PeekCount(m_FishInfo) != 0);
        m_BaitButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = IngredientStorage.PeekCount(m_FishInfo).ToString();
    }
}
