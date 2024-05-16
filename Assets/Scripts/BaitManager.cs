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


    [SerializeField]
    private FishInfo[] m_Infos;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchBait(m_Infos[0]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchBait(m_Infos[1]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchBait(m_Infos[2]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchBait(m_Infos[3]);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SwitchBait(m_Infos[4]);
        }
    }
}
