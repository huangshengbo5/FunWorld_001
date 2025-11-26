using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public HomeLayer home;
    public WorldLayer world;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void sethome()
    {
        home.gameObject.SetActive(true);
        world.gameObject.SetActive(false);
        MusicPlayer.getInstance().PlayW("City", true);
    }
    public void setworld()
    {
        home.gameObject.SetActive(false);
        world.gameObject.SetActive(true);
        MusicPlayer.getInstance().PlayW("Map", true);
        world.geguai();
        
    }
}
