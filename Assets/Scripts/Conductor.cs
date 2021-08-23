using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Units;
using Game.States;

using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;

public class Conductor : MonoBehaviour
{
    public GameObject unitPrefab;
    public Camera cam;
    private GameObject currentUnit;
    private int currentUnitIdx;
    private List<GameObject> players = new List<GameObject>();

    void Start()
    {
        currentUnit = SpawnUnit(unitPrefab, new Vector3(0, .5f, 0));
        currentUnitIdx = 0;
    }

    void Update()
    {
        Unit unit = currentUnit?.GetComponent<Unit>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int nextIdx = GetNextIdx();
            SwitchPlayer(nextIdx);
        }

        else if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnUnit(unitPrefab, new Vector3(0, .5f, 0));
        }
    }

    public GameObject SpawnUnit(GameObject unitPrefab, Vector3 position)
    {
        GameObject unitObj = Instantiate(unitPrefab, position, Quaternion.identity);
        Unit unit = unitObj.GetComponent<Unit>();
        unit.SetCamera(cam);

        players.Add(unitObj);

        SwitchPlayer(GetNextIdx());
        return unitObj;
    }

    public int GetNextIdx()
    {
        currentUnitIdx = currentUnitIdx + 1 >= players.Count ? 0 : currentUnitIdx + 1;
        return currentUnitIdx;
    }

    void SwitchPlayer(int index)
    {
        Unit prevUnit = currentUnit?.GetComponent<Unit>();
        if (prevUnit != null)
        {
            prevUnit.SwitchState(new IdleState(prevUnit));
        }

        currentUnit = players[index];
        Unit curUnit = currentUnit?.GetComponent<Unit>();
        if (curUnit.fieldOfView != null)
        {
            curUnit.SwitchState(new PlayerControlState(curUnit));
        }

        FollowPlayer followPlayer = cam.GetComponent<FollowPlayer>();
        followPlayer.SetPlayer(currentUnit);
    }
}