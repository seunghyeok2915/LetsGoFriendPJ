using System;

[Serializable]
public class UserChapterVO
{
    public int chapterid;
    public int stagenum;

    public UserChapterVO(int chapterid, int stagenum)
    {
        this.chapterid = chapterid;
        this.stagenum = stagenum;
    }
}
