using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHook : MonoBehaviour
{
    public FishInfo m_CurrentBait;
    private bool m_FishHooked = false;

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
                        m_FishHooked = false;
                        break;
                    }
                    else if (m_CurrStrainPercent >= 1)
                    {
                        m_CurrentFish.GetCaught();
                        m_FishHooked = false;
                        foreach (var Fish in m_HookSocket.RemoveObjs())
                        {
                            m_StorageSocket?.ForceStack(Fish);
                        }
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


    public void SwitchBait(FishInfo p_BaitInfo)
    {
        m_CurrentBait = p_BaitInfo;
    }
}
