using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterReceiveRequest : Request
{
    public int Index;
    public MonsterReceiveRequest()
    {
        this.serviceName = "/api/lobby/monster/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public MonsterReceiveRequest(int Index) : this()
    {
        this.Index = Index;
    }
}
