using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishEntryHolder : MonoBehaviour
{
    [SerializeField]
    private Button m_FishButton;
    [SerializeField]
    private Button m_PriceIncreaseButton;
    [SerializeField]
    private Button m_PriceDecreaseButton;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_CountText;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_PriceText;

    public void SetButtonSprite(Sprite p_FishSprite)
    {
        m_FishButton.GetComponent<Image>().sprite = p_FishSprite;
    }

    public void SetCount(int p_NewCount)
    {
        m_CountText.text = "x " + p_NewCount.ToString();
    }
    public void SetPrice(int p_NewPrice)
    {
        m_PriceText.text = p_NewPrice.ToString();
    }

    public void DisableEntry()
    {
        m_FishButton.interactable = false;
        m_PriceIncreaseButton.interactable = false;
        m_PriceDecreaseButton.interactable = false;
    }
}
