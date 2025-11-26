using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailReadRequest : Request
{
    public int id;
    public MailReadRequest()
    {
        this.serviceName = "/api/mail/read";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public MailReadRequest(int id) : this()
    {
        this.id = id;
    }
}
