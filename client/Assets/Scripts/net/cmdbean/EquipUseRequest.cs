using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipUseRequest : Request
{
    public int id;
    public int partnerID;
    public int pos;
    public EquipUseRequest()
    {
        this.serviceName = "/api/equip/use";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipUseRequest(int id,int partnerID,int pos) : this()
    {
        this.id = id;
        this.partnerID = partnerID;
        this.pos = pos;
    }
}
