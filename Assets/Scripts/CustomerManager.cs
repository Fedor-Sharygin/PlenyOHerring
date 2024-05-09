using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomerManager : MonoBehaviour
{
    [SerializeField]
    private Timer m_DayTimer;
    [SerializeField]
    private Timer m_CustomerSpawnTimer;
    [SerializeField]
    private ObjectSocket m_CustomerPosition;

    public void SpawnCustomer()
    {
        if (!m_DayTimer.IsPlaying())
        {
            return;
        }
    }

    private bool m_FishingLoading = false;
    public void SetLoadState()
    {
        m_FishingLoading = true;
    }
    private void Update()
    {
        if (!m_FishingLoading || !m_CustomerPosition.AvailableForStack)
        {
            return;
        }

        SceneManager.LoadScene("FishingLevel");
    }
}
