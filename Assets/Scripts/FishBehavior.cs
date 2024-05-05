using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    [SerializeField]
    private bool m_Frozen;

    [SerializeField]
    private FishInfo m_FishInfo;

    private PlayerHook m_Hook;
    private void Start()
    {
        m_Hook = GameObject.FindFirstObjectByType<PlayerHook>();
        SetNewPos(10 * m_Direction);

        GetComponent<Animator>().runtimeAnimatorController = m_FishInfo.m_FishAnimator;
    }

    [SerializeField]
    private float m_Speed;
    public int m_Direction = 1;
    private bool m_Hooked = false;
    private void Update()
    {
        if (m_Frozen || m_Hooked)
        {
            return;
        }

        transform.localPosition += Vector3.right * m_Speed * Time.deltaTime * m_Direction;

        if (m_Direction == 1 && transform.localPosition.x >= 10)
        {
            SetNewPos(-10);
        }
        if (m_Direction == -1 && transform.localPosition.x <= -10)
        {
            SetNewPos(10);
        }
    }

    private void SetNewPos(float p_NewX)
    {
        transform.localPosition = new Vector3(p_NewX, Random.Range(-3f, 0f));
        m_Speed = Random.Range(m_FishInfo.m_MinSpeed, m_FishInfo.m_MaxSpeed);
    }


    [SerializeField]
    private Timer m_TugTimer;
    private void OnTriggerEnter2D(Collider2D p_Other)
    {
        if (m_Hook == null)
        {
            return;
        }

        if (!p_Other.CompareTag("Player"))
        {
            return;
        }

        bool TargetFound = false;
        foreach (var Target in m_FishInfo.m_FishTargets)
        {
            if (m_Hook.m_CurrentBait == Target)
            {
                TargetFound = true;
                break;
            }
        }
        if (!TargetFound)
        {
            m_Hook = null;
            return;
        }

        m_Hooked = true;

        m_Hook.m_CurrentWeightHooked = m_FishInfo.m_Weight;
        m_Hook.m_CurrStrainPercent = .5f;
        m_Hook.m_FishHooked = true;
        m_Hook.m_CurrentFish = this;

        m_TugTimer?.Play();
    }
    public void TugTheHook()
    {
        if (m_Hook == null)
        {
            return;
        }

        m_Hook.AddStrainPercent(-m_FishInfo.m_Weight / 100f);
    }

    public void Release()
    {
        m_Hook = null;
        m_Hooked = false;
        m_TugTimer?.Stop();
    }
    public void GetCaught()
    {
        IngredientStorage.m_FishList.Add(m_FishInfo);
        Destroy(gameObject);
    }
}
