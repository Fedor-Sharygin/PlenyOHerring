using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerBehavior : MonoBehaviour
{
    //ORDER MUST HAVE AT MOST ORDERSOCKETS COUNT OF FISH
    public FishInfo[] m_FishOrder;
    private int m_OrderIdx = 0;
    private bool m_ShowingItem = false;
    public FishInfo CurItem
    {
        get
        {
             if (!m_ShowingItem || m_OrderIdx >= m_FishOrder.Length)
            {
                return null;
            }

            return m_FishOrder[m_OrderIdx];
        }
    }
    [SerializeField]
    private ObjectSocket m_OrderSocket;
    [SerializeField]
    private ObjectSocket m_SpawnSocket;
    [SerializeField]
    private ObjectSocket m_DisposeSocket;

    [SerializeField]
    private GameObject m_FishSpritePrefab;

    private bool m_Done = false;
    public void TriggerNextClient()
    {
        if (!m_Done)
        {
            return;
        }

        Destroy(gameObject);
    }

    public void ShowOrderItem()
    {
        if (m_OrderIdx == m_FishOrder.Length || m_FishOrder[m_OrderIdx] == null)
        {
            m_Done = true;
            GetComponent<Animator>().SetTrigger("Leave");
            return;
        }

        var OrderItem = GameObject.Instantiate(m_FishSpritePrefab, Vector3.zero, Quaternion.identity, m_SpawnSocket.transform);
        OrderItem.GetComponent<SpriteRenderer>().sprite = m_FishOrder[m_OrderIdx].m_FishOrderSprite;
        m_OrderSocket.Stack(OrderItem.transform);
        m_ShowingItem = true;
    }

    private AudioSource m_AudioSource;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    //public int[] m_ItemPrices;
    [SerializeField]
    private Timer m_OrderTimer;
    [SerializeField]
    private AudioClip[] m_VoiceLines;
    public void ReceiveOrderItem()
    {
        //IngredientStorage.m_CurProfits += m_ItemPrices[m_OrderIdx];
        m_DisposeSocket.Stack(m_OrderSocket.RemoveObj());
        m_OrderTimer?.ResetTimer();
        m_OrderTimer?.Play();
        m_OrderIdx++;
        m_ShowingItem = false;
        m_AudioSource.PlayOneShot(m_VoiceLines[Random.Range(0, m_VoiceLines.Length)]);
    }
}
