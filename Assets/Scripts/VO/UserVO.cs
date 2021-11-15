using System;

[Serializable]
public class UserVO
{
    public string id; //디바이스 아이디
    public string name;

    public UserVO(string id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
