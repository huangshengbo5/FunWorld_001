using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 这个类主要做了一件事,就是优化了,NGUI UIScrollView 在数据量很多都时候,
/// 创建过多都GameObject对象,造成资源浪费.
/// </summary>
public class LoopScrollView : MonoBehaviour
{
	public enum ArrangeDirection
	{
		Left_to_Right,
		Right_to_Left,
		Up_to_Down,
		Down_to_Up,
	}


    public LoopScrollBar loopScrollBar = null;


    /// <summary>
    /// items的排列方式
    /// </summary>
    public ArrangeDirection arrangeDirection = ArrangeDirection.Up_to_Down;

	/// <summary>
	/// 列表单项模板
	/// </summary>
	public GameObject itemPrefab;

	/// <summary>
	/// The items list.
	/// </summary>
	public List<LoopItemObject> itemsList;
	/// <summary>
	/// The datas list.
	/// </summary>
	public List<LoopItemData> datasList;

	/// <summary>
	/// 列表脚本
	/// </summary>
	public UIScrollView scrollView;

	public GameObject itemParent;

    /// <summary>
    /// itemsList的第一个元素
    /// </summary>
    public LoopItemObject firstItem;
    /// <summary>
    /// itemsList的最后一个元素
    /// </summary>
    public LoopItemObject lastItem;


	public delegate void DelegateHandler(LoopItemObject item, LoopItemData data);
	/// <summary>
	/// 响应
	/// </summary>
	public DelegateHandler OnItemInit;

	/// <summary>
	/// 第一item的起始位置
	/// </summary>
	public Vector3 itemStartPos = Vector3.zero;
	/// <summary>
	/// 菜单项间隙
	/// </summary>
	public float gapDis = 0f;

	// 对象池
	// 再次优化，频繁的创建与销毁
	Queue<LoopItemObject> itemLoop = new Queue<LoopItemObject>();


    //标记参数
    public float initLocalX = 0;
    public float initLocalY = 0;
    public float totlePanelWidth = 0;
    

	void Awake()
	{
		if(itemPrefab==null || scrollView==null || itemParent==null)
		{
			//Debug.LogError("Lzh_LoopScrollView.Awake() 有属性没有在inspector中赋值");
		}

		// 设置scrollview的movement
		if(arrangeDirection == ArrangeDirection.Up_to_Down || 
		   arrangeDirection == ArrangeDirection.Down_to_Up)
		{
			scrollView.movement = UIScrollView.Movement.Vertical;

            totlePanelWidth = this.GetComponent<UIPanel>().GetViewSize().y;
        }
		else
		{
			scrollView.movement = UIScrollView.Movement.Horizontal;

            totlePanelWidth = this.GetComponent<UIPanel>().GetViewSize().x;
        }

        //-----------------
        initLocalX = this.transform.localPosition.x;
        initLocalY = this.transform.localPosition.y;
        
    }
	
	// Update is called once per frame
	void Update ()
	{
		//if(scrollView.isDragging)
        {
            Validate();
        }
	}

