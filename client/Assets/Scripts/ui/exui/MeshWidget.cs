using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[RequireComponent(typeof(MeshNode))]
public class MeshWidget : UIWidget
{

    protected override void OnInit()
    {
        base.OnInit();
        isFairyDrawCall = true;



    }
    public override void onSortingChange(int value)
    {
        if (drawCall != null&& gameObject.GetComponent<Renderer>()!=null)
        {
            Material[] materials = gameObject.GetComponent<Renderer>().sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                if(materials[i]!=null)
                materials[i].renderQueue = drawCall.renderQueue;
            }
            gameObject.GetComponent<Renderer>().sortingOrder = drawCall.sortingOrder;
           //    Debug.Log(this.drawCall.triangles);

        }

    }
    
}
