using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public FishInfo m_CurrentBait;
    public bool m_FishHooked = false;

    [SerializeField]
    private float m_HookSpeed = 1.35f;
    [SerializeField]
    private float m_MaxHeight = 0f;
    [SerializeField]
    private float m_MinHeight = -2f;

    private float m_CurrentWeightHooked = 0f;
    private FishBehavior m_CurrentFish = null;

    [SerializeField]
    private ObjectSocket m_StorageSocket;
    [SerializeField]
    private GameObject m_TugOMeter;
    private void Update()
    {
        switch (m_FishHooked)
        {
            case false:
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    {
                        transform.localPosition += Vector3.up * IngredientStorage.HookSpeedBonus * m_HookSpeed * Time.deltaTime;
                        if (transform.localPosition.y > m_MaxHeight)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, m_MaxHeight);
                        }
                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        transform.localPosition -= Vector3.up * IngredientStorage.HookSpeedBonus * m_HookSpeed * Time.deltaTime;
                        if (transform.localPosition.y < m_MinHeight)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, m_MinHeight);
                        }
                    }
                }
                break;

            case true:
                {
                    if (m_CurrStrainPercent <= 0)
                    {
                        m_CurrentFish.Release();
                        m_HookSocket.RemoveObjs();
                        m_FishHooked = false;
                        SwitchBait(m_CurrentBait, true);
                        m_TugOMeter.SetActive(false);
                        break;
                    }
                    else if (m_CurrStrainPercent >= 1)
                    {
                        m_FishHooked = false;
                        foreach (var Fish in m_HookSocket.RemoveObjs())
                        {
                            Fish.GetComponent<FishBehavior>()?.GetCaught();
                            m_StorageSocket?.ForceStack(Fish);
                        }
                        foreach (var BB in FindObjectsOfType<BaitButton>())
                        {
                            BB.CheckButtonState();
                        }
                        SwitchBait(m_CurrentBait, true);
                        m_TugOMeter.SetActive(false);
                        break;
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
                    {
                        AddStrainPercent((1 - m_CurrentWeightHooked / 100f) / 4f * IngredientStorage.FishTugBonus);
                    }
                }
                break;
        }
    }

    private AudioSource m_AudioSource;
    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    [SerializeField]
    private Transform m_TugOMeterPercent;
    public float m_CurrStrainPercent;
    [SerializeField]
    private AudioClip m_ProgressBleep;
    public void AddStrainPercent(float p_StrainPercent)
    {
        m_CurrStrainPercent += p_StrainPercent;
        m_TugOMeterPercent.localScale = Vector3.one * Mathf.Max(m_CurrStrainPercent, 0f);
        m_AudioSource.pitch = m_CurrStrainPercent;
        m_AudioSource.PlayOneShot(m_ProgressBleep);
    }

    [SerializeField]
    private ObjectSocket m_HookSocket;
    public void CatchFish(FishBehavior p_FishObj, FishInfo p_CaughtFishInfo)
    {
        if (p_FishObj == null || p_CaughtFishInfo == null)
        {
            return;
        }

        m_FishHooked = true;
        m_CurrentFish = p_FishObj;
        m_CurrentWeightHooked = p_CaughtFishInfo.m_Weight;
        m_CurrStrainPercent = .4f;
        m_HookSocket?.ForceStack(p_FishObj.transform);
        m_TugOMeter.SetActive(true);
        m_TugOMeterPercent.localScale = Vector3.one * m_CurrStrainPercent;
    }


    [SerializeField]
    private FishInfo m_DefaultBait;
    [SerializeField]
    private SpriteRenderer m_BaitSprite;
    public void SwitchBait(FishInfo p_BaitInfo, bool p_Force = false)
    {
        if (m_FishHooked)
        {
            return;
        }


        if (!p_Force && m_CurrentBait.m_ID == p_BaitInfo.m_ID)
        {
            return;
        }
        if (p_BaitInfo != m_DefaultBait && IngredientStorage.PeekCount(p_BaitInfo) == 0)
        {
            m_CurrentBait = m_DefaultBait;
            m_BaitSprite.sprite = m_CurrentBait.m_FishOrderSprite;
            return;
        }
        if (m_CurrentBait == p_BaitInfo)
        {
            IngredientStorage.RemoveFishFromList(p_BaitInfo);
        }
        else
        {
            if (m_CurrentBait != m_DefaultBait)
            {
                IngredientStorage.AddFishToList(m_CurrentBait);
            }
            if (p_BaitInfo != m_DefaultBait)
            {
                IngredientStorage.RemoveFishFromList(p_BaitInfo);
            }
            m_CurrentBait = p_BaitInfo;
        }

        foreach (var BB in FindObjectsOfType<BaitButton>())
        {
            BB.CheckButtonState();
        }
        m_BaitSprite.sprite = m_CurrentBait.m_FishOrderSprite;
    }
}
