using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FishManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_FishPrefabs;
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

    [SerializeField]
    private Timer m_FishSpawnTimer;
    private void Awake()
    {
        IngredientStorage.InitializeStorage(m_Infos);
        m_FishSpawnTimer.TimerModifier = IngredientStorage.FishSpawnBonus;
    }



    [SerializeField]
    private AudioSource m_FishSFXSource;
    public void SpawnFish()
    {
        if (m_FishPrefabs.Length == 0)
        {
            return;
        }

        GameObject NFish = null;

        float RandomWeightChoice = Random.Range(0f, m_TotalWeight);
        for (int i = 0; i < m_FishRandomWeights.Length; ++i)
        {
            RandomWeightChoice -= m_FishRandomWeights[i];
            if (RandomWeightChoice > 0f)
            {
                continue;
            }
            NFish = GameObject.Instantiate(m_FishPrefabs[i], Vector3.up * -100f, Quaternion.identity);
            NFish.GetComponent<FishBehavior>().m_Direction = Random.Range(0, 2) == 0 ? -1 : 1;
            NFish.GetComponent<FishBehavior>().InitializeValues(m_Infos[i]);
            NFish.GetComponent<FishBehavior>().m_AudioSource = m_FishSFXSource;
            NFish.transform.localScale = new Vector3(NFish.transform.localScale.x * NFish.GetComponent<FishBehavior>().m_Direction, NFish.transform.localScale.y, NFish.transform.localScale.z);
            break;
        }
    }


    public void LoadStoreLevel()
    {
        SceneManager.LoadScene("KitchenScene");
    }
}
