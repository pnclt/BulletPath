using UnityEngine;
using System.Collections;

public class BulletPathComponent : MonoBehaviour
{
    [SerializeField]
    public BulletPath  Path;
    public Transform Target;
    public float TotalTime;

    public Quaternion PathRotation {get{return Quaternion.Euler(PathAngle);}}
    public Vector3 PathAngle;
    public float PathLengthScale = 1;

    public float CurTime ;

    public bool Pause = false;

    public bool IsUpdateObjectOrientation = false;

    public Vector3 StartPosition { get; private set; }
    private Transform activeTransform;
    public void Start()
    {
        activeTransform = transform;
        StartPosition = transform.position;
        TotalTime = Path ? Path.TotalTime : 0;

    }
    public void Update()
    {
        if (IsUpdateObjectOrientation)
        {
            var angle = GetTangentForTime(CurTime);
            var rotation = Quaternion.LookRotation(angle, Vector3.up) * Quaternion.Euler(new Vector3(90, 0, 0));
            activeTransform.rotation = rotation;
        }
        activeTransform.position = GetWorldPosition(GetPointForTime(CurTime));
        if (!Pause)
        {
            CurTime += Time.deltaTime;
            if (CurTime > TotalTime)
            {
                CurTime = 0;
                Pause = true;
            }
        }
    }
    public void Priview()
    {
        CurTime = 0;
        Pause = false;
        RefreshTarget();
    }
    public void RefreshTarget()
    {
        if(Target)
        {
            var dir = Target.position - StartPosition;
            PathAngle = (Quaternion.FromToRotation(Path.TargetDir,dir)).eulerAngles;
            PathLengthScale = dir.magnitude / Path.TargetDir.magnitude;
        }
    }
    public Vector3 GetPointDetail(float t, float allsteps = 100)
    {
        return Path ? Path.GetPointDetail(t,allsteps):Vector3.zero;
    }
    public Vector3 GetTangentForDetail(float t, float allsteps = 100)
    {
        return Path ? Path.GetTangentForDetail(t, allsteps) : Vector3.zero;
    }
    public Vector3 GetPointForTime(float cur)
    {
        return Path ? Path.GetPointForTime(cur) : Vector3.zero;
    }
    public Vector3 GetTangentForTime(float cur)
    {
        return Path ? Path.GetTangentForTime(cur) : Vector3.zero;
    }
    public Vector3 GetWorldPosition(Vector3 pos)
    {
        pos = PathRotation * pos * PathLengthScale;
        return pos + StartPosition;
    }
    public void Reset()
    {

    }
}
