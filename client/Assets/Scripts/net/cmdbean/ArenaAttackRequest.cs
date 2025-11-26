using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArenaAttackRequest : Request
{
    public int ID;
    public ArenaAttackRequest()
    {
        this.serviceName = "/api/arena/attack";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public ArenaAttackRequest(int id) : this()
    {
        ID = id;
    }
}
