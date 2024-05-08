using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_FishPrefab;
    [SerializeField]
    private FishInfo[] m_Infos;
    [SerializeField]
    private float[] m_FishRandomWeights;
    private float m_TotalWeight
    {
        get
        {
            float Total = 0f;
            foreach (var RW in m_FishRandomWeights)
            {
                Total += RW;
            }
            return Total;
        }
    }

    public void SpawnFish()
    {
        if (m_FishPrefab == null)
        {
            return;
        }

        var NFish = GameObject.Instantiate(m_FishPrefab, Vector3.up * -100f, Quaternion.identity);
        NFish.GetComponent<FishBehavior>().m_Direction = Random.Range(0, 2) == 0 ? -1 : 1;

        float RandomWeightChoice = Random.Range(0f, m_TotalWeight);
        for (int i = 0; i < m_FishRandomWeights.Length; ++i)
        {
            RandomWeightChoice -= m_FishRandomWeights[i];
            if (RandomWeightChoice > 0f)
            {
                continue;
            }

            NFish.GetComponent<FishBehavior>().InitializeValues(m_Infos[i]);
            break;
        }
    }
}
