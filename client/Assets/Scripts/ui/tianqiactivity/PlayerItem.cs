
using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    public UISprite icongen;
    public UILabel lb_level;
    public UILabel lb_name;
    public Player bean;

    TexPaintNode temppaintnode;
    public void setdata(Player data)
    {
        bean = data;

        if (string.IsNullOrEmpty(data.name))
        {
            lb_name.text = data.id.ToString();
        }
        else
        {
            lb_name.text = data.name;
        }

        lb_level.text = bean.level.ToString();
        if (!temppaintnode)
        {
            GameObject texframeobjs = ResManager.getGameObject("allpre", "vtexpaintnode");
            texframeobjs.name = "icon";
            temppaintnode = texframeobjs.GetComponent<TexPaintNode>();
            temppaintnode.create1(icongen.gameObject, "huobantou");
            temppaintnode.setdepth(22);
        }
        string headstr = data_avatarDef.dicdatas[bean.avatar][0].name;
        temppaintnode.playAction(headstr);
    }
}
