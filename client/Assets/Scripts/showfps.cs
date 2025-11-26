using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class showfps : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    float mDelta = 0f;
    int mFPS =0;
	// Update is called once per frame
	void Update () {
			mDelta += Time.deltaTime;
            mFPS++;
            if (mDelta>1 )
            {
             gameObject.GetComponent<UILabel>().text = mFPS.ToString();

            mDelta = 0;
                mFPS = 0;
            }
	}
    public void changcolor()
    {
        gameObject.GetComponent<UILabel>().color = Color.black;
    }
}
