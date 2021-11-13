using System;

[Serializable]
public class ChapterVO
{
    public int id;
    public string name;
    public int maxstage;

    public ChapterVO(int id, string name, int maxstage)
    {
        this.id = id;
        this.name = name;
        this.maxstage = maxstage;
    }
}
