//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//using Game.Units;
//using Game.FieldsOfView;
//using Util = Game.Utils.Util;

//[CustomEditor (typeof (Unit))]
//public class UnitEditor : Editor
//{
//    private void OnSceneGUI()
//    {
//        FieldOfView fow = ((Unit)target).fieldOfView;
//        Debug.Log(fow.transform.position);
//        Handles.color = Color.white;
//        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRaius);

//        float unitAngle = fow.transform.eulerAngles.y;
//        Vector3 viewAngleA = Util.DirFromAngle(unitAngle, -fow.viewAngle / 2, false);
//        Vector3 viewAngleB = Util.DirFromAngle(unitAngle, fow.viewAngle / 2, false);

//        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRaius);
//        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRaius);

//        Handles.color = Color.green;

//        foreach (Transform visTarget in fow.GetVisibleTargets())
//        {
//            Handles.DrawLine(fow.transform.position, visTarget.position);
//        }
//    }
//}
