using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TianqiAwardRequest : Request
{
    public int apocalypseID;

    public TianqiAwardRequest()
    {
        this.serviceName = "/api/apocalypse/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public TianqiAwardRequest(int id):this()
    {
        apocalypseID = id;
    }
}
