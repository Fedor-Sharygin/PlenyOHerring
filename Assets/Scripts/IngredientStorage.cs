using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStorage
{
    private static Dictionary<FishInfo, int> m_FishCount = new Dictionary<FishInfo, int>();
    public static int FishTypeCount
    {
        get
        {
            return m_FishCount.Count;
        }
    }
    public static FishInfo[] FishArray
    {
        get
        {
            FishInfo[] FishKeys = new FishInfo[m_FishCount.Count];
            m_FishCount.Keys.CopyTo(FishKeys, 0);
            return FishKeys;
        }
    }

    private static bool m_Initialized = false;
    public static void InitializeStorage(FishInfo[] p_Fishes)
    {
        if (m_Initialized)
        {
            return;
        }

        foreach (var FI in p_Fishes)
        {
            m_FishCount.Add(FI, 0);
        }

        m_Initialized = true;
    }

    public static void AddFishToList(FishInfo p_FishInfo, int p_Count = 1)
    {
        if (m_FishCount.TryGetValue(p_FishInfo, out int Count))
        {
            m_FishCount[p_FishInfo] = Count + p_Count;
        }
        else
        {
            m_FishCount.Add(p_FishInfo, p_Count);
        }
    }

    public static bool RemoveFishFromList(FishInfo p_FishInfo)
    {
        if (!m_FishCount.ContainsKey(p_FishInfo) || m_FishCount[p_FishInfo] <= 0)
        {
            return false;
        }

        m_FishCount[p_FishInfo]--;
        return true;
    }

    public static int PeekCount(FishInfo p_FishInfo) => m_FishCount[p_FishInfo];


    //DAILY INFO/BONUSES
    public delegate void GameOverDelegate();
    public static event GameOverDelegate m_GameOverEvent;

    public static int m_CurDay = 0;
    public static float m_CurQuota = 0f;
    public static int m_CurProfits = 0;
    public static void UpdateQuota()
    {
        ++m_CurDay;
        m_CurProfits -= Mathf.FloorToInt(m_CurQuota);
        if (m_CurProfits < 0)
        {
            m_CurDay = 0;
            m_CurQuota = 0f;
            m_CurProfits = 0;

            foreach (var FI in FishArray)
            {
                m_FishCount[FI] = 0;
            }

            m_GameOverEvent();
            return;
        }
        m_CurQuota += m_CurDay * Random.Range(50f, 100f);
        ResetBonuses();
    }

    public static void ResetBonuses()
    {
        HookSpeedBonus = 1f;
        CustomerPayBonus = 1f;
        FishSpawnBonus = 1f;
        FishTugBonus = 1f;
    }

    public static float HookSpeedBonus { get; private set; } = 1f;
    public static void IncreaseHookSpeedBonus()
    {
        HookSpeedBonus *= 1.2f;
    }

    public static float CustomerPayBonus { get; private set; } = 1f;
    public static void IncreaseCustomerPayBonus()
    {
        CustomerPayBonus *= 1.1f;
    }

    public static float FishSpawnBonus { get; private set; } = 1f;
    public static void IncreaseFishSpawnBonus()
    {
        FishSpawnBonus *= .9f;
    }

    public static float FishTugBonus { get; private set; } = 1f;
    public static void IncreaseFishTugBonus()
    {
        FishTugBonus *= 1.5f;
    }
}
