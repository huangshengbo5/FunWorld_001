using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TianqiAttackRequest : Request
{
    public int apocalypseID;
    public string adsID;
    public int type;
    public TianqiAttackRequest()
    {
        this.serviceName = "/api/apocalypse/attack";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public TianqiAttackRequest(int id,int type,string ADIdstr) :this()
    {
        this.apocalypseID = id;
        this.adsID = ADIdstr;
        this.type = type;
    }
}
