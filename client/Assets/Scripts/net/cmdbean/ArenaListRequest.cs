using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArenaListRequest : Request
{
    public int page;
    public int perpage;

    public ArenaListRequest()
    {
        this.serviceName = "/api/arena/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public ArenaListRequest(int page, int perpage) : this()
    {
        this.page = page;
        this.perpage = perpage;
    }
}
