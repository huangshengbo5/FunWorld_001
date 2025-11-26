using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GMIncreatGoldRequest : Request
{
    public int gold;
    public GMIncreatGoldRequest()
    {
        this.serviceName = "/api/gm/incrgold";
        this.responseName = "Response";
    }

    public GMIncreatGoldRequest(int vl):this()
    {
        gold = vl;
    }
}