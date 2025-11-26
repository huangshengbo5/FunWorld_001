using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EquipListRequest : Request
{
    public int Page;
    public int PerPage;
    public EquipListRequest()
    {
        this.serviceName = "/api/equip/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
    public EquipListRequest(int Page,int perPage = 100) : this()
    {
        this.Page = Page;
        this.PerPage = perPage;
    }
}
