using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CampaignAttackResponse:Response
{
    public int heroID;
    public int heroLevel;
    public FightBean report;
    public List<Reward> rewards;
    public bool win;
}
