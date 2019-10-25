using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Parabola : MonoBehaviour
{
    public Transform target0;
    public Transform target1;
    [Range(1, 256)]
    [SerializeField] int resolution = 1;//line resolution
    [Range(0.1f, 100f)]
    [SerializeField] float height = 0.1f;

    LineRenderer line;
    private void Awake()
    {
        line = GetComponent<LineRenderer>();
    }
    private void Update()
    {
        GenerateParabola();
    }
    public void GenerateParabola()
    {
        if(target0!=null&&target1!=null)
            DrawParabola();
    }
    /*Eqation
       x^2 = 4p(y-height) :p<0 
        => draw between 
        pos0(sqrt(-4p*height):a, 0)~ pos1(b:b<0,c)
    */
    private void DrawParabola()
    {
        Vector3 pos0 = target0.position;
        Vector3 pos1 = target1.position;

        float deltaX = (pos1.x - pos0.x) / resolution;
        float deltaZ = (pos1.z- pos0.z) / resolution;

        float p,a,b,c = pos1.y-pos0.y;
        float h = c <= height ? height : c;
        Vector3[] vertices = new Vector3[resolution + 1];

        if (c == 0)
        {
            a = (pos0.x - pos1.x) * 0.5f;
            b = -a;

            p = a * a / (-4 * h);
            if (p == 0)
            {
                StraightLine();
                return;
            }
        }
        else
        {
            p = pos0.x - pos1.x;
            p /= Mathf.Sqrt(4 * h) + Mathf.Sqrt(4 * (h - c));
            p*=-p;
            if (p == 0)
            {
                StraightLine();
                return;
            }
            
            a = deltaX >= 0 ? -Mathf.Sqrt(4 * p * -h) : Mathf.Sqrt(4 * p * -h);
            b = deltaX >= 0 ? Mathf.Sqrt(4 * p * (c - h)): -Mathf.Sqrt(4 * p * (c - h));
        }

        //setVertices
        vertices[0] = pos0;
        vertices[resolution] = pos1;
        for (int i = 1; i < resolution; i++)
        {
            float x = a + (deltaX * i);
            float y = x * x / (4 * p) + h;
            vertices[i].x = pos0.x+x-a;
            vertices[i].z = pos0.z + (deltaZ * i);
            vertices[i].y = pos0.y+y;
        }

        //setLine
        line.positionCount = resolution + 1;
        line.SetPositions(vertices);
    }
    void StraightLine() {
        line.positionCount = 2;
        line.SetPosition(0, target0.position);
        line.SetPosition(1, target1.position);
    }
}
