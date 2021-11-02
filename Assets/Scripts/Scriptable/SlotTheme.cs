using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Slot Theme", menuName = "Slot Theme")]
public class SlotTheme : ScriptableObject
{
    public Sprite slotImage;
    public string slotName;
    public SlotSkill[] slotSkills;
}
