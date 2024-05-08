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

    public static void AddFishToList(FishInfo p_FishInfo)
    {
        if (m_FishCount.TryGetValue(p_FishInfo, out int Count))
        {
            m_FishCount[p_FishInfo] = Count + 1;
        }
        else
        {
            m_FishCount.Add(p_FishInfo, 1);
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
}
