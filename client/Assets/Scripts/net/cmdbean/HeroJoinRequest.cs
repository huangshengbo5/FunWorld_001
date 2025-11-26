using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeroJoinRequest : Request
{
    public int id;
    public HeroJoinRequest()
    {
        this.serviceName = "/api/hero/join";
        /// api / hero / unload
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public HeroJoinRequest(int id) : this()
    {
        this.id = id;
    }
}
