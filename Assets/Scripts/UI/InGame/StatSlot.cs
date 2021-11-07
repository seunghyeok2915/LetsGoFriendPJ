using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlot : MonoBehaviour
{
    public Button button;
    public Image icon;
    private SlotSkill slotSkill;

    public void SetData(SlotSkill slotSkill)
    {
        this.slotSkill = slotSkill;
        icon.sprite = slotSkill.skillImage;
    }
}
