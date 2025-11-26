using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBTest : MonoBehaviour
{
    public static string tempjson = "{\"Type\":1,\"AttackerID\":1111,\"AttackerName\":\"崔亮\",\"AttackerLevel\":10,\"DefenderID\":2222,\"DefenderName\":\"加班哥\",\"DefenderLevel\":5,\"Actions\":[[0, 0, 1111, 2222, 0, 10013, false, false],[1, true, 1111, 2222, 117],[3],[0, 0, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 0, 2222, 1111, 0,10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 1000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 1000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 1000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500,0],[3],[0, 2000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 2000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 2000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 3000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 3000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 4000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 4000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 4000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 5000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 5000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 6000],[0, 6000, 1111, 2222, 0, 10011, false, false],[3],[0, 6000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 7000, 1111, 2222, 0, 10013, false, true],[3],[0, 7000, 1111, 2222, 0, 10012, false, true],[3],[0, 7000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 8000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 8000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 9000, 1111, 2222, 0, 10013, true, false],[1,true, 1111, 2222, 99],[3],[0, 9000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 10000],[0, 10000, 1111, 2222, 0, 10011, false, false],[3],[0, 10000, 1111, 2222, 0, 10012, false, false],[3],[0, 10000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 11000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 196],[3],[0, 11000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 39],[3],[0, 11000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 12000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 12000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 13000, 1111, 2222, 0, 10013, false, true],[3],[0, 13000, 1111, 2222, 0, 10012, false, true],[3],[0, 13000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 14000],[0, 14000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 117],[3],[0, 14000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 14000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 15000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 196],[3],[0, 15000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 15000, 2222, 1111, 0, 10022, true, true],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 16000],[2, 0, 1111, 1, 0, 0, 0, 16000],[0, 16000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 17000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 17000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 17000, 2222, 1111, 0, 10022, true, false],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 18000],[2, 0, 1111, 1, 0, 0, 0, 18000],[0, 18000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 19000, 1111, 2222, 0, 10011, true, false],[1, true, 1111, 2222, 83],[3],[0, 19000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 19000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 20000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 20000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 20000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 21000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 21000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 21000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 22000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 22000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 22000, 2222, 1111, 0, 10022, true, true],[2, 2, 1111, 1, 1000, 0, 0],[3],[2, 1, 1111, 1, 0, 0, 0, 23000],[2, 0, 1111, 1, 0, 0, 0, 23000],[0, 23000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[2, 1, 2222, 8, 0, 0, 0, 24000],[0, 24000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 235],[3],[0, 24000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 39],[3],[0, 24000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 25000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 117],[3],[0, 25000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 19],[3],[0, 25000, 2222, 1111, 0, 10021, true, true],[1, true, 2222, 1111, 196],[3],[0, 26000, 1111, 2222, 0, 10011, false, false],[3],[0, 26000, 1111, 2222, 0, 10012, false, false],[3],[0,26000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 235],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 27000, 1111, 2222, 0, 10013, true, false],[1, true, 1111, 2222, 99],[3],[0, 27000, 1111, 2222, 0, 10012, true, false],[1, true, 1111, 2222, 16],[3],[0, 27000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 28000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 28000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 28000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 29000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 29000, 2222, 1111, 0, 10021, true, false],[1, true, 2222, 1111, 98],[3],[0, 30000, 1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0,30000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 30000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000,1500, 0],[3],[0, 31000, 1111, 2222, 0, 10013, true, true],[1, true, 1111, 2222, 199],[3],[0, 31000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 31000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 32000, 1111, 2222, 0, 10011, false, false],[3],[0, 32000, 1111, 2222, 0, 10012, false, false],[3],[0, 32000, 2222, 1111, 0, 10033, true, false],[1, true, 2222, 1111, 117],[2, 2, 2222, 8, 3000, 1500, 0],[3],[0, 33000,1111, 2222, 0, 10011, true, true],[1, true, 1111, 2222, 166],[3],[0, 33000, 1111, 2222, 0, 10012, true, true],[1, true, 1111, 2222, 33],[3],[0, 33000, 2222, 1111, 0, 10033, true, true],[1, true, 2222, 1111, 86],[2, 2, 2222, 8, 3000, 1500, 0],[3]],\"Result\":{\"Result\":1,\"AttackerHP\":0,\"DefenderHP\":857,\"Background\":1}}";
    public static FightBean fb;
    void Start()
    {
        tempjson = ResManager.getText("ftest/0928.txt");
        Debug.LogError(tempjson);
        fb = (FightBean)XLH.readToObject(tempjson, "FightBean");
        fb.format();
        Debug.LogError(fb.attacker.hp + " " + fb.defender.hp);
        for (int i=0;i< fb.actionlist.Count; i++)
        {
            FightBeanAction action = fb.actionlist[i];
            Debug.LogError(i + " " + action);
            if (action.type == 0)
            {
                if (action.resultaction!=null)
                {
                    if (action.resultaction.hitresult)
                    {
                        if (fb.defender.id == action.resultaction.toid)
                        {
                            fb.defender.hp -= action.resultaction.hpcost;
                        }
                        else if (fb.attacker.id == action.resultaction.toid)
                        {
                            fb.attacker.hp -= action.resultaction.hpcost;
                        }
                        Debug.LogError(i+" "+action.resultaction.toid + "-=>" + action.resultaction.hpcost);
                    }
                    else
                    {
                        Debug.LogError(i + " " + action.resultaction.toid + "-=>shanbi");
                    }

                }
                if (action.buffactionlist.Count>0)
                {
                    for (int ii = 0; ii < action.buffactionlist.Count; ii++)
                    {
                        FightBeanAction buffaction = action.buffactionlist[ii];
                        if (buffaction.buffid == 0)
                        {
                            if (fb.attacker.id == buffaction.toid)
                            {
                                fb.attacker.hp -= action.buffeff0;
                            }
                            else
                            {
                                fb.defender.hp -= action.buffeff0;
                            }
                            Debug.LogError(i + " " + buffaction.toid + "=>" + buffaction.buffeff0);
                        }else if (buffaction.buffid == 5)
                        {
                            Debug.Log(fb.attacker.id + ":" + fb.attacker.hp + " " + fb.defender.id + ":" + fb.defender.hp);

                            if (fb.attacker.id == buffaction.toid)
                            {
                                fb.attacker.hp += buffaction.buffeff0;
                            }
                            else
                            {
                                fb.defender.hp += buffaction.buffeff0;
                            }
                            Debug.LogError(i + " " + buffaction.toid + "=>+" + buffaction.buffeff0);
                        }
                    }


                }



            }
            else if(action.type == 2)
            {
                if (action.buffid == 0)
                {
                    if(fb.attacker.id== action.toid)
                    {
                        fb.attacker.hp -= action.buffeff0;
                    }
                    else
                    {
                        fb.defender.hp -= action.buffeff0;
                    }
                    Debug.LogError(action.toid + "->" + action.buffeff0);
                }else if (action.buffid == 5)
                {
                    if (fb.attacker.id == action.toid)
                    {
                        fb.attacker.hp += action.buffeff0;
                    }
                    else
                    {
                        fb.defender.hp += action.buffeff0;
                    }
                    Debug.LogError(action.toid + "->+" + action.buffeff0);
                }
            }

            Debug.LogError(fb.attacker.id + ":" + fb.attacker.hp + " " + fb.defender.id + ":" + fb.defender.hp);
        }

        Debug.LogError(fb.attacker.hp + " " + fb.defender.hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
