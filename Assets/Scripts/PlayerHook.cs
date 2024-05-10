using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public FishInfo m_CurrentBait;
    public bool m_FishHooked = false;

    [SerializeField]
    private float m_HookSpeed = 2f;
    [SerializeField]
    private float m_MaxHeight = 0f;
    [SerializeField]
    private float m_MinHeight = -2f;

    private float m_CurrentWeightHooked = 0f;
    private FishBehavior m_CurrentFish = null;

    [SerializeField]
    private ObjectSocket m_StorageSocket;
    private void Update()
    {
        switch (m_FishHooked)
        {
            case false:
                {
                    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    {
                        transform.localPosition += Vector3.up * m_HookSpeed * Time.deltaTime;
                        if (transform.localPosition.y > m_MaxHeight)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, m_MaxHeight);
                        }
                    }
                    if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    {
                        transform.localPosition -= Vector3.up * m_HookSpeed * Time.deltaTime;
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
                        SwitchBait(m_CurrentBait);
                        break;
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
                    {
                        AddStrainPercent((1 - m_CurrentWeightHooked / 100f) / 2f);
                    }
                }
                break;
        }
    }

    public float m_CurrStrainPercent;
    public void AddStrainPercent(float p_StrainPercent)
    {
        m_CurrStrainPercent += p_StrainPercent;
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
    }


    [SerializeField]
    private FishInfo m_DefaultBait;
    public void SwitchBait(FishInfo p_BaitInfo)
    {
        if (m_CurrentBait.m_ID == p_BaitInfo.m_ID)
        {
            return;
        }

        if (p_BaitInfo != m_DefaultBait && IngredientStorage.PeekCount(p_BaitInfo) == 0)
        {
            m_CurrentBait = m_DefaultBait;
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
    }
}
