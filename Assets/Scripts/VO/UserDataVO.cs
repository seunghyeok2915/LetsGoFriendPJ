using System;

[Serializable]
public class UserDataVO
{
    public string name;
    public int zem;
    public int chapter;
    public int total_stage;

    public UserDataVO(string name, int zem, int chapter, int total_stage)
    {
        this.name = name;
        this.zem = zem;
        this.chapter = chapter;
        this.total_stage = total_stage;
    }
}
