using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NovaQuest", menuName = "Quests/Quest", order = 1)]

public class Quest : ScriptableObject
{   
    public Narrativa narrativa;
    public Problema problema;
}
