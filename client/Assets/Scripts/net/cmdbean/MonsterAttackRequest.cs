using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MonsterAttackRequest : Request
{
    public int Index;
    public MonsterAttackRequest()
    {
        this.serviceName = "/api/lobby/monster/attack";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public MonsterAttackRequest(int Index) : this()
    {
        this.Index = Index;
    }
}
