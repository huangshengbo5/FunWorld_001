using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TriggerCheck : MonoBehaviour
{
    public Action onTriggerChecked;
    public string triggerName = "clipendtrigger";
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals(triggerName) )
        {
            onTriggerChecked?.Invoke();
        }
    }
}
