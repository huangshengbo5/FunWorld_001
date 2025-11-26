using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroListRequest : Request
{
    public HeroListRequest()
    {
        this.serviceName = "/api/hero/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
