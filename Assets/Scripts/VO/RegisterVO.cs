using System;

[Serializable]
public class RegisterVO
{
    public string id;
    public string password;
    public string name;

    public RegisterVO(string id, string password, string name)
    {
        this.id = id;
        this.password = password;
        this.name = name;
    }
}
