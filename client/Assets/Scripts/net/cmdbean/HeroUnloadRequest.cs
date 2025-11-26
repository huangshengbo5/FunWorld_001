using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroUnloadRequest : Request
{
    public HeroUnloadRequest()
    {
        this.serviceName = "/api/hero/unload";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
