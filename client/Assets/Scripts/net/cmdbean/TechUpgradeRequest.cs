using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TechUpgradeRequest : Request
{
    public int id;
    public TechUpgradeRequest()
    {
        this.serviceName = "/api/tech/upgrade";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public TechUpgradeRequest(int id) : this()
    {
        this.id = id;
    }
}
