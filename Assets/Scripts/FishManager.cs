using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_FishPrefab;
    [SerializeField]
    private FishInfo[] m_Infos;

    public void SpawnFish()
    {
        if (m_FishPrefab == null)
        {
            return;
        }

        var NFish = GameObject.Instantiate(m_FishPrefab, Vector3.up * -100f, Quaternion.identity);
        NFish.GetComponent<FishBehavior>().m_Direction = Random.Range(0, 2) == 0 ? -1 : 1;
        NFish.GetComponent<FishBehavior>().InitializeValues(m_Infos[Random.Range(0, m_Infos.Length)]);
    }
}
