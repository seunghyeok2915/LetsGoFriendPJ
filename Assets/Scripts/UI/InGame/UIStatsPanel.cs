using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStatsPanel : MonoBehaviour
{
    public Transform contentView;
    public StatSlot statSlotPrefab;

    public SkillDescriptionPanel skillDescriptionPanel;
    public void AddSkill(SlotSkill stat)
    {
        StatSlot statSlot = Instantiate(statSlotPrefab,contentView);
        statSlot.SetData(stat);
        statSlot.button.onClick.RemoveAllListeners();
        statSlot.button.onClick.AddListener(() => OnClickStatBtn(stat));
    }
    
    public void OnClickStatBtn(SlotSkill stat)
    {
        skillDescriptionPanel.Open(stat);
    }
}
