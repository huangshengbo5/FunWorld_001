
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathNode
{
    public PathNode m_parent;//父节点  
    public PathNode m_child;//子节点  
    public float m_costToSource;//到起始点的距离  
    public int m_idx;
    public float m_FValue;
    public PathNode()
    {
        m_parent = null;
        m_child = null;
        m_costToSource = 0;
        m_FValue = 0;
    }
};
public class PathSearchInfo
{

    public static int m_startidx;

    public static int m_endidx;

    public static List<PathNode> m_openList;//开放列表(里面存放相邻节点)  
    public static List<PathNode> m_closeList;//检测列表(里面存放除了障碍物的节点)  

    public static PathNode openhead, closehead;
    public static bool lianbiaoflag = true;

  //  public static List<StagePoint> mappointlist;
    public static StagePoint[] mappointarray;

    static float getDis(PathNode obj1, PathNode obj2)
    {
        float _x = Math.Abs(mappointarray[obj2.m_idx].dbbean.x - mappointarray[obj1.m_idx].dbbean.x);
        float _y = Math.Abs(mappointarray[obj2.m_idx].dbbean.y - mappointarray[obj1.m_idx].dbbean.y);

        return _x + _y;
    }
    //把相邻的节点放入开放节点中;  
    static void addNodeIntoList(PathNode node, PathNode adjacent, PathNode endNode)
    {

        if (adjacent != null)
        {
            float _x = Math.Abs(mappointarray[endNode.m_idx].dbbean.x - mappointarray[adjacent.m_idx].dbbean.x);
            float _y = Math.Abs(mappointarray[endNode.m_idx].dbbean.y - mappointarray[adjacent.m_idx].dbbean.y);

            float F, G, H1, H2, H3;
            adjacent.m_costToSource = node.m_costToSource + getDis(node, adjacent);//获得累计的路程  
            G = adjacent.m_costToSource;

            //三种算法, 感觉H2不错  
            H1 = _x + _y;
            H2 = hypot(_x, _y);
            H3 = max(_x, _y);

            //A*算法 = Dijkstra算法 + 最佳优先搜索  
            //F = G + H2;  

            //Dijkstra算法  
            //F = G;  

            //最佳优先搜索  
            F = H2;

            adjacent.m_FValue = F;

            adjacent.m_parent = node;//设置父节点  
                                     //adjacent->setColor(Color3B::ORANGE);//搜寻过的节点设为橘色  
                                     //   node.m_child = adjacent;//设置子节点    
            PathSearchInfo.m_openList.Add(adjacent);//加入开放列表  

            // AddNode(openhead, adjacent);
        }
    }
    static void addNodeIntoList1(PathNode node, PathNode adjacent, PathNode endNode)
    {

        if (adjacent != null)
        {
            float _x = Math.Abs(mappointarray[endNode.m_idx].dbbean.x - mappointarray[adjacent.m_idx].dbbean.x);
            float _y = Math.Abs(mappointarray[endNode.m_idx].dbbean.y - mappointarray[adjacent.m_idx].dbbean.y);

            float F, G, H1, H2, H3;
            adjacent.m_costToSource = node.m_costToSource + getDis(node, adjacent);//获得累计的路程  
            G = adjacent.m_costToSource;

            //三种算法, 感觉H2不错  
            H1 = _x + _y;
            H2 = hypot(_x, _y);
            H3 = max(_x, _y);

            //A*算法 = Dijkstra算法 + 最佳优先搜索  
            F = G + H2;

            //Dijkstra算法  
            //F = G;  

            //最佳优先搜索  
            // F = H2;

            adjacent.m_FValue = F;

            adjacent.m_parent = node;//设置父节点  
                                     //adjacent->setColor(Color3B::ORANGE);//搜寻过的节点设为橘色  
                                     //   node.m_child = adjacent;//设置子节点    
                                     //   PathSearchInfo.m_openList.Add(adjacent);//加入开放列表  

            AddNode(openhead, adjacent);
        }
    }
    //从开放节点中获取路径最小值; 
    static PathNode getMinPathFormOpenList()
    {
        if (m_openList.Count > 0)
        {
            PathNode _sp = m_openList[0];
            for (int i = 1; i < m_openList.Count; i++)
            {
                PathNode _sp1 = m_openList[i];
                if (_sp1.m_FValue < _sp.m_FValue)
                {
                    _sp = _sp1;
                }
            }
            return _sp;


        }
        else
        {
            return null;
        }
    }
    static PathNode getMinPathFormOpenList1()
    {

        PathNode min = openhead.m_child;
        PathNode current = min.m_child;
        while (current != null)
        {
            if (current.m_FValue < min.m_FValue)
                min = current;
            current = current.m_child;
        }
        return min;

    }
    //根据点获取对象; 
    static PathNode getPathNode(int idx)
    {

        for (int i = 0; i < m_openList.Count; i++)
        {
            PathNode _sp1 = m_openList[i];

            if (_sp1.m_idx == idx)
            {
                return null;
            }
        }

        for (int i = 0; i < m_closeList.Count; i++)
        {
            PathNode _sp1 = m_closeList[i];

            if (_sp1.m_idx == idx)
            {
                // Debug.Log("11111111111111111");
                return null;
            }
        }


        PathNode ps;
        ps = new PathNode();
        ps.m_idx = idx;
        return ps;

        // return null;
    }
    static PathNode getPathNode1(int idx)
    {


        if (InLinkList(openhead, idx))
        {
            return null;
        }
        if (InLinkList(closehead, idx))
        {

            return null;
        }


        PathNode ps;
        ps = new PathNode();
        ps.m_idx = idx;
        return ps;
    }

