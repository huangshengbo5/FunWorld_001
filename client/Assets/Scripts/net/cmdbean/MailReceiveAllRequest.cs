using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailReceiveAllRequest : Request
{
    public MailReceiveAllRequest()
    {
        this.serviceName = "/api/mail/receiveall";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
