using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{   
    /// <summary>
    /// 要检测的层级
    /// </summary>
    [SerializeField] private LayerMask layerMask;
    private Mesh mesh;

    //视锥起点
    private Vector3 origin;
    //初始角度
    private float startingAngle;
    //视野范围
    private float fov;
    //视野距离
    private float viewDistance;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        origin = Vector3.zero;
        fov = 60f;
        startingAngle = 135;
    }

  void LateUpdate() {
        //线条数量，用于等分扇形区域
        int rayCount = 60;

        float angle = startingAngle;

        //角度增量
        float angleIncrease = fov / rayCount;
        viewDistance = 20f;


        //顶点数组
        Vector3[] vertices = new Vector3[rayCount + 1 + 1];//第一个+1为原点，第二个+1为防止索引越界

        Vector2[] uv = new Vector2[vertices.Length];

        int[] triangles = new int[rayCount * 3];//rayCount代表三角形的数量，一个三角形三个顶点

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex  = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            // RaycastHit2D raycastHit2D = Physics2D.Raycast(origin, Utility.GetVectorFormAngle(angle), viewDistance, layerMask);

            // if(raycastHit2D.collider == null)
            // {
                  //No hit
            //     vertex = origin + Utility.GetVectorFormAngle(angle) * viewDistance;
            // }
            // else{
                   //Hit object
            //     vertex = raycastHit2D.point;
            // }

            //Debug.DrawRay(origin, Utility.GetVectorFormAngle(angle),viewDistance)
            
             Debug.DrawLine(transform.position,transform.position + viewDistance * Utility.GetVectorFormAngle(angle),Color.black );
            if(Physics.Raycast(transform.position, Utility.GetVectorFormAngle(angle),out RaycastHit hitInfo,viewDistance,layerMask))
            {
                //Debug.DrawLine
                // Hit object
                vertex = hitInfo.point;
                Debug.Log(hitInfo.transform.position);
                Debug.Log(angle);
                Debug.Log(hitInfo.distance);
            }
            else
            {
                //No hit
                vertex = transform.position + Utility.GetVectorFormAngle(angle) * viewDistance;
            }
            
            vertices[vertexIndex] = vertex;

            if( i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
    
    /// <summary>
    /// 设置视野范围的起点
    /// </summary>
    /// <param name="origin"></param>
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }
    
    public void SetFov(float fov)
    {
        this.fov = fov;
    }

    /// <summary>
    /// 设置视野的方向
    /// </summary>
    /// <param name="aimDirection"></param>
    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = Utility.GetAngleFromVectorFloat(aimDirection) + fov / 2f;
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }

}
