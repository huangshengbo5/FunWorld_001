using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guang : MonoBehaviour
{
    public string filename= "PenQuan";
    public int actionid;
    // Start is called before the first frame update
    void Start()
    {
        GameObject texframeobjs111 = ResManager.getGameObject("allpre", "vapaintnodespine");
        APaintNodeSpine xgpaintnode = texframeobjs111.GetComponent<APaintNodeSpine>();
        xgpaintnode.create1(gameObject, filename, filename);
        xgpaintnode.playActionAuto(actionid, true);
        // xgpaintnode.playAction2(actionname, true);
        xgpaintnode.setdepth(20);
        xgpaintnode.transform.localScale = Vector3.one;
        xgpaintnode.transform.localPosition = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
