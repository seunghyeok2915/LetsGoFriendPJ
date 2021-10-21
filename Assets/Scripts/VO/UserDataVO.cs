using System;

[Serializable]
public class UserDataVO
{
    public string name;
    public int zem;

    public UserDataVO( string name,int zem)
    {
        this.name = name;
        this.zem = zem;
    }
}
