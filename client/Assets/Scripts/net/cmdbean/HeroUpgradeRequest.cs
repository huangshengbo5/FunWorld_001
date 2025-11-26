using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroUpgradeRequest : Request
{
    public int id;
    public HeroUpgradeRequest()
    {
        this.serviceName = "/api/hero/upgrade";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public HeroUpgradeRequest(int id) : this()
    {
        this.id = id;
    }
}
