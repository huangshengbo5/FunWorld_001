using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VDTest : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RenderTexture renderTexture;
    public UITexture texture;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        texture = GetComponent<UITexture>();
        renderTexture = new RenderTexture(texture.width, texture.height, 24);
        renderTexture.useMipMap = false;
        videoPlayer.targetTexture = renderTexture;
        texture.mainTexture = renderTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
