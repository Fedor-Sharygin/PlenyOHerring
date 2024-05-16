using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI m_QuotaText;
    private void Awake()
    {
        m_QuotaText.text = Mathf.FloorToInt(IngredientStorage.m_CurQuota).ToString();
    }

    [SerializeField]
    private PlayerHook m_Hook;
    public void SwitchBait(FishInfo p_FishInfo)
    {
        m_Hook.SwitchBait(p_FishInfo);
    }
}
