using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailListRequest : Request
{
    public int page;
    public int perpage;
    public MailListRequest()
    {
        this.serviceName = "/api/mail/list";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public MailListRequest(int page, int perpage) : this()
    {
        this.page = page;
        this.perpage = perpage;
    }
}