	/// <summary>
	/// 检验items的两端是否要补上或删除
	/// </summary>
	void Validate()
	{
		if( datasList==null || datasList.Count==0)
		{
			return;
		}

        // 如果itemsList还不存在
        if(itemsList==null || itemsList.Count==0)
        {
            itemsList = new List<LoopItemObject>();

			LoopItemObject item = GetItemFromLoop();
            InitItem(item, 0, datasList[0]);
            firstItem = lastItem = item;
            itemsList.Add(item);

            Validate();
        }
        

		// 先判断前端是否要增减
		if(firstItem.widget.isVisible)
		{
			// 判断要不要在它的前面补充一个item
			if(firstItem.dataIndex>0)
			{
				LoopItemObject item = GetItemFromLoop();

				// 初化：数据索引、大小、位置、显示
				int index = firstItem.dataIndex-1;
				//InitItem(item, index, datasList[index]);
				AddToFront(firstItem, item, index, datasList[index]);
				firstItem = item;
				itemsList.Insert(0,item);

                Validate();
			}
		}
		else
		{
			// 判断要不要将它移除
			// 条件：自身是不可见的；且它后一个item也是不可见的（或被被裁剪过半的）.
			// 		这有个隐含条件是itemsList.Count>=2.
			if(itemsList.Count>=2
			   && itemsList[0].widget.isVisible==false
			   && itemsList[1].widget.isVisible==false)
			{
                itemsList.Remove(firstItem);
				PutItemToLoop(firstItem);
                firstItem = itemsList[0];

                Validate();
			}
		}

		// 再判断后端是否要增减
		if(lastItem.widget.isVisible)
		{
			// 判断要不要在它的后面补充一个item
			if(lastItem.dataIndex < datasList.Count-1)
			{
				LoopItemObject item = GetItemFromLoop();

				// 初化：数据索引、大小、位置、显示
				int index = lastItem.dataIndex+1;
				AddToBack(lastItem, item, index, datasList[index]);
				lastItem = item;
                itemsList.Add(item);

                Validate();
			}
		}
		else
		{
			// 判断要不要将它移除
			// 条件：自身是不可见的；且它前一个item也是不可见的（或被被裁剪过半的）.
			// 		这有个隐含条件是itemsList.Count>=2.
			if(itemsList.Count>=2
				&& itemsList[itemsList.Count-1].widget.isVisible==false
			   	&& itemsList[itemsList.Count-2].widget.isVisible==false)
			{
				itemsList.Remove(lastItem);
				PutItemToLoop(lastItem);
				lastItem = itemsList[itemsList.Count-1];

                Validate();
			}
		}

        if(loopScrollBar != null)
        {
            loopScrollBar.updataBar(datasList, firstItem, this);
        }

    }

	/// <summary>
	/// Init the specified datas.
	/// </summary>
	/// <param name="datas">Datas.</param>
	public void Init(List<LoopItemData> datas, DelegateHandler onItemInitCallback)
	{
		datasList = datas;
		this.OnItemInit = onItemInitCallback;

		Validate();

        initLocalX = this.transform.localPosition.x;
    }

    public void RemoveNode(int id)
    {



        int goalIndex = -1;
        int goalID = -1;
        for (int i = 0; i < datasList.Count; i++)
        {
            if (datasList[i].id == id)
            {
                goalID = id;
                goalIndex = i;
                break;
            }
        }

        if (goalID != -1)
        {
            LoopItemData data = datasList[goalIndex];
            datasList.Remove(data);

            for (int i = 0; i < itemsList.Count; i++)
            {

                LoopItemObject itemObject = itemsList[i];

                if (itemObject.dataIndex == goalIndex)
                {

                    if (firstItem == itemsList[i]) //删除的是列表头
                    {
                        itemsList.Remove(firstItem);
                        PutItemToLoop(firstItem);
                        if (itemsList.Count > 0)
                        {
                            firstItem = itemsList[0];
                        }
                        else
                        {
                            firstItem = null;
                        }

                        for (int z = 0; z < itemsList.Count; z++)
                        {
                            itemsList[z].dataIndex -= 1;
                        }

                    }
                    else if (lastItem == itemsList[i]) //删除的是列表尾
                    {
                        itemsList.Remove(lastItem);
                        PutItemToLoop(lastItem);
                        lastItem = itemsList[itemsList.Count - 1];
                    }
                    else
                    {
                        LoopItemObject tempObj = itemsList[i];
                        itemsList.Remove(tempObj);
                        PutItemToLoop(tempObj);

                        for (int z = i; z < itemsList.Count; z++)
                        {
                            itemsList[z].dataIndex -= 1;
                        }
                    }
                    reviseItemOffset(i);
                    return;
                }
            }
            Debug.Log("删除节点失败，未找到需要删除的ITEM.");
        }
        else
        {
            Debug.Log("删除节点失败...  未找到该ID.");
        }





        /* int goalIndex = -1;
         int goalID = -1;
         for(int i = 0; i < datasList.Count; i++)
         {
             if(datasList[i].id == id)
             {
                 goalID = id;
                 goalIndex = i;
                 break;
             }
         }

         if (goalID != -1)
         {
             LoopItemData data = datasList[goalIndex];
             datasList.Remove(data);

             for (int i = 0; i < itemsList.Count; i++)
             {
                 if (data.id == goalID)
                 {

                     if (firstItem == itemsList[i]) //删除的是列表头
                     {
                         itemsList.Remove(firstItem);
                         PutItemToLoop(firstItem);
                         if(itemsList.Count > 0)
                         {
                             firstItem = itemsList[0];
                         }else
                         {
                             firstItem = null;
                         }

                         for(int z = 0; z < itemsList.Count; z++)
                         {
                             itemsList[z].dataIndex -= 1;
                         }

                     }
                     else if(lastItem == itemsList[i]) //删除的是列表尾
                     {
                         itemsList.Remove(lastItem);
                         PutItemToLoop(lastItem);
                         lastItem = itemsList[itemsList.Count - 1];
                     }
                     else
                     {
                         LoopItemObject tempObj = itemsList[i];
                         itemsList.Remove(tempObj);
                         PutItemToLoop(tempObj);

                         for (int z = i; z < itemsList.Count; z++)
                         {
                             itemsList[z].dataIndex -= 1;
                         }
                     }
                     reviseItemOffset(i);
                     return;
                 }
             }
             Debug.Log("删除节点失败，未找到需要删除的ITEM.");
         }
         else
         {
             Debug.Log("删除节点失败...  未找到该ID.");
         }*/
    }

