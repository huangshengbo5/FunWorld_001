using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Login1Request : Request
{
    public string name;
    public string password;
    public string idcard;
    public Login1Request()
    {
        this.serviceName = "/login/login/regist";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public Login1Request(string name, string password, string idcard) : this()
    {
        this.name = name;
        this.password = password;
        this.idcard = idcard;
    }
}
