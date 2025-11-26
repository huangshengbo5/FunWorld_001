using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SkeletonAnimation))]
    public class NGUISpine : UIWidget
    {

        public SkeletonAnimation animation;
        public bool visible = true;


    protected override void OnInit()
        {
            base.OnInit();
            isFairyDrawCall = true;

            animation = this.GetComponent<SkeletonAnimation>();
        //animation.onSortingChange += onSortingChange;
        this.GetComponent<SkeletonRenderer>().onSortingChange = onSortingChange;
            
        }


    public override void onSortingChange(int value)
    {
        //if (transform.parent.parent.parent.parent.name == "Build_1011-12,12")
        //    Debug.Log(value +" "+drawCall+" "+ animation + ",  " + transform.parent.name + " " + transform.parent.parent.parent.parent.name);

        if (drawCall != null && animation != null)
        {

            Material[] materials = gameObject.GetComponent<MeshRenderer>().sharedMaterials;
            //if (transform.parent.parent.parent.parent.name == "Build_1011-12,12")
            //    Debug.Log(materials.Length);
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].renderQueue = drawCall.renderQueue;
               // materials[i].renderQueue = value;
                //   Debug.Log(materials[i].name);
            }
            gameObject.GetComponent<MeshRenderer>().sortingOrder = drawCall.sortingOrder;

            //if(transform.parent.parent.parent.parent.name== "Build_1011-12,12")
            //Debug.Log(this.drawCall.renderQueue+",  " + transform.parent.name + " " + transform.parent.parent.parent.parent.name);
        }



    }

    //protected override void OnUpdate()
    //{
    //    base.OnUpdate();
    //    if (drawCall != null)
    //    {
    //        //  Debug.Log(drawCall);
    //        //drawCall.panel.clipRange.clipSoftness

    //        if (drawCall.panel.hasClipping)
    //        {
    //            float angle = 0f;
    //            Vector4 cr = drawCall.panel.drawCallClipRange;

    //            Vector3 pos = drawCall.panel.cachedTransform.InverseTransformPoint(panel.cachedTransform.position);
    //            cr.x -= pos.x;
    //            cr.y -= pos.y;

    //            Vector3 v0 = panel.cachedTransform.rotation.eulerAngles;
    //            Vector3 v1 = drawCall.panel.cachedTransform.rotation.eulerAngles;
    //            Vector3 diff = v1 - v0;

    //            diff.x = NGUIMath.WrapAngle(diff.x);
    //            diff.y = NGUIMath.WrapAngle(diff.y);
    //            diff.z = NGUIMath.WrapAngle(diff.z);

    //            if (Mathf.Abs(diff.x) > 0.001f || Mathf.Abs(diff.y) > 0.001f)
    //                Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", panel);

    //            angle = diff.z;

    //            // Pass the clipping parameters to the shader
    //            // SetClipping(i++, cr, currentPanel.clipSoftness, angle);

    //            Vector2 soft = drawCall.panel.clipSoftness;
    //            int index = 0;

    //            angle *= -Mathf.Deg2Rad;

    //            Vector2 sharpness = new Vector2(1000.0f, 1000.0f);
    //            if (soft.x > 0f) sharpness.x = cr.z / soft.x;
    //            if (soft.y > 0f) sharpness.y = cr.w / soft.y;

    //            if (index < UIDrawCall.ClipRange.Length)
    //            {
    //                Material[] materials = gameObject.GetComponent<MeshRenderer>().sharedMaterials;
    //                if (materials.Length > 0)
    //                {
    //                    materials[0].SetVector(UIDrawCall.ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
    //                    materials[0].SetVector(UIDrawCall.ClipArgs[index], new Vector4(sharpness.x, sharpness.y, Mathf.Sin(angle), Mathf.Cos(angle)));
    //                }

    //            }
    //        }
    //    }
    //}
        public void applydrawcall()
        {

            if (drawCall != null && animation != null)
            {
                Material[] materials = gameObject.GetComponent<MeshRenderer>().sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i].renderQueue = drawCall.renderQueue;
                }
                gameObject.GetComponent<MeshRenderer>().sortingOrder = drawCall.sortingOrder;
            }

            RemoveFromPanel();
            MarkAsChanged();

        }

    public void SetActive(bool v)
        {
            visible = v;

            gameObject.SetActive(v);


        }

        void OnDestroy()
        {
            Destroy(animation);
            animation = null;
        }
    }

