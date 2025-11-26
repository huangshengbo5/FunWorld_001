using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UserInfoRequest : Request
{
    public UserInfoRequest()
    {
        this.serviceName = "/api/lobby/user";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}