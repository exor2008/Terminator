using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Game.Units;
using Util = Game.Utils.Util;

[CustomEditor (typeof (Unit))]
public class UnitEditor : Editor
{
    private void OnSceneGUI()
    {
        Unit unit = (Unit)target;
        Debug.Log(unit.transform.position);
        Handles.color = Color.white;
        Handles.DrawWireArc(unit.transform.position, Vector3.up, Vector3.forward, 360, unit.viewRaius);

        float unitAngle = unit.transform.eulerAngles.y;
        Vector3 viewAngleA = Util.DirFromAngle(unitAngle, -unit.viewAngle / 2, false);
        Vector3 viewAngleB = Util.DirFromAngle(unitAngle, unit.viewAngle / 2, false);

        Handles.DrawLine(unit.transform.position, unit.transform.position + viewAngleA * unit.viewRaius);
        Handles.DrawLine(unit.transform.position, unit.transform.position + viewAngleB * unit.viewRaius);

        Handles.color = Color.green;
        foreach(Transform visTarget in unit.fieldOfViewBehaviour.GetVisibleTargets())
        {
            Handles.DrawLine(unit.transform.position, visTarget.position);
        }
    }
}
