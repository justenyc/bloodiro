using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "QuickJam/New Attack", order = 1)]
public class AttackData : ScriptableObject
{
    public string id;
    public int cancelPriority;
    public int lengthInFrames;
    [Space(10)]
    public int activeStart;
    public int activeEnd;
    [Space(10)]
    public AnimationClip animationClip;
}
