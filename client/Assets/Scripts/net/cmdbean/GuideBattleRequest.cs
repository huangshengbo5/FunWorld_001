using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuideBattleRequest : Request
{

    public GuideBattleRequest()
    {
        this.serviceName = "/api/lobby/guide/battle";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
