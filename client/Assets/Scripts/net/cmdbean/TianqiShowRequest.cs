using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TianqiShowRequest : Request
{
    public TianqiShowRequest()
    {
        this.serviceName = "/api/apocalypse/show";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
