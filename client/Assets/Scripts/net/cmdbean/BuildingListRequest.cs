using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildingListRequest : Request
{
    public BuildingListRequest()
    {
        this.serviceName = "/api/building/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
