using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishInfo", menuName = "Descriptors/Fish", order = 1)]
public class FishInfo : ScriptableObject
{
    public float    m_RecommendedCost;
    public float    m_Weight;
    public string   m_Taste;
    public string   m_Minigame;

    [Space(10)]
    public RuntimeAnimatorController m_FishAnimator;

    [Space(10)]
    public float    m_MinSpeed;
    public float    m_MaxSpeed;

    [Space(10)]
    public FishInfo[] m_FishTargets;
}
