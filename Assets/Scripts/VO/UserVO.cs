using System;

[Serializable]
public class UserVO
{
    public string id; //����̽� ���̵�
    public string name;

    public UserVO(string id, string name)
    {
        this.id = id;
        this.name = name;
    }
}
