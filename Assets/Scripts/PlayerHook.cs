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

    public float m_CurrentWeightHooked = 0f;
    public FishBehavior m_CurrentFish = null;

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
                        break;
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
                    {
                        AddStrainPercent(1 - m_CurrentWeightHooked / 100f);
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
}