    //显示到某index
    public void setShowIndex(int id)
    {
        int goalIndex = -1;
        int goalID = -1;
        for (int i = 0; i < datasList.Count; i++)
        {
            if (datasList[i].id == id)
            {
                goalID = id;
                goalIndex = i;
                break;
            }
        }

        if (goalID != -1)
        {
           /* firstItem = itemsList[goalIndex];
            PutItemToLoop(firstItem);*/

        }
        else
        {
            Debug.Log("没有找到该id.");
        }
    }

    private void reviseItemOffset(int index)
    {
        float offset = 0;
        float itemSize = 0;
        Vector3 firstVec = new Vector3();
        if(itemsList.Count > 0)
        {
            if (scrollView.movement == UIScrollView.Movement.Vertical)
            {
                itemSize = itemsList[0].widget.height;
                offset = itemSize + gapDis;
                firstVec = new Vector3(0f, offset, 0f);
            }
            else
            {
                itemSize = itemsList[0].widget.width;
                offset = itemSize + gapDis;
                firstVec = new Vector3(offset, 0f, 0f);
            }
        }
        

        for (int i = 0; i < itemsList.Count; i++)
        {
            if (i >= index)
            {
                itemsList[i].widget.cachedTransform.localPosition -= firstVec;
            }
        }
        

        UIPanel mPanel = this.GetComponent<UIPanel>();
        UIScrollView sv = mPanel.GetComponent<UIScrollView>();
        if (sv != null)
        {
            sv.Press(false);  
        }
    }



    public void setItemPrefab(GameObject itemPrefab)
    {
        this.itemPrefab = itemPrefab;
    }



	/// <summary>
	/// 构造一个 item 对象
	/// </summary>
	/// <returns>The item.</returns>
	LoopItemObject CreateItem()
	{
		GameObject go = NGUITools.AddChild(itemParent,itemPrefab);
        UIWidget widget = go.GetComponent<UIWidget>();
        LoopItemObject item = new LoopItemObject();
        item.widget = widget;
		go.SetActive(true);
        return item;
	}

