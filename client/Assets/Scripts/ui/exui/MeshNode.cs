using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshWidget))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshNode : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    Mesh mesh;
   
    // Start is called before the first frame update
    void Awake()
    {
        //Material mattemp = new Material(Shader.Find("myshader/yellow"));
        //mattemp.mainTexture = ResManager.getTex11("Out.png");

        Material mattemp = new Material(Shader.Find("Sprites/Default"));
        mattemp.color = new Color(Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.3f);
        mattemp.color = Color.red;

        meshFilter = gameObject.GetComponent<MeshFilter>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = mattemp;
      //  meshRenderer.sharedMaterials=new Material[] { mattemp};
        mesh = meshFilter.mesh;
        mesh.Clear();
       // DrawCircle(800, 50, Vector3.zero);
    }
    public void settex(Texture2D zhong)
    {
        Material mattemp = new Material(Shader.Find("Unlit/Transparent Colored"));
        mattemp.mainTexture = zhong;
        meshRenderer.material = mattemp;
    }
    void Start()
    {
        //DoCreatPloygonMesh(s_Vertives);
        // DrawCircle(80, 50, Vector3.zero);

        //Vector3[] vertices = new Vector3[5];
        //float currentAngle = 0;
        //Vector3 centerCircle = Vector3.zero;
        //vertices[0] = centerCircle;
        //float radius = 80;
        //float deltaAngle = -72;
        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    float cosA = Mathf.Cos(currentAngle);
        //    float sinA = Mathf.Sin(currentAngle);
        //    vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
        //    currentAngle += deltaAngle;
        //}

        //float[] bili = new float[6] {0, 0.01f,0.2f,0.8f,0.4f,0.5f };

        //for(int i = 0; i < bili.Length; i++)
        //{
        //   // bili[i] = 1;
        //}

        //Vector3[] vertices = new Vector3[5 + 1];
        //float radius = 200;
        //Vector3 centerCircle = Vector3.zero;
        //vertices[0] = centerCircle;
        //float deltaAngle = Mathf.Deg2Rad * 360f / 5;
        //float currentAngle = 0;
        //for (int i = 1; i < vertices.Length; i++)
        //{
        //    float cosA = Mathf.Cos(currentAngle);
        //    float sinA = Mathf.Sin(currentAngle);
        //    vertices[i] = new Vector3(cosA * radius*bili[i] + centerCircle.x, sinA * radius * bili[i] + centerCircle.y, 0);
        //    currentAngle += deltaAngle;
        //}
        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    Debug.Log(vertices[i]);
        //}

        //drawMesh(vertices, Color.red);
    }
   // public float[] bili = new float[6] { 0, 0.01f, 0.2f, 0.8f, 0.4f, 0.5f };
    // Update is called once per frame
    void Update()
    {

    }

    public void drawMesh(Vector3[] s_Vertives, Color color)
    {

        meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.3f*color.a);

       // meshRenderer.material.color = color;
    
        int segments = s_Vertives.Length - 1;
     //   Debug.Log(segments);
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;

        //赋值多边形顶点
        mesh.vertices = s_Vertives;
        //赋值三角形点排序
        mesh.triangles = triangles;

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


    }
    public void drawMesh1(Vector3[] s_Vertives, Color color)
    {

        //meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.3f);

        meshRenderer.material.color = color;

        //存储所有的顶点
        Vector3[] tVertices = s_Vertives;

        //存储画所有三角形的点排序
        List<int> tTriangles = new List<int>();
        //根据所有顶点填充点排序
        for (int i = 0; i < tVertices.Length - 1; i++)
        {
            tTriangles.Add(i);
            tTriangles.Add(i + 1);
            tTriangles.Add(tVertices.Length - i - 1);
        }
        List<int> lists = new List<int>();
        for (int i = 1; i < s_Vertives.Length; i++)
        {
            lists.Add(i);
        }
        //  Debug.Log("i=0;" + s_Vertives[0] + " i=len-1:" + s_Vertives[s_Vertives.Length - 1]);
          int[] trianglesex = Triangulation.WidelyTriangleIndex(new List<Vector3>(s_Vertives), lists).ToArray();




        //赋值多边形顶点
        mesh.vertices = s_Vertives;
        //赋值三角形点排序
        mesh.triangles = trianglesex;

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


    }

    public void drawMesh2(Vector3[] s_Vertives, Color color)
    {

        //meshRenderer.material.color = new Color(color.r, color.g, color.b, 0.3f);

        meshRenderer.material.color = color;

        Vector2[] vertices2D = new Vector2[s_Vertives.Length-1];
        List<int> lists = new List<int>();
        for (int i = 1; i < s_Vertives.Length; i++)
        {
            vertices2D[i] = new Vector2(s_Vertives[i].x, s_Vertives[i].y);
            lists.Add(i);
        }


        Triangulator tr = new Triangulator(vertices2D);
        int[] trianglesex = tr.Triangulate();

        //赋值多边形顶点
        mesh.vertices = s_Vertives;
        //赋值三角形点排序
        mesh.triangles = trianglesex;

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


    }

    void DrawCircle(float radius, int segments, Vector3 centerCircle)
    {


        //顶点
        Vector3[] vertices = new Vector3[segments + 1];
        vertices[0] = centerCircle;
        float deltaAngle = Mathf.Deg2Rad * 360f / segments;
        float currentAngle = 0;
        for (int i = 1; i < vertices.Length; i++)
        {
            float cosA = Mathf.Cos(currentAngle);
            float sinA = Mathf.Sin(currentAngle);
            vertices[i] = new Vector3(cosA * radius + centerCircle.x, sinA * radius + centerCircle.y, 0);
            currentAngle += deltaAngle;
        }

        //三角形
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;



        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
    Vector3[] s_Vertives;
    Color color;
    public void setdata(Vector3[] s_Vertives, Color color)
    {
        this.s_Vertives = s_Vertives;
        this.color = color;
        meshRenderer.material.color = color;
    }

    public void DoCreatPloygonMesh(Vector3[] s_Vertives)
    {

        //存储所有的顶点
        Vector3[] tVertices = s_Vertives;

        //存储画所有三角形的点排序
        List<int> tTriangles = new List<int>();
        //根据所有顶点填充点排序
        for (int i = 0; i < tVertices.Length - 1; i++)
        {
            tTriangles.Add(i);
            tTriangles.Add(i + 1);
            tTriangles.Add(tVertices.Length - i - 1);
        }

        List<int> lists = new List<int>();
        for (int i = 0; i < s_Vertives.Length; i++)
        {
            lists.Add(i);
        }
        //Debug.Log("i=0;" + s_Vertives[0] + " i=len-1:" + s_Vertives[s_Vertives.Length - 1]);
        int[] trianglesex = Triangulation.WidelyTriangleIndex(new List<Vector3>(s_Vertives), lists).ToArray();

        //赋值多边形顶点
        mesh.vertices = s_Vertives;

        //赋值三角形点排序
        mesh.triangles = trianglesex;// tTriangles.ToArray();// tTriangles.ToArray();// triangles;//tTriangles.ToArray();trianglesex;// 

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();



    }

    public void drawMeshNew(Vector3[] s_Vertives, Vector2[] uv, int[] triangles, Color color)
    {

        meshRenderer.material.color = color;



        mesh.vertices = s_Vertives;
        mesh.uv = uv;
        mesh.triangles = triangles;

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


    }

    public void drawMeshTex(Vector3[] s_Vertives, Vector2[] uv, Color color)
    {

        meshRenderer.material.color = color;


        int segments = s_Vertives.Length - 1;
        int[] triangles = new int[segments * 3];
        for (int i = 0, j = 1; i < segments * 3 - 3; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j;
        }
        triangles[segments * 3 - 3] = 0;
        triangles[segments * 3 - 2] = 1;
        triangles[segments * 3 - 1] = segments;



        mesh.vertices = s_Vertives;
        mesh.uv = uv;
        mesh.triangles = triangles;

        //重新设置UV，法线
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();


    }
}
