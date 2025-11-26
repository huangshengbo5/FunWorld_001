using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GuideStepRequest : Request
{
    public int step;
    public GuideStepRequest()
    {
        this.serviceName = "/api/lobby/guide/step";
        this.responseName = this.GetType().Name.Replace("Request", "Response");
    }

    public GuideStepRequest(int _step) : this()
    {
        step = _step;
    }
}
