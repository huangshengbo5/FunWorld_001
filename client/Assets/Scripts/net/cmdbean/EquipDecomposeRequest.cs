using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipDecomposeRequest : Request
{
    public List<int> ids;
    public EquipDecomposeRequest()
    {
        this.serviceName = "/api/equip/decompose";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipDecomposeRequest(List<int> ids) : this()
    {
        this.ids = ids;
    }
}
