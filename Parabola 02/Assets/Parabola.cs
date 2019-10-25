using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Parabola : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform startPos;
    
    //about raycast
    [SerializeField] string rayCheckTag;
    [SerializeField] float distance = 100f;

    //about draw line
    [Range(1, 256)]
    [SerializeField] int resolution = 1;
    [Range(0.1f, 100f)]
    [SerializeField] float height = 0.1f;

    LineRenderer line;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;//remove line
    }
    private void Update()
    {
        GenerateParabola();
    }
    public void GenerateParabola()
    {
        if (cam!=null&&startPos!=null)
        {
            Ray ray = new Ray(startPos.position, cam.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, distance))
            {
                if (hit.transform.CompareTag(rayCheckTag))
                {
                    DrawParabola(hit.point);
                }else
                    line.positionCount = 0;
            }
            else
                line.positionCount = 0;
        }
        else
            line.positionCount = 0;
    }
    /*Eqation
       x^2 = 4p(y-height) :p<0 
        => draw between 
        pos0(sqrt(-4p*height):a, 0)~ pos1(b:b<0,c)
    */
    private void DrawParabola(Vector3 endPos)
    {
        //camera forward : x axis 
        //camera up : y axis
        Vector3 dir = endPos - startPos.position;

        float distanceX = Vector3.Dot(dir, cam.forward);
        float distanceY = Vector3.Dot(dir, cam.up);
        float distanceZ = Vector3.Dot(dir, cam.right);

        float deltaX = distanceX / resolution;
        float deltaZ = distanceZ / resolution;

        float p,a,b,c = distanceY;
        float h = c <= height ? height : c;

        Vector3[] vertices = new Vector3[resolution + 1];

        if (c == 0)
        {
            a = -distanceX * 0.5f;
            b = -a;

            p = a * a / (-4 * h);
            if (p == 0)
            {
                StraightLine(endPos);
                return;
            }
        }
        else
        {
            p = distanceX;
            p /= Mathf.Sqrt(4 * h) + Mathf.Sqrt(4 * (h - c));
            p*=-p;
            if (p == 0)
            {
                StraightLine(endPos);
                return;
            }
            
            a = deltaX >= 0 ? -Mathf.Sqrt(4 * p * -h) : Mathf.Sqrt(4 * p * -h);
            b = deltaX >= 0 ? Mathf.Sqrt(4 * p * (c - h)): -Mathf.Sqrt(4 * p * (c - h));
        }

        //setVertices
        vertices[0] = startPos.position;
        vertices[resolution] = endPos;
        for (int i = 1; i < resolution; i++)
        {
            float x = a + (deltaX * i);
            float y = x * x / (4 * p) + h;

            vertices[i] = startPos.position 
                + (x - a) * cam.forward 
                + y * cam.up 
                + (deltaZ * i) * cam.right;
        }

        //setLine
        line.positionCount = resolution + 1;
        line.SetPositions(vertices);
    }
    void StraightLine(Vector3 endPos) {
        line.positionCount = 2;
        line.SetPosition(0, cam.position);
        line.SetPosition(1, endPos);
    }
}
