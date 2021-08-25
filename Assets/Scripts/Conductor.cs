using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Units;
using Game.States;

using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;

public class Conductor : MonoBehaviour
{
    public GameObject unitHumanPrefab;
    public GameObject unitTerminatorPrefab;
    public Camera cam;
    private GameObject currentUnit;
    private int currentUnitIdx;
    private List<GameObject> players = new List<GameObject>();
    private List<GameObject> terminators = new List<GameObject>();

    void Start()
    {
        currentUnit = SpawnHumanUnit(unitHumanPrefab, new Vector3(0, .5f, 0));
        currentUnitIdx = 0;
        StartCoroutine(SwitchPlayerWithDelay(currentUnitIdx));

        for (int i = 0; i < 1; i++)
        {
            Vector3 randPos = new Vector3(Random.Range(-10, 10), 0.5f, Random.Range(-10, 10));
            GameObject terminator = SpawnTerminatorUnit(unitTerminatorPrefab, randPos);
            StartCoroutine(InitTerminatorWithDelay(terminator));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int nextIdx = GetNextIdx();
            StartCoroutine(SwitchPlayerWithDelay(nextIdx));
        }

        else if (Input.GetKeyDown(KeyCode.N))
        {
            SpawnHumanUnit(unitHumanPrefab, new Vector3(0, .5f, 0));
            StartCoroutine(SwitchPlayerWithDelay(GetNextIdx()));
        }
    }

    public GameObject SpawnHumanUnit(GameObject unitPrefab, Vector3 position)
    {
        GameObject unitObj = Instantiate(unitPrefab, position, Quaternion.identity);
        players.Add(unitObj);        
        return unitObj;
    }

    public GameObject SpawnTerminatorUnit(GameObject unitPrefab, Vector3 position)
    {
        GameObject unitObj = Instantiate(unitPrefab, position, Quaternion.identity);
        terminators.Add(unitObj);
        return unitObj;
    }

    public void InitTerminator(GameObject terminator)
    {
        Unit unit = terminator.GetComponent<Unit>();
        unit.SwitchState(new RoamingState(unit));
    }

    public IEnumerator InitTerminatorWithDelay(GameObject terminator)
    {
        yield return new WaitForEndOfFrame();
        InitTerminator(terminator);
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
            prevUnit.SwitchState(new RoamingState(prevUnit));
            prevUnit.navAgent.enabled = true;
        }

        currentUnit = players[index];
        Unit curUnit = currentUnit?.GetComponent<Unit>();
        curUnit.SetCamera(cam);

        FollowPlayer followPlayer = cam.GetComponent<FollowPlayer>();
        followPlayer.SetPlayer(currentUnit);

        curUnit.navAgent.enabled = false;
        curUnit.SwitchState(new PlayerControlState(curUnit));
        
    }

    public IEnumerator SwitchPlayerWithDelay(int index)
    {
        yield return new WaitForEndOfFrame();
        SwitchPlayer(index);
    }
}