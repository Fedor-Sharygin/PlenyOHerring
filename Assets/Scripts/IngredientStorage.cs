using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientStorage
{
    private static Dictionary<FishInfo, int> m_FishCount = new Dictionary<FishInfo, int>();

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
}
