using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArenaSignupRequest : Request
{
    public ArenaSignupRequest()
    {
        this.serviceName = "/api/arena/signup";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
