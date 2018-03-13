using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BulletPathPoint
{
    public float time;
    public Vector3 position;
    public Vector3 Tangent;

    public Vector3 inTangent{ get{return Tangent;} }
    public Vector3 outTangent{ get{ return -Tangent;} }
    

    public Vector3 TangentPos { get{ return Tangent + position;} set{Tangent =  value -  position;} }
    public Vector3 InTangentPos { get{ return Tangent + position;} set{Tangent =  value -  position;} }
    public Vector3 OutTangentPos { get{ return -Tangent + position;} set{Tangent =  -value +  position;} }

}