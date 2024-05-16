using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class CutsceneAnimator : MonoBehaviour
{
    [SerializeField]
    private string m_CutsceneTextFile;
    private StreamReader m_CutsceneTextReader;
    private Animator m_Animator;

    [Space(10)]
    [SerializeField]
    private TMPro.TextMeshProUGUI m_NameText;
    [SerializeField]
    private TMPro.TextMeshProUGUI m_SpeechText;
    private string m_CurText;
    private int m_DisplayTextIdx = -1;

    private AudioSource m_Source;
    private void Awake()
    {
        m_CutsceneTextReader = new StreamReader(Path.Combine(Application.streamingAssetsPath, m_CutsceneTextFile + ".csv"));
        m_Animator = GetComponent<Animator>();
        m_Source = GetComponent<AudioSource>();
    }

    [SerializeField]
    private Timer m_LetterDisplayTimer;
    public bool DisplayNextSpeech()
    {
        if (m_CutsceneTextReader.EndOfStream)
        {
            return false;
        }

        string Line = m_CutsceneTextReader.ReadLine();
        string TriggerText = m_NameText.text;
        m_NameText.text = Line.Substring(0, Line.IndexOf(','));
        SetCurVoice();
        m_SpeechText.text = "";
        m_CurText = Line.Substring(Line.IndexOf(',') + 2);
        m_CurText = m_CurText.Substring(0, m_CurText.Length - 1);
        m_DisplayTextIdx = 0;
        m_LetterDisplayTimer?.ResetTimer();
        m_LetterDisplayTimer?.Play();

        TriggerText += '-' + m_NameText.text;
        m_Animator.SetTrigger(TriggerText);
        return true;
    }

    [Serializable]
    public struct CharacterVoice{
        public string m_CharacterName;
        public AudioClip m_CharacterVoice;
    }
    [SerializeField]
    private CharacterVoice[] m_CharacterVoices;
    private AudioClip m_CurVoice;
    private void SetCurVoice()
    {
        m_CurVoice = null;
        foreach (var CV in m_CharacterVoices)
        {
            if (CV.m_CharacterName.ToLower() == m_NameText.text.ToLower())
            {
                m_CurVoice = CV.m_CharacterVoice;
                break;
            }
        }
    }
    public void ShowNextLetter()
    {
        if (m_DisplayTextIdx == m_CurText.Length)
        {
            m_LetterDisplayTimer?.Stop();
            m_DisplayTextIdx = -1;
            return;
        }

        m_SpeechText.text += m_CurText[m_DisplayTextIdx];
        m_DisplayTextIdx++;

        if (m_Source.isPlaying)
        {
            return;
        }
        m_Source.pitch = UnityEngine.Random.Range(.9f, 1.12f);
        m_Source.PlayOneShot(m_CurVoice);
    }

    private bool m_Done = false;
    private void Update()
    {
        if (m_Done || m_DisplayTextIdx != -1)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            if (!DisplayNextSpeech())
            {
                m_Animator.SetTrigger("PlayIntro");
                m_Done = true;
            }
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("DayStartScene");
    }
}
