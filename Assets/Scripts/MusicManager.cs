using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager m_Instance;
    private AudioSource m_Source;
    private void Awake()
    {
        if (m_Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_Instance = this;

        m_Source = GetComponent<AudioSource>();

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    [SerializeField]
    private AudioClip m_MainMenuMusic;
    [SerializeField]
    private AudioClip m_GameMusic;
    private void OnLevelLoaded(Scene p_Scene, LoadSceneMode _p_SceneMode)
    {
        switch (p_Scene.name)
        {
            case "MainMenu":
                {
                    m_Source.Stop();
                    m_Source.clip = m_MainMenuMusic;
                    m_Source.Play();
                }
                break;
            case "DayStartScene":
                {
                    if (m_Source.clip == m_GameMusic)
                    {
                        break;
                    }

                    m_Source.Stop();
                    m_Source.clip = m_GameMusic;
                    m_Source.Play();
                }
                break;
            case "EntryCutscene":
            case "BlackHoleGameOver":
                {
                    m_Source.Stop();
                }
                break;
        }
    }
}
