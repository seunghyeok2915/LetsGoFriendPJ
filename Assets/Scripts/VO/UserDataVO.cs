using System;

[Serializable]
public class UserDataVO
{
    public string name;
    public int zem;
    public int stage;
    public int score;

    public UserDataVO( string name,int zem, int stage, int score)
    {
        this.name = name;
        this.zem = zem;
        this.stage = stage;
        this.score = score;
    }
}
