using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    //ORDER MUST HAVE AT MOST ORDERSOCKETS COUNT OF FISH
    public FishInfo[] m_FishOrder;
    private int m_OrderIdx = 0;
    [SerializeField]
    private ObjectSocket m_OrderSocket;
    [SerializeField]
    private ObjectSocket m_SpawnSocket;
    [SerializeField]
    private ObjectSocket m_DisposeSocket;

    [SerializeField]
    private GameObject m_FishSpritePrefab;

    public void ShowOrderItem()
    {
        if (m_OrderIdx == m_FishOrder.Length || m_FishOrder[m_OrderIdx] == null)
        {
            return;
        }

        var OrderItem = GameObject.Instantiate(m_FishSpritePrefab, Vector3.zero, Quaternion.identity, m_SpawnSocket.transform);
        OrderItem.GetComponent<SpriteRenderer>().sprite = m_FishOrder[m_OrderIdx].m_FishOrderSprite;
        m_OrderSocket.Stack(OrderItem.transform);
    }

    //public int[] m_ItemPrices;
    [SerializeField]
    private Timer m_OrderTimer;
    public void ReceiveOrderItem()
    {
        //IngredientStorage.m_CurProfits += m_ItemPrices[m_OrderIdx];
        m_DisposeSocket.Stack(m_OrderSocket.RemoveObj());
        m_OrderTimer?.ResetTimer();
        m_OrderTimer?.Play();
        m_OrderIdx++;
    }
}
