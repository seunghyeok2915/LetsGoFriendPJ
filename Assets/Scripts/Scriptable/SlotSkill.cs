using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot Skill", menuName = "Slot Skill")]
public class SlotSkill : ScriptableObject
{
    public Sprite skillImage;
    public string skillName;
    public string definition;
    public byte skillNumber;
}