	/// <summary>
	/// 用数据列表来初始化scrollview
	/// </summary>
	/// <param name="item">Item.</param>
	/// <param name="indexData">Index data.</param>
	/// <param name="data">Data.</param>
	void InitItem(LoopItemObject item, int dataIndex, LoopItemData data)
	{
		item.dataIndex = dataIndex;
        if(OnItemInit!=null)
        {
            OnItemInit(item, data);
        }
		item.widget.transform.localPosition = itemStartPos;
	}

	/// <summary>
	/// 在itemsList前面补上一个item
	/// </summary>
	void AddToFront(LoopItemObject priorItem, LoopItemObject newItem, int newIndex, LoopItemData newData)
	{
		InitItem (newItem, newIndex, newData);
		// 计算新item的位置
		if(scrollView.movement == UIScrollView.Movement.Vertical)
		{
			float offsetY = priorItem.widget.height*0.5f + gapDis + newItem.widget.height*0.5f;
			if(arrangeDirection == ArrangeDirection.Down_to_Up) offsetY *=-1f;
			newItem.widget.transform.localPosition = priorItem.widget.cachedTransform.localPosition + new Vector3(0f, offsetY, 0f);
		}
		else
		{
			float offsetX = priorItem.widget.width*0.5f + gapDis + newItem.widget.width*0.5f;
			if(arrangeDirection == ArrangeDirection.Right_to_Left) offsetX *=-1f;
			newItem.widget.transform.localPosition = priorItem.widget.cachedTransform.localPosition - new Vector3(offsetX, 0f, 0f);
		}
	}

	/// <summary>
	/// 在itemsList后面补上一个item
	/// </summary>
	void AddToBack(LoopItemObject backItem, LoopItemObject newItem, int newIndex, LoopItemData newData)
	{
		InitItem (newItem, newIndex, newData);
		// 计算新item的位置
		if(scrollView.movement == UIScrollView.Movement.Vertical)
		{
			float offsetY = backItem.widget.height*0.5f + gapDis + newItem.widget.height*0.5f;
			if(arrangeDirection == ArrangeDirection.Down_to_Up) offsetY *=-1f;
			newItem.widget.transform.localPosition = backItem.widget.cachedTransform.localPosition - new Vector3(0f, offsetY, 0f);
		}
		else
		{
			float offsetX = backItem.widget.width*0.5f + gapDis + newItem.widget.width*0.5f;
			if(arrangeDirection == ArrangeDirection.Right_to_Left) offsetX *=-1f;
			newItem.widget.transform.localPosition = backItem.widget.cachedTransform.localPosition + new Vector3(offsetX, 0f, 0f);
		}
	}


	#region 对象池性能相关
	/// <summary>
	/// 从对象池中取行一个item
	/// </summary>
	/// <returns>The item from loop.</returns>
	LoopItemObject GetItemFromLoop()
	{
		LoopItemObject item;
		if(itemLoop.Count<=0)
		{
			item = CreateItem();
		}
		else
		{
			item = itemLoop.Dequeue();
		}
		item.widget.gameObject.SetActive(true);
		return item;
	}
	/// <summary>
	/// 将要移除的item放入对象池中
	/// --这个里我保证这个对象池中存在的对象不超过3个
	/// </summary>
	/// <param name="item">Item.</param>
	void PutItemToLoop(LoopItemObject item)
	{
		if(itemLoop.Count>=3)
		{
			Destroy(item.widget.gameObject);
			return;
		}
		item.dataIndex = -1;
		item.widget.gameObject.SetActive(false);
		itemLoop.Enqueue(item);
	}
	#endregion

    public void clear()
    {
        //P
        scrollView.transform.position = new Vector3(0, 0, 0);
        scrollView.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        //消除动画
        SpringPanel spring = this.GetComponent<SpringPanel>();
        if(spring != null)
        {
            spring.target = new Vector3(0, 0, 0);
        }

        //清理child
        UTools.clearChild(itemParent);

        if (itemsList != null)
        {
            itemsList.Clear();
        }

        if(itemLoop != null)
        {
            itemLoop.Clear();
        }
        
        if (datasList != null)
        {
            datasList.Clear();
        }
    }

}
