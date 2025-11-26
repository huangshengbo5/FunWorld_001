using UnityEngine;
using System.Collections;
using System;


public delegate void jumpover(int code);

public class JumpTo : MonoBehaviour
{
    Vector3 from, to;
    float _height, _heightback;
    public Vector3 _delta, _previousPos, _startPosition, _deltacha;
    int _jumps;
    public float _elapsed, _duration;
    public float y0, y1, y2;
    bool initleflag = false;
    public bool overflag = false;
    bool shuijianflag = false;
    bool jiasuflag = false;
    // Use this for initialization

    public jumpover mycallback;
    void Start()
    {

    }
    public void creat(Vector3 md, float height, int jptime, float time, bool sj, bool js)
    {
        _delta = md;
        _deltacha = md - gameObject.transform.localPosition;
        _height = height;
        _heightback = _height;
        _jumps = jptime;
        _duration = time;
        _elapsed = 0;
        _previousPos = gameObject.transform.localPosition;
        _startPosition = gameObject.transform.localPosition;

        shuijianflag = sj;
        initleflag = true;
        overflag = false;
        jiasuflag = js;

        y0 = 0;
        y1 = 0;
        y2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (mycallback != null)
        {
            logic(Time.deltaTime);
        }

    }

    private void update(float t)
    {
        float frac = t * _jumps % 1.0f;
        float y = _height * 4 * frac * (1 - frac);


        y += _deltacha.y * t;
        //    Debug.Log("height=" + y+"  "+(frac * (1 - frac)).ToString()+" "+frac);
        y2 = y1;
        y1 = y0;
        y0 = y;

        if (shuijianflag)
        {
            if ((y2 - y1) > 0 && (y1 - y0) < 0)
            {
                _height = _heightback - t * _heightback;
                y0 = 0;
                y1 = 0;
                y2 = 0;
                //  Debug.Log("==============" );
            }
        }


        float x = _deltacha.x * t;
        Vector3 currentPos = gameObject.transform.localPosition;

        Vector3 diff = currentPos - _previousPos;
        _startPosition = diff + _startPosition;

        Vector3 newPos = _startPosition + new Vector3(x, y, 0);


        gameObject.transform.localPosition = newPos;

        _previousPos = newPos;
    }

    public void logic(float delta)
    {
        if (initleflag)
        {
            float t = delta;
            _elapsed += t;

            if (_elapsed < _duration)
            {
                float time = Math.Max(0, Math.Min(1, _elapsed / _duration));
                if (jiasuflag)
                {
                    time = Mathf.Pow(time, 2.5f);
                    // time = -1 * Mathf.Cos(time * (float)Math.PI/2.0f) + 1;
                }
                update(time);
            }
            else
            {
                gameObject.transform.localPosition = _delta;
                overflag = true;
                if (mycallback != null)
                {
                    mycallback(0);
                    Destroy(gameObject);
                }
            }
        }
    }
    public void setover()
    {
        _elapsed = _duration;
        overflag = true;
    }
}
