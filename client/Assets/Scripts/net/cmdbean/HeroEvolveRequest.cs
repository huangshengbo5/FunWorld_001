using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroEvolveRequest : Request
{
    public int id;
    public HeroEvolveRequest()
    {
        this.serviceName = "/api/hero/evolve";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
      //  this.SeqId = msgId;
    }
    public HeroEvolveRequest(int id) : this()
    {
        this.id = id;

    }
}
