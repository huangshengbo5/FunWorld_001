using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLoader : MonoBehaviour
{
    static GameObject globalnode;

    void Awake()
    {
        if (globalnode == null)
        {
            globalnode = ResManager.getGameObject("allpre", "global");
            globalnode.gameObject.SetActive(true);
            //  prefabUI.gameObject.transform.SetParent(transform);
            globalnode.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