    static PathNode getPathNode1(int nowidx, int idx)
    {


        if (InLinkList(openhead, idx))
        {
            //if (x == 26 && y == 39)
            //{
            //    Debug.Log("inorout====00======" + inorout);
            //}
            return null;
        }
        if (InLinkList(closehead, idx))
        {
            //if (x == 26 && y == 39)
            //{
            //    Debug.Log("inorout=====11=====" + inorout);
            //}
            return null;
        }


        return null;
    }



    static PathNode getPathNode2(int idx)
    {

        //if (x == 26 && y == 39)
        //{
        //  Debug.Log("inorout==========" + whichfloor);
        //}

        if (InLinkList(openhead, idx))
        {
            //if (x == 26 && y == 39)
            //{
            //   Debug.Log("inorout====00======" + whichfloor);
            //}
            return null;
        }
        if (InLinkList(closehead, idx))
        {
            //if (x == 26 && y == 39)
            //{
            //   Debug.Log("inorout=====11=====" + whichfloor);
            //}
            return null;
        }



        //if (PMap.dataArray[x,y].zhi == 0)
        //{
        //    PathNode ps;
        //    ps = new PathNode();
        //    ps.m_x = x;
        //    ps.m_y = y;
        //    return ps;
        //}
        //Debug.Log("3333333333333333333333");

        //if (x == 26 && y == 39)
        //{
        //  Debug.Log("inorout=====44444=====" + whichfloor);
        //}
        return null;
    }



    private static bool InLinkList(PathNode ohead, int idx)
    {
        PathNode preNode = ohead;
        ohead = ohead.m_child;
        while (ohead != null)
        {
            if (ohead.m_idx == idx)
            {// 如果找到了
                return true;
            }
            else
            {
                ohead = ohead.m_child;
            }

        }
        return false;
    }

    static PathNode getBeginPathNode(int idx)
    {

        PathNode ps;
        ps = new PathNode();
        ps.m_idx = idx;
        return ps;
    }
    //从容器中移除对象; 
    static bool removeNodeFromList(List<PathNode> spriteVector, PathNode sprite)
    {

        for (int i = 0; i < spriteVector.Count; i++)
        {
            PathNode _sp1 = spriteVector[i];
            if (_sp1 == sprite)
            {
                spriteVector.Remove(_sp1);
                return true;
            }
        }

        return false;
    }

