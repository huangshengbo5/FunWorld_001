using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SystemConfigRequest : Request
{
    public SystemConfigRequest()
    {
        this.serviceName = "/api/lobby/system/config";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
