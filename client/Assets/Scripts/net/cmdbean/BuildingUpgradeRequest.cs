using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildingUpgradeRequest : Request
{
    public int id;
    public BuildingUpgradeRequest()
    {
        this.serviceName = "/api/building/upgrade";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public BuildingUpgradeRequest(int id) : this()
    {
        this.id = id;
    }
}
