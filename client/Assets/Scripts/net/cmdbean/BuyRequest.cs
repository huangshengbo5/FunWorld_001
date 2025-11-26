using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuyRequest : Request
{
    public int id;
    public BuyRequest()
    {
        this.serviceName = "/api/lobby/buy";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public BuyRequest(int id) : this()
    {
        this.id = id;
    }
}