    static float max(float a, float b)
    {
        if (a > b)
        {
            return a;
        }
        return b;
    }
    static int hypot(float a, float b)
    {
        return (int)Math.Sqrt(a + b);
    }
    // static int whichfloor;
    //static void AStar();
    public static void AStar1(Role pp, int tidx)
    {

        //if (pp.nextbigkind == 3 || (pp.bigkind == 3 && pp.nextbigkind != -1))
        //{
        //    AStar1(pp,tx, ty, nw, nw);
        //    return;
        //}

        //if (true)
        //{
        //    return;
        //}

        pp.path.Clear();
        pp.pIndex = 0;
        pp.zhaobudao = false;



        m_openList = new List<PathNode>();
        m_closeList = new List<PathNode>();

        m_startidx = pp.idx;


        m_endidx = tidx;



        //if (true)
        //{
        //    pp.path.Insert(0, new Vector2(tx, ty));
        //    return;
        //}

        //得到开始点的节点  
        PathNode _sp = getBeginPathNode(m_startidx);
        //得到开始点的节点  
        PathNode _endNode = getBeginPathNode(m_endidx);
        //因为是开始点 把到起始点的距离设为0  
        _sp.m_costToSource = 0;
        _sp.m_FValue = 0;
        //把已经检测过的点从检测列表中删除  
        //PathSearchInfo::removeObjFromList(PathSearchInfo::m_closeList, _sp);  
        //然后加入开放列表  
        m_openList.Add(_sp);

        PathNode _node = null;
        while (true)
        {
            //得到离起始点最近的点  
            _node = getMinPathFormOpenList();
            if (_node == null)
            {
                //找不到路径
                Debug.Log("zhaobudao");
                pp.zhaobudao = true;
                break;
            }
            m_closeList.Add(_node);
            //把计算过的点从开放列表中删除  
            removeNodeFromList(m_openList, _node);

            int _x = _node.m_idx;

            //log("nowx=%d nowy=%d",_x,_y);

            if (_x == m_endidx)
            {
                break;
            }

            //检测8个方向的相邻节点是否可以放入开放列表中 
            PathNode _adjacent;


            for (int i = 0; i < mappointarray[_x].dbbean.aiidlist.Count; i++)
            {
                int tempidx = mappointarray[_x].dbbean.aiidlist[i];
              //  Debug.LogError(tempidx);
                if(mappointarray[tempidx]==null)
                {
                    continue;
                }
                if (mappointarray[tempidx].dbbean.lockflag)
                {
                    continue;
                }
                _adjacent = getPathNode(mappointarray[_x].dbbean.aiidlist[i]);
                addNodeIntoList(_node, _adjacent, _endNode);
            }





        }
        //  Color ccc = new Color(CTTools.rd.Next(100)*0.01f, CTTools.rd.Next(100) * 0.01f, CTTools.rd.Next(100) * 0.01f);
        // Color3B ccc = Color3B(Tools::getRandom(255), Tools::getRandom(255), Tools::getRandom(255));
        while (_node != null)
        {
            //log("lunowx=%d lunowy=%d",_node->m_x,_node->m_y);
            //pmap->dataArray[_node->m_x][_node->m_y]->text->setColor(ccc);
            // PMap.dataArrayList[whichfloor][_node.m_x, _node.m_y].setColor(ccc);
            //PathSprite* _sp = node;  
            //PathSearchInfo::m_pathList.insert(PathSearchInfo::m_pathList.begin(), _node);  
            if (_node.m_parent != null)
            {
                pp.path.Insert(0, new Vector2(mappointarray[_node.m_idx].dbbean.x, mappointarray[_node.m_idx].dbbean.y));
            }
            _node = _node.m_parent;
        }



        Debug.Log( m_openList.Count+" "+ m_closeList.Count+" "+ pp.path.Count);


    }







