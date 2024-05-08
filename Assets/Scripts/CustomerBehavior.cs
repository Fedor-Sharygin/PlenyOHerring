using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    //ORDER MUST HAVE AT MOST ORDERSOCKETS COUNT OF FISH
    [SerializeField]
    private FishInfo[] m_FishOrder;
    [SerializeField]
    private ObjectSocket[] m_OrderSockets;

    [SerializeField]
    private GameObject m_FishSpritePrefab;

    public void ShowOrder()
    {
        for (int i = 0; i < m_FishOrder.Length; ++i)
        {
            var OrderedFish = GameObject.Instantiate(m_FishSpritePrefab, Vector3.zero, Quaternion.identity, transform);
            OrderedFish.GetComponent<SpriteRenderer>().sprite = m_FishOrder[i].m_FishOrderSprite;
            m_OrderSockets[i].Stack(OrderedFish.transform);
        }
    }
}
