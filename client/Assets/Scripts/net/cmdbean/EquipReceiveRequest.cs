using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipReceiveRequest : Request
{
    public int equipID;
    public EquipReceiveRequest()
    {
        this.serviceName = "/api/equip/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipReceiveRequest(int equipID) : this()
    {
        this.equipID = equipID;
    }
}
