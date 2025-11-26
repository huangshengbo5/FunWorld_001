using UnityEngine;
using System.Collections;

public class UIParticleNode : UIWidget
{

    private Renderer renderer;
    
    protected override void OnInit()
    {
        base.OnInit();
        isFairyDrawCall = true;


        renderer = this.GetComponent<Renderer>();
        Update();
    }


    public override void onSortingChange(int value)
    {
        if (drawCall != null && renderer != null)
        {
            if (renderer != null && renderer.sharedMaterial != null)
            {
                renderer.sharedMaterial.renderQueue = drawCall.renderQueue;

                renderer.sortingOrder = drawCall.sortingOrder;

            }
        }

        //需要更新Materials的shader  clip之类的信息

    }


    void Update()
    {
        onSortingChange(0);
    }

}
