using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BulletPath))]
public class BulletPathEditor : Editor
{
    private static bool DrawTangent = false; 
    private int selectedIndex;
    private BulletPath path;
    void OnEnable()
    {
        path = target as BulletPath;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

    }
    void OnDisable()
    {

        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DrawTangent = GUILayout.Toggle(DrawTangent, "Draw Tangent.");
        GUILayout.Label(string.Format("Total Time :{0}s", path.TotalTime));
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }

    private void OnSceneGUI(SceneView view)
    {
        DrawScenePoint();
        DrawSceneLine();
        if (DrawTangent)
            DrawSceneTangentLine();
    }
    private void DrawScenePoint()
    {
        BulletPathPoint[] points = path.points;
        int numpos = points.Length;
        Quaternion handleRotation = Quaternion.identity;
        for (int i = 0; i < numpos; i++)
        {
            int pointIndex = i * 3;
            Handles.color = Color.green;
            BulletPathPoint pathpoint = points[i];
            Vector3 position = GetWorldPosition(pathpoint.position);
            float pointHandleSize = HandleUtility.GetHandleSize(position) * 0.04f;
            float pointPickSize = pointHandleSize * 0.7f;
            Handles.Label(position, "Point " + i);
            if(Handles.Button(position,handleRotation,pointHandleSize,pointPickSize,Handles.DotHandleCap))
            {
                selectedIndex = pointIndex;
                if (Selection.activeObject != target)
                {
                    Selection.activeObject = target;
                }
            }
            if (selectedIndex == pointIndex)
            {
                EditorGUI.BeginChangeCheck();
                var worldpos = GetWorldPosition(pathpoint.position);
                var pos = Handles.PositionHandle(worldpos, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path, "modity position");
                    points[i].position = GetLocalPosition(pos);
                    return;
                }

            }
            Handles.color = Color.gray;
            int inIndex = pointIndex - 1;
            int outIndex = pointIndex + 1;
            if (selectedIndex < 0 || selectedIndex < inIndex || selectedIndex > outIndex)
            {
                continue;

            }
            if(i!= 0)
            {
                var tangent = GetWorldPosition(pathpoint.InTangentPos);
                Handles.DrawLine(position,tangent);
                if(Handles.Button(tangent,handleRotation,pointHandleSize,pointPickSize,Handles.DotHandleCap))
                {
                    selectedIndex =  inIndex;
                }
                EditorGUI.BeginChangeCheck();
                var pos = Handles.PositionHandle(tangent,handleRotation);
                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path, "modity tangent");
                    points[i].InTangentPos = GetLocalPosition(pos);
                    return;
                }
            }
            if(i != numpos -1 )
            {
                var tangent = GetWorldPosition(pathpoint.OutTangentPos);
                Handles.DrawLine(position,tangent);
                if(Handles.Button(tangent,handleRotation,pointHandleSize,pointPickSize,Handles.DotHandleCap))
                {
                    selectedIndex =  outIndex;
                }
                EditorGUI.BeginChangeCheck();
                var pos = Handles.PositionHandle(tangent,handleRotation);
                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(path, "modity tangent");
                    points[i].OutTangentPos = GetLocalPosition(pos);
                    return;
                }

            }
        }
    }
    private void DrawSceneLine()
    {
        Handles.color = Color.white;
        Vector3 last = GetWorldPosition(path.GetPointDetail(0.0f));
        for (int i = 0; i < 100; i++)
        {
            Vector3 cur = GetWorldPosition(path.GetPointDetail(i));
            Handles.DrawLine(last, cur);
            last = cur;
        }

    }
    private void DrawSceneTangentLine()
    {
        Handles.color = Color.blue;
        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = GetWorldPosition(path.GetPointDetail(i));
            Vector3 v = path.GetTangentForDetail(i);
            Handles.DrawLine(pos, pos + v.normalized);
        }
    }

    private Vector3 GetWorldPosition(Vector3 pos)
    {
        return pos;
    }
    private Vector3 GetLocalPosition(Vector3 pos)
    {
        return pos;
    }
}
