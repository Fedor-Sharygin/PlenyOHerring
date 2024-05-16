using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayManager : MonoBehaviour
{

    private static bool m_SubscribedToGameOver = false;
    private static void SubscribeToGameOver()
    {
        if (m_SubscribedToGameOver)
        {
            return;
        }

        IngredientStorage.m_GameOverEvent += QuotaNotMetActions;
        m_SubscribedToGameOver = true;
    }
    private static void QuotaNotMetActions()
    {
        SceneManager.LoadScene("BlackHoleGameOver");
    }

    [SerializeField]
    private FishInfo[] m_Infos;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_DayText;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_QuotaText;
    private void Awake()
    {
        SubscribeToGameOver();
        IngredientStorage.InitializeStorage(m_Infos);
        IngredientStorage.UpdateQuota();

        m_DayText.text += IngredientStorage.m_CurDay.ToString();
        m_QuotaText.text += Mathf.FloorToInt(IngredientStorage.m_CurQuota).ToString();
    }

    private void OnDestroy()
    {
        
    }

    private Dictionary<FishInfo, int> m_FishCount = new Dictionary<FishInfo, int>();

    public void UseFish(FishInfo p_Fish)
    {
        IngredientStorage.RemoveFishFromList(p_Fish);

        if (!m_FishCount.ContainsKey(p_Fish)) {
            m_FishCount.Add(p_Fish, 0);
        }
        m_FishCount[p_Fish]++;
    }

    public void ResetBonuses()
    {
        foreach (var FIC in m_FishCount)
        {
            IngredientStorage.AddFishToList(FIC.Key, FIC.Value);
        }
        foreach (var FI in IngredientStorage.FishArray)
        {
            m_FishCount[FI] = 0;
        }
        IngredientStorage.ResetBonuses();
    }

    public void IncreaseHookSpeedBonus()
    {
        IngredientStorage.IncreaseHookSpeedBonus();
    }
    public void IncreaseFishSpawnBonus()
    {
        IngredientStorage.IncreaseFishSpawnBonus();
    }
    public void IncreaseFishTugBonus()
    {
        IngredientStorage.IncreaseFishTugBonus();
    }
    public void IncreaseCustomerPayBonus()
    {
        IngredientStorage.IncreaseCustomerPayBonus();
    }


    public void ProceedWithDay()
    {
        SceneManager.LoadScene("FishingLevel");
    }

    public void GiveUp()
    {
        IngredientStorage.m_CurProfits = -Mathf.FloorToInt(IngredientStorage.m_CurQuota);
        IngredientStorage.UpdateQuota();
        //SceneManager.LoadScene("BlackHoleGameOver");
    }
}
