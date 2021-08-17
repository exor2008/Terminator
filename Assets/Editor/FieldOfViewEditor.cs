using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fow = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRaius);

        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRaius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRaius);

        Handles.color = Color.green;
        foreach(Transform visTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visTarget.position);
        }
    }
}
