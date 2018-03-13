using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BulletPathComponent))]
public class BulletPathComponentInspector : Editor
{
    private GameObject activeGameObject;
    private BulletPathComponent PathComponent;

    void OnEnable()
    {
        PathComponent = target as BulletPathComponent;
        activeGameObject = PathComponent.gameObject;
        PathComponent.Start();
    }
    void OnDisable()
    {

      
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Priview"))
        {
            PathComponent.Priview();
        }
        if (PathComponent.Path != null)
        {
            if (GUILayout.Button("Edit BulletPath"))
            {
                Selection.activeObject = PathComponent.Path;
            }
        }

    }

    public void OnSceneGUI()
    {
        DrawScenePoint();
        DrawSceneLine();
        DrawSceneTangentLine();
    }
    private void DrawScenePoint()
    {

    }
    private void DrawSceneLine()
    {
        Handles.color = Color.white;
        Vector3 last = PathComponent.GetWorldPosition(PathComponent.GetPointDetail(0.0f));
        for (int i = 0; i < 100; i++)
        {
            Vector3 cur = PathComponent.GetWorldPosition(PathComponent.GetPointDetail(i));
            Handles.DrawLine(last, cur);
            last = cur;
        }

    }
    private void DrawSceneTangentLine()
    {
        Handles.color = Color.blue;
        for (int i = 0; i < 100; i++)
        {
            Vector3 pos = PathComponent.GetWorldPosition(PathComponent.GetPointDetail(i));
            Vector3 v = PathComponent.GetTangentForDetail(i);
            Handles.DrawLine(pos, pos + v.normalized);
        }
    }


}
