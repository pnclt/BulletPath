using UnityEngine;
using System.Collections;
[CreateAssetMenu()]
public class BulletPath : ScriptableObject
{
    [SerializeField]
    public BulletPathPoint[]  points;

    public float TotalTime{
        get
        {
            float total = 0;
            for (int i = 0; i < points.Length; i++)
                total += points[i].time;
            return total;
        }
    }

    public Vector3 TargetDir
    {
        get
        {
            return points != null ? (points[points.Length - 1].position - points[0].position) : Vector3.zero;
        }
    }

    public Vector3 GetPointDetail(float t, float allsteps = 100)
    {
        var single_steps = allsteps / (points.Length - 1);
        var last = (int)(t / single_steps);
        var cur = last + 1;
        var per = t / single_steps - last;
        return cur >= points.Length ? points[last].position:GetBezierPoint(points[last],points[cur],per);
    }
    public Vector3 GetTangentForDetail(float t, float allsteps = 100)
    {
        var single_steps = allsteps / (points.Length - 1);
        var last = (int)(t / single_steps);
        var cur = last + 1;
        var per = t / single_steps - last;
        return cur >= points.Length ? points[last].inTangent.normalized : GetBezierTangent(points[last], points[cur], per);

    }
    public Vector3 GetPointForTime(float cur)
    {
        for (int i = 1; i < points.Length; i++)
        {
            if (cur <= points[i].time)
            {
                float per = cur / points[i].time;
                return GetBezierPoint(points[i - 1], points[i], per);
            }
            else
            {
                cur -= points[i].time;

            }
        }
        return Vector3.zero;
    }
    public Vector3 GetTangentForTime(float cur)
    {
        cur = cur < 0.01f ? 0.01f : cur;
        for (int i = 1; i < points.Length; i++)
        {
            if (cur <= points[i].time)
            {
                float per = cur / points[i].time;
                return GetBezierTangent(points[i - 1], points[i], per);
            }
            else
            {
                cur -= points[i].time;

            }
        }
        return Vector3.zero;
    }
    public static Vector3 GetBezierPoint(BulletPathPoint point, BulletPathPoint nextpoint, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p =  uuu * point.position;
        p += 3 * uu * t * point.OutTangentPos;
        p += 3 * u * tt * nextpoint.InTangentPos;
        p += ttt * nextpoint.position;
        return p;
    }
    public static Vector3 GetBezierTangent(BulletPathPoint point,BulletPathPoint nextpoint,float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float tu = t * u;
        float uu = u * u;
        Vector3 p = -3 * uu * point.position;
        p += 3 * (uu - 2 * tu) * point.OutTangentPos;
        p += 3 * (2 * tu - tt) * nextpoint.InTangentPos;
        p += 3 * tt * nextpoint.position;
        return p.normalized;
    }
    public BulletPath()
    {
        Reset();

    }
    public void Reset()
    {
        if (points == null)
        {
            points = new BulletPathPoint[]{
                new BulletPathPoint(){time = 0.1f,position = new Vector3(0,0,0),Tangent = new Vector3(0,0,0)},
                new BulletPathPoint(){time = 0.1f,position = new Vector3(10,0,0),Tangent = new Vector3(0,0,0)},
                new BulletPathPoint(){time = 0.1f,position = new Vector3(20,0,0),Tangent = new Vector3(0,0,0)},
                new BulletPathPoint(){time = 0.1f,position = new Vector3(30,0,0),Tangent = new Vector3(0,0,0)},
            };
        }else{
            for(int i =0;i< points.Length;i++)
            {
                points[i].time = 0.1f;
                points[i].position = new Vector3(i * 10, 0, 0);
                points[i].Tangent = new Vector3(0, 0, 0);

            }
        }
        
    }
}
