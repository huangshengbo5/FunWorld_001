using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MailRemoveAllRequest : Request
{
    public MailRemoveAllRequest()
    {
        this.serviceName = "/api/mail/removeall";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }
}
