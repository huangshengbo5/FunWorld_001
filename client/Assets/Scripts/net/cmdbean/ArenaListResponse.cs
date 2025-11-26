using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ArenaListResponse : Response
{
    public ArenaPageInfo pageInfo;
    public List<ArenaRanker> rankers;
    public List<ArenaRecord> records;
    public ArenaSelf self;
}
