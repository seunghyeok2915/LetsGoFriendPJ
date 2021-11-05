using System;

[Serializable]
public class StageVO
{
    public int id;
    public int stage;
    public string name;
    public float star_remain_hp_persent;
    public float star_clear_time_second;
    public float remainHp;
    public float clearTime;

    public StageVO(int stage, float remainHp, float clearTime)
    {
        this.stage = stage;
        this.remainHp = remainHp;
        this.clearTime = clearTime;
    }
    public StageVO(int id, string name, float star_remain_hp_persent, float star_clear_time_second)
    {
        this.id = id;
        this.name = name;
        this.star_remain_hp_persent = star_remain_hp_persent;
        this.star_clear_time_second = star_clear_time_second;
    }
}
