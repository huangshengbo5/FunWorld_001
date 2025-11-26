using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipUnloadRequest : Request
{
    public int id;
    public EquipUnloadRequest()
    {
        this.serviceName = "/api/equip/unload";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipUnloadRequest(int id) : this()
    {
        this.id = id;
    }
}
