using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TechListRequest : Request
{
    public TechListRequest()
    {
        this.serviceName = "/api/tech/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
