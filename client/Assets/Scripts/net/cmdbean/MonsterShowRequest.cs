using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterShowRequest : Request
{
    public MonsterShowRequest()
    {
        this.serviceName = "/api/lobby/monster/show";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