    public static int AStarTest(int bidx, int eidx)
    {
        //pp.path.Clear();
        //pp.pIndex = 0;





        m_openList = new List<PathNode>();
        m_closeList = new List<PathNode>();



        m_startidx = bidx;


        m_endidx = eidx;


        if (getPathNode(m_startidx) == null)
        {
            return 0;
        }

     //   Debug.LogError(bidx + " " + eidx);

        //if (true)
        //{
        //    pp.path.Insert(0, new Vector2(ex, ey));
        //    return;
        //}

        //得到开始点的节点  
        PathNode _sp = getBeginPathNode(m_startidx);
        //得到开始点的节点  
        PathNode _endNode = getPathNode(m_endidx);
        // PathNode _endNode = getBeginPathNode(m_endX, m_endY);
        //因为是开始点 把到起始点的距离设为0  
        _sp.m_costToSource = 0;
        _sp.m_FValue = 0;
        //把已经检测过的点从检测列表中删除  
        //PathSearchInfo::removeObjFromList(PathSearchInfo::m_closeList, _sp);  
        //然后加入开放列表  
        m_openList.Add(_sp);

        // AddNode(openhead, _sp);

        PathNode _node = null;
        while (true)
        {
            //得到离起始点最近的点  
            _node = getMinPathFormOpenList();
            if (_node == null)
            {
                //找不到路径
                Debug.Log("zhaobudao");
                break;
            }

            // AddNode(closehead, _node);
            // RemoveNode(openhead, _node);

            //把计算过的点从开放列表中删除  
            m_closeList.Add(_node);
            removeNodeFromList(m_openList, _node);

            int _x = _node.m_idx;


          //  Debug.LogError("nowx="+ _x);

            if (_x == m_endidx)
            {
                break;
            }

            //检测8个方向的相邻节点是否可以放入开放列表中 
            PathNode _adjacent;

            for (int i = 0; i < mappointarray[_x].dbbean.aiidlist.Count; i++)
            {
                int tempidx = mappointarray[_x].dbbean.aiidlist[i];
                //  Debug.LogError(tempidx);
                if (mappointarray[tempidx] == null)
                {
                    continue;
                }
                if (mappointarray[tempidx].dbbean.lockflag)
                {
                    continue;
                }
                _adjacent = getPathNode(mappointarray[_x].dbbean.aiidlist[i]);
                addNodeIntoList(_node, _adjacent, _endNode);
            }



        }

        int num = 0;
        while (_node != null)
        {
          //  Debug.Log(_node.m_idx);
            num++;
            _node = _node.m_parent;
        }

        return num;




    }

    public static List<int> AStarTestgetpathlist(int bidx, int eidx)
    {
        //pp.path.Clear();
        //pp.pIndex = 0;





        m_openList = new List<PathNode>();
        m_closeList = new List<PathNode>();



        m_startidx = bidx;


        m_endidx = eidx;


        if (getPathNode(m_startidx) == null)
        {
            return null;
        }



        //if (true)
        //{
        //    pp.path.Insert(0, new Vector2(ex, ey));
        //    return;
        //}

        //得到开始点的节点  
        PathNode _sp = getBeginPathNode(m_startidx);
        //得到开始点的节点  
        PathNode _endNode = getPathNode(m_endidx);
        // PathNode _endNode = getBeginPathNode(m_endX, m_endY);
        //因为是开始点 把到起始点的距离设为0  
        _sp.m_costToSource = 0;
        _sp.m_FValue = 0;
        //把已经检测过的点从检测列表中删除  
        //PathSearchInfo::removeObjFromList(PathSearchInfo::m_closeList, _sp);  
        //然后加入开放列表  
        m_openList.Add(_sp);

        // AddNode(openhead, _sp);

        PathNode _node = null;
        while (true)
        {
            //得到离起始点最近的点  
            _node = getMinPathFormOpenList();
            if (_node == null)
            {
                //找不到路径
                Debug.Log("zhaobudao");
                break;
            }

            // AddNode(closehead, _node);
            // RemoveNode(openhead, _node);

            //把计算过的点从开放列表中删除  
            m_closeList.Add(_node);
            removeNodeFromList(m_openList, _node);

            int _x = _node.m_idx;


            //log("nowx=%d nowy=%d",_x,_y);

            if (_x == m_endidx)
            {
                break;
            }

            //检测8个方向的相邻节点是否可以放入开放列表中 
            PathNode _adjacent;

            for (int i = 0; i < mappointarray[_x].dbbean.aiidlist.Count; i++)
            {
                _adjacent = getPathNode(mappointarray[_x].dbbean.aiidlist[i]);
                addNodeIntoList(_node, _adjacent, _endNode);
            }



        }

        List<int> num = new List<int>();
        while (_node != null)
        {
            Debug.Log(_node.m_idx);
            num.Insert(0, _node.m_idx);
            _node = _node.m_parent;
        }

        return num;




    }



    private static void AddNode(PathNode ohead, PathNode _sp)
    {
        while (ohead.m_child != null)
        {
            ohead = ohead.m_child;
        }

        ohead.m_child = _sp;
    }

    private static void RemoveNode(PathNode ohead, PathNode _node)
    {
        PathNode preNode = ohead;
        ohead = ohead.m_child;
        while (ohead != null)
        {
            if (ohead == _node)
            {// 如果找到了
                preNode.m_child = ohead.m_child;
                ohead.m_child = null;
                break;
            }
            else
            {
                preNode = ohead;
                ohead = ohead.m_child;
            }
        }
    }

}
