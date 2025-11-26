using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ELSTest : MonoBehaviour
{
    int Key_State_Store = 0;
    int Key_State = 0;
    int Key_DownState = 0;
    int Key_UpState = 0;
    int Cur_KeyState = 0;
    int Cur_KeyDownState = 0;
    int Cur_KeyUpState = 0;

    const int KEYS_UP = 1 << 11;
    const int KEYS_DOWN = 1 << 12;
    const int KEYS_LEFT = 1 << 13;
    const int KEYS_RIGHT = 1 << 14;
    const int KEYS_FIRE = 1 << 15;


    int intervalId;
    int count = 0;
    int maxCount = 1000;
    int duration = 10;

    int begin;
    int end;
    int s;
    int GameState = 0;
    int overDelay = 0;

    int xiaohang = 0;
    int dengji = 0;


   public UILabel text1, xiaohangkuang, dengjikuang;

    int mcx = 100;
    int mcy = 100;
    int listx = 1200;
    int listy = 100;
    int w = 10;
    int h = 20;
    const int KuaiWH = 100;
    KuaiTest[][] mcArray = new KuaiTest[20][];
    KuaiTest[][] mcSmallArray = new KuaiTest[4][];
    KuaiTest[][] mcMidArray = new KuaiTest[4][];
    int[][] bigArray = new int[20][];
    int[][] smallArray = new int[4][];
    int smallIndex, smallRot, smallMaxRot;
    int smallMidIndex, smallMidRot;
    int smallx, smally;
    int luoDelay = 0;
    int[][][][] zuzongArray = {
        new int[][][] {
            new int[][] {
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 0, 0 },
                new int[] { 1, 1, 1, 1 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 1, 1, 0, 0 },
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 1, 1, 0, 0 },
                new int[] { 1, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 0, 1, 1, 0 },
                new int[] { 1, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 0, 1, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 1, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 0, 0 },
                new int[] { 1, 1, 1, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 1, 0, 0 },
                new int[] { 1, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 1, 1, 1, 0 },
                new int[] { 0, 0, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 1, 0 },
                new int[] { 0, 0, 1, 0 },
                new int[] { 0, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 1, 0, 0, 0 },
                new int[] { 1, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        },
        new int[][][] {
            new int[][] {
                new int[] { 1, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 1, 0 },
                new int[] { 1, 1, 1, 0 },
                new int[] { 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 1, 0, 0, 0 },
                new int[] { 1, 0, 0, 0 },
                new int[] { 1, 1, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            },
            new int[][] {
                new int[] { 0, 0, 0, 0 },
                new int[] { 1, 1, 1, 0 },
                new int[] { 1, 0, 0, 0 },
                new int[] { 0, 0, 0, 0 }
            }
        }
    };

    public GameObject root;
    public GameObject cc;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    Key_DownState |= KEYS_UP;
        //    Key_State |= KEYS_UP;
        //}
        //if (Input.GetKeyUp(KeyCode.W))
        //{
        //    Key_UpState |= KEYS_UP;
        //    Key_State &= (~KEYS_UP);
        //}


        if (Input.GetKeyDown(KeyCode.W))
        {
            Key_DownState |= KEYS_UP;
            Key_State |= KEYS_UP;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Key_UpState |= KEYS_UP;
            Key_State &= (~KEYS_UP);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Key_DownState |= KEYS_DOWN;
            Key_State |= KEYS_DOWN;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Key_UpState |= KEYS_DOWN;
            Key_State &= (~KEYS_DOWN);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Key_DownState |= KEYS_LEFT;
            Key_State |= KEYS_LEFT;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Key_UpState |= KEYS_LEFT;
            Key_State &= (~KEYS_LEFT);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Key_DownState |= KEYS_RIGHT;
            Key_State |= KEYS_RIGHT;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Key_UpState |= KEYS_RIGHT;
            Key_State &= (~KEYS_RIGHT);
        }
        logic();
    }


    void getKey()
    {
        Cur_KeyDownState = Key_DownState;
        Cur_KeyUpState = Key_UpState;
        Cur_KeyState = Key_State;
        Key_DownState = 0;
        Key_UpState = 0;
    }

    bool testKeyDown(int tkey) {
	return (Cur_KeyDownState & tkey) != 0;
}


    bool testKeyUp(int tkey) {
	return (Cur_KeyUpState & tkey) != 0;
}

    bool testKeyKeep(int tkey) {
	return (Cur_KeyState & tkey) != 0;
}
    void clearKey()
    {
        Key_State_Store = Cur_KeyState;
        Key_State = 0;
        Key_DownState = 0;
        Key_UpState = 0;
        Cur_KeyState = 0;
        Cur_KeyDownState = 0;
        Cur_KeyUpState = 0;
    }


void logic() {
	//trace(begin-end);
	getKey();
	switch (GameState) {
	case 0 :
		playgame();
		break;
	case 1 :
		xiaoChu();
		break;
	case 2 :
		overChuLi();
		break;
	case 3 :
		pauseed();
		break;
	}

	//var tmp=now.getTime()-begin
	//if(tmp>duration){
	//tmp=duration;
	//}
	//clearInterval(intervalId);
	//intervalId = setInterval(this, "executeCallback", duration-tmp);
}

void pauseed(){
	if (testKeyDown(KEYS_UP)) {
		GameState = 0;
		text1.text = "play";
		clearKey();
	}
}

void overChuLi() {
	//trace(h-overDelay-1);
	for (int i=0; i<w; i++) {
		bigArray[h - (overDelay < 20 ? overDelay : 19) - 1][i] = 1;
		mcArray[h - (overDelay < 20 ? overDelay : 19) - 1][i].gotoAndStop(2);
            mcArray[h - (overDelay < 20 ? overDelay : 19) - 1][i].SetActive(true);
	}
	//paint1();
	//updateAfterEvent();
    overDelay++;
	if (overDelay == 25) {
		overFlag = false;
		overDelay = 0;
		GameState = 0;
		xiaohang = 0;
		dengji = 0;
		xiaohangkuang.text = xiaohang.ToString();
	dengjikuang.text = dengji.ToString();
		for (int j=0; j<h; j++) {
			for (int i=0; i<w; i++) {
				bigArray[j][i] = 0;
                    mcArray[j][i].SetActive(false);
			}
		}
		text1.text = "play";
		create();
		//paint1();
	}
}
void playgame() {
	//trace("play");
	if (testKeyDown(KEYS_FIRE)) {
		GameState = 3;
		text1.text = "pause";
		clearKey();
            return;
	}
	//if (testKeyDown(KEYS_9)) {
	//	while (!peng(2)) {
	//		smally++;
	//	}
	//	paint();
	//}

	if (testKeyDown(KEYS_DOWN) || testKeyKeep(KEYS_DOWN)) {
		if (!peng(2)) {
			// mysound.gotoAndPlay("a2");
			smally++;
			paint();
		}
	}
	if (testKeyDown(KEYS_LEFT)) {
		if (!peng(3)) {
			 //mysound.gotoAndPlay("a2");
			smallx--;
			paint();
		}
	}
	if (testKeyDown(KEYS_RIGHT)) {
		if (!peng(4)) {
			// mysound.gotoAndPlay("a2");
			smallx++;
			paint();
		}
	}
	if (testKeyDown(KEYS_UP)) {
		if (canRot()) {

			smallRot = (smallRot+1)%smallMaxRot;
			for (int j=0; j<4; j++) {
				for (int i=0; i<4; i++) {
					smallArray[j][i] = zuzongArray[smallIndex][smallRot][j][i];
				}
			}

			paint();
		}
	}
	//if (testKeyDown(KEYS_3)) {
	//	create();
	//}
	luoDelay++;
	if (luoDelay>10-dengji) {
		luoDelay = 0;
		if (peng(2)) {
			daoDi();
		} else {
			smally++;
		}
		paint();
	}

	if (overFlag) {
		GameState = 2;
		clearKey();
	}
	if (xiaoChuFlag) {

		GameState = 1;
		clearKey();
	}
}



    void init()
    {


        for (int j = 0; j < h; j++)
        {
            mcArray[j] = new KuaiTest[w];
            bigArray[j] = new int[w];
            for (int i = 0; i < w; i++)
            {
                GameObject tempobj = NGUITools.AddChild(root, cc);
                tempobj.transform.localPosition = new Vector3(mcx + i * KuaiWH, mcy + j * KuaiWH);
                //tempobj.SetActive(false);
                mcArray[j][i] = tempobj.GetComponent<KuaiTest>();
                mcArray[j][i].SetActive(false);
                bigArray[j][i] = 0;
            }
        }
        for (int j = 0; j < 4; j++)
        {
            mcSmallArray[j] = new KuaiTest[4];
            smallArray[j] = new int[4];
            mcMidArray[j] = new KuaiTest[4];
            for (int i = 0; i < 4; i++)
            {
                GameObject tempobj = NGUITools.AddChild(root, cc);
                //  tempobj.transform.localPosition = new Vector3(mcx + i * 10, mcy + j * 10);
                //tempobj.SetActive(false);

                mcSmallArray[j][i] = tempobj.GetComponent<KuaiTest>();
                mcSmallArray[j][i].SetActive(false);
                smallArray[j][i] = 0;

                GameObject tempobj0 = NGUITools.AddChild(root, cc);
                tempobj0.transform.localPosition = new Vector3(listx + i * KuaiWH, listy + j * KuaiWH);
                //tempobj0.SetActive(false);

                mcMidArray[j][i] = tempobj0.GetComponent<KuaiTest>();
                mcMidArray[j][i].SetActive(false);

            }
        }
        text1.text = "play";
        xiaohangkuang.text = xiaohang.ToString();
        dengjikuang.text = dengji.ToString();

        smallMidIndex = CTTools.rd.Next(7);
        smallMidRot = CTTools.rd.Next(zuzongArray[smallMidIndex].Length);
        create();
        paint2();
    }
    void create() {
	smallIndex = smallMidIndex;
	smallMaxRot = zuzongArray[smallIndex].Length;
	smallRot = smallMidRot;
        smallMidIndex = CTTools.rd.Next(7);
        smallMidRot = CTTools.rd.Next(zuzongArray[smallMidIndex].Length);
        paint2();
	//trace("index="+smallIndex);
	//trace("rot="+smallRot);
	for (int j=0; j<4; j++) {
		for (int i=0; i<4; i++) {
			smallArray[j][i] = zuzongArray[smallIndex][smallRot][j][i];
		}
	}
	smallx = 3;
	smally = 0;
        int jtemp=0;
	for (int j=0; j<4; j++) {
		var flag = true;
		for (int i=0; i<4; i++) {
			if (smallArray[j][i] == 1) {
				flag = false;
				break;
			}
		}
		if (!flag) {
                jtemp = j;

            break;
		}
	}
	smally = 0-jtemp;
	clearKey();
}
bool peng(int fangxiang) {
	switch (fangxiang) {
	case 2 :
		for (int j=3; j>=0; j--) {
			for (int i=0; i<4; i++) {
				if (smallArray[j][i] == 1) {
					if (smally+j+1>=h) {
						return true;
					}
					if (bigArray[smally + j + 1][smallx + i] != 0) {
						return true;
					}
				}
			}
		}
		break;
	case 3 :
		for (int i=0; i<4; i++) {
			for (int j=0; j<4; j++) {
				if (smallArray[j][i] == 1) {
					if (smallx+i-1<0) {
						return true;
					}
					if (bigArray[smally + j][smallx + i - 1] != 0) {
						return true;
					}
				}
			}
		}
		break;
	case 4 :
		for (int i=3; i>=0; i--) {
			for (int j=0; j<4; j++) {
				if (smallArray[j][i] == 1) {
					if (smallx+i+1>=w) {
						return true;
					}
					if (bigArray[smally + j][smallx + i + 1] != 0) {
						return true;
					}
				}
			}
		}
		break;
	}
	return false;
}
bool overFlag = false;
bool xiaoChuFlag = false;
void daoDi() {
	for (int j=0; j<4; j++) {
		for (int i=0; i<4; i++) {
			if (smallArray[j][i] == 1) {
				bigArray[smally + j][smallx + i] = smallIndex+1;
				mcArray[smally + j][smallx + i].gotoAndStop(smallIndex+2);
mcArray[smally + j][smallx + i].SetActive(true);
				if (smally+j == 0) {
					text1.text = "over";
					overFlag = true;
				}
				smallArray[j][i] = 0;
			}
		}
	}
	paint();
	//paint1();
	if (!overFlag) {
		if (canXiaoChu()) {
			//GameState=1;
			xiaoChuFlag = true;
		} else {
			create();
		}
	}
}
bool canXiaoChu() {
	for (int j=smally+3; j>=smally; j--) {
		if (j>=h) {
			continue;
		}
		bool flag = true;
		for (int i=0; i<w; i++) {
			if (bigArray[j][i] == 0) {
				flag = false;
				break;
			}
		}
		if (flag) {
			return true;
		}
	}
	return false;
}
int xiaochuDelay = 0;
void xiaoChu() {
	if (xiaochuDelay == 0) {
		for (int j=smally+3; j>=smally; j--) {
			if (j>=h) {
				continue;
			}
			bool flag = true;
			for (int i=0; i<w; i++) {
				if (bigArray[j][i] == 0) {
					flag = false;
					break;
				}
			}
			if (flag) {
				for (int i=0; i<w; i++) {
					mcArray[j][i].gotoAndPlay(bigArray[j][i]*10+1);
				}
			}
 
		}
	}
	//trace(xiaochuDelay);  
	xiaochuDelay++;
	if (xiaochuDelay>9) {
		for (int j=smally+3; j>=smally; j--) {
			if (j>=h) {
				continue;
			}
			bool flag = true;
			for (int i=0; i<w; i++) {
				if (bigArray[j][i] == 0) {
					flag = false;
					break;
				}
			}
			if (flag) {
				for (int l = j; l>=1; l--) {
					for (int i=0; i<w; i++) {
                            Debug.Log(l + " " + i);
						bigArray[l][i] = bigArray[l - 1][i];

					}
				}
				for (int i=0; i<w; i++) {
					bigArray[0][i] = 0;
				}
				xiaohang++;
				xiaohangkuang.text = xiaohang.ToString();
				if (xiaohang%50 == 0) {
					dengji++;
					dengjikuang.text = dengji.ToString();
				}
				j++;
				//paint1();
			}
		}
		for (int j=0; j<h; j++) {
			for (int i=0; i<w; i++) {
				if (bigArray[j][i] != 0) {
					mcArray[j][i].gotoAndStop(bigArray[j][i]+1);
mcArray[j][i].SetActive(true);
				} else {
					mcArray[j][i].SetActive(false);
				}
			}
		}
		xiaoChuFlag = false;
		xiaochuDelay = 0;
		GameState = 0;
		create();
	}
}
bool canRot() {
	if(smallIndex==0){
		return false;
	}
	int temp = (smallRot + 1) % smallMaxRot;
	for (int j=0; j<4; j++) {
		for (int i=0; i<4; i++) {
			if (zuzongArray[smallIndex][temp][j][i] == 1) {
				if (smallx+i<0 || smallx+i>=w || smally+j<0 || smally+j>=h) {
					return false;
				}
				if (bigArray[smally + j][smallx + i] != 0) {
					return false;
				}
			}
		}
	}
	return true;
}
void paint() {

	for (int j=0; j<4; j++) {
		for (int i=0; i<4; i++) {

			if (smallArray[j][i] == 1) {
				
				//mcSmallArray[j][i]._x=mcx+i*10+smallx*10;
				//mcSmallArray[j][i]._y=mcy+j*10+smally*10;
                    mcSmallArray[j][i].transform.localPosition = new Vector3(mcx + i * KuaiWH + smallx * KuaiWH, mcy + j * KuaiWH + smally * KuaiWH);
                                    mcSmallArray[j][i].gotoAndStop(smallIndex+2);
mcSmallArray[j][i].SetActive(true);
                }
                else{
				mcSmallArray[j][i].SetActive(false);
                }
		}
	}

}

    void paint2()
    {
        //trace("paint2");
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 4; i++)
            {

                if (zuzongArray[smallMidIndex][smallMidRot][j][i] == 1)
                {

                    mcMidArray[j][i].gotoAndStop(smallMidIndex + 2);
                    mcMidArray[j][i].SetActive(true);
                }
                else
                {
                    mcMidArray[j][i].SetActive(false);
                }
            }
        }
    }
    //init();
    //beginInterval();

}
