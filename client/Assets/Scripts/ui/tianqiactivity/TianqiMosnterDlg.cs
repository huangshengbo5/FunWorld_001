using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TianqiMosnterDlg : DialogMonoBehaviour
{
    public static TianqiMosnterDlg instance;

    public UILabel titleName;//标题
    public UISprite close;//关闭按钮

    public GridAdapter grid;
    public GameObject tempGameobject;
    
    private void Awake()
    {
        instance = this;
       
    }

    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
