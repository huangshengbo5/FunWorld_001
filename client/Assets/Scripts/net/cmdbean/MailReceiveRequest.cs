using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailReceiveRequest : Request
{
    public int id;
    public MailReceiveRequest()
    {
        this.serviceName = "/api/mail/receive";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public MailReceiveRequest(int id) : this()
    {
        this.id = id;
    }
}
