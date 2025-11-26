using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LoginRequest : Request
{
    public string name;
    public string password;
    public bool clear;
    public LoginRequest()
    {
        this.serviceName = "/login/login/native";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public LoginRequest(string name, string password, bool clear) : this()
    {
        this.name = name;
        this.password = password;
        this.clear = clear;
    }
}
