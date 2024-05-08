using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    private static bool m_Initialized = false;
    private static int[] m_CurrentPrices;

    private static void InitializeShopPrices()
    {
        if (m_Initialized)
        {
            return;
        }

        m_CurrentPrices = new int[IngredientStorage.FishTypeCount];
        for (int i = 0; i < IngredientStorage.FishArray.Length; ++i)
        {
            m_CurrentPrices[i] = IngredientStorage.FishArray[i].m_RecommendedCost;
        }
        m_Initialized = true;
    }

    [SerializeField]
    private FishInfo[] m_FishArray;
    [SerializeField]
    private FishEntryHolder[] m_FishEntries;
    private void Awake()
    {
        IngredientStorage.InitializeStorage(m_FishArray);
        InitializeShopPrices();

        for (int i = 0; i < m_FishEntries.Length; ++i)
        {
            m_FishEntries[i].SetButtonSprite(m_FishArray[i].m_FishOrderSprite);
        }
    }

}
