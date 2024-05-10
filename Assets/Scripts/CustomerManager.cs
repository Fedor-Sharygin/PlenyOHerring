using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Timer m_DayTimer;
    [Space(10)]
    [SerializeField]
    private Timer m_CustomerSpawnTimer;
    [SerializeField]
    private ObjectSocket m_CustomerPosition;
    [SerializeField]
    private GameObject[] m_CustomerPrefabs;
    [Space(10)]
    [SerializeField]
    private FishInfo[] m_FishArray;
    private float[] m_CurFishSpawnWeights;
    [SerializeField]
    private ShopController m_CurShopController;

    private void Start()
    {
        m_CurFishSpawnWeights = new float[m_FishArray.Length];
    }

    private bool m_CustomerWasOnScreen = false;
    public void SpawnCustomer()
    {
        if (!m_DayTimer.IsPlaying())
        {
            return;
        }

        float TotalWeight = 0;
        for (int i = 0; i < m_FishArray.Length; ++i)
        {
            TotalWeight += (m_CurFishSpawnWeights[i]
                = IngredientStorage.PeekCount(m_FishArray[i]) * Mathf.Floor(.75f *(200 - ShopController.CurrentPrices[i])));
        }
        var Customer = GameObject.Instantiate(m_CustomerPrefabs[Random.Range(0, m_CustomerPrefabs.Length)],
            Vector3.zero, Quaternion.identity, m_CustomerPosition.transform);
        var CB = Customer.GetComponent<CustomerBehavior>();
        int OrderLength = Random.Range(3, 6);
        CB.m_FishOrder = new FishInfo[OrderLength];
        //CB.m_ItemPrices = new int[OrderLength];

        for (int i = 0; i < OrderLength; ++i)
        {
            if (TotalWeight <= 0.005f)
            {
                break;
            }

            float FishItemWeight = Random.Range(0f, TotalWeight);
            for (int j = 0; j < m_FishArray.Length; ++j)
            {
                FishItemWeight -= m_CurFishSpawnWeights[j];
                if (FishItemWeight > 0f)
                {
                    continue;
                }

                CB.m_FishOrder[i] = m_FishArray[j];
                //CB.m_ItemPrices[i] = ShopController.CurrentPrices[j];
                float Decr = Mathf.Floor(.75f * (200 - ShopController.CurrentPrices[j]));
                m_CurFishSpawnWeights[j] -= Decr;
                TotalWeight -= Decr;
            }
        }
        m_CurShopController.m_CurCustomer = Customer;
        m_CustomerWasOnScreen = true;
    }

    private bool m_FishingLoading = false;
    public void SetLoadState()
    {
        m_FishingLoading = true;
    }
    private void Update()
    {
        if (m_CustomerWasOnScreen && !m_FishingLoading && m_CustomerPosition.AvailableForStack)
        {
            m_CustomerSpawnTimer.ResetTimer();
            m_CustomerSpawnTimer.Play();
            m_CustomerWasOnScreen = false;
        }

        if (!m_FishingLoading || !m_CustomerPosition.AvailableForStack)
        {
            return;
        }

        SceneManager.LoadScene("FishingLevel");
    }
}
