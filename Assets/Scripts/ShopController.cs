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
            m_FishEntries[i].SetCount(IngredientStorage.PeekCount(IngredientStorage.FishArray[i]));
            m_FishEntries[i].SetPrice(m_CurrentPrices[i]);
            if (IngredientStorage.PeekCount(m_FishArray[i]) == 0)
            {
                m_FishEntries[i].DisableEntry();
            }
        }
    }


    public void IncreasePrice(int p_Idx)
    {
        ChangePrice(p_Idx, 1);
    }
    public void DecreasePrice(int p_Idx)
    {
        ChangePrice(p_Idx, -1);
    }
    private void ChangePrice(int p_Idx, int p_Change)
    {
        int MinPrice = Mathf.FloorToInt(.7f * (float)IngredientStorage.FishArray[p_Idx].m_RecommendedCost);
        int MaxPrice = Mathf.CeilToInt(1.3f * (float)IngredientStorage.FishArray[p_Idx].m_RecommendedCost);

        int TempPrice = m_CurrentPrices[p_Idx] + p_Change;
        if (TempPrice >= MinPrice && TempPrice <= MaxPrice)
        {
            m_CurrentPrices[p_Idx] = TempPrice;
            m_FishEntries[p_Idx].SetPrice(TempPrice);
        }
    }

    public GameObject m_CurCustomer = null;
    private int m_CurItemIdx = -1;

    [SerializeField]
    private GameObject m_FishPrefab;
    [SerializeField]
    private ObjectSocket m_ItemSpawnSocket;
    [SerializeField]
    private ObjectSocket m_TableSocket;
    public void TakeFish(int p_Idx)
    {
        if (m_CurCustomer == null || m_ItemState != -1)
        {
            return;
        }

        if (!IngredientStorage.RemoveFishFromList(IngredientStorage.FishArray[p_Idx]))
        {
            return;
        }

        //SPAWN THE FISH TO THE TABLE
        var FishItem = GameObject.Instantiate(m_FishPrefab, Vector3.zero, Quaternion.identity, m_ItemSpawnSocket.transform);
        m_TableSocket.Stack(m_ItemSpawnSocket.RemoveObj());
        m_ItemState = 0;
        m_FishEntries[p_Idx].SetCount(IngredientStorage.PeekCount(IngredientStorage.FishArray[p_Idx]));
        m_CurItemIdx = p_Idx;

        if (IngredientStorage.PeekCount(IngredientStorage.FishArray[p_Idx]) != 0)
        {
            return;
        }
        m_FishEntries[p_Idx].DisableEntry();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdateItemState();
        }
    }

    private int m_ItemState = -1;
    [SerializeField]
    private ObjectSocket m_CustomerSocket;
    private void UpdateItemState()
    {
        if (m_ItemState == -1)
        {
            return;
        }

        if (m_ItemState == 2)
        {
            m_CustomerSocket.Stack(m_TableSocket.RemoveObj());
            m_ItemState = -1;
            m_CurCustomer.GetComponent<CustomerBehavior>().ReceiveOrderItem();
            IngredientStorage.m_CurProfits += m_CurrentPrices[m_CurItemIdx];
            return;
        }
        m_ItemState++;
    }
}
