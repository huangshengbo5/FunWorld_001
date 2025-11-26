using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipUpgradeRequest : Request
{
    public int equipID;
    public int id;
    public EquipUpgradeRequest()
    {
        this.serviceName = "/api/equip/upgrade";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipUpgradeRequest(int equipID,int id) : this()
    {
        this.equipID = equipID;
        this.id = id;
    }
}
