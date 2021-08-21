using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Units;
using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;

public class PlayerControled : MonoBehaviour
{
    [HideInInspector]
    public Unit unit;

    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public Camera cam;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public MeshFilter viewMeshFilter;
    public int edgeResolveIterations;
    public float edgeDistThreshold;
    public float meshResolution;
    public float speed;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Weapon weapon = new DebugWeapon();
        FieldOfView fieldOfView = new FieldOfView(
            transform,
            viewRadius,
            viewAngle,
            targetMask,
            obstacleMask,
            viewMeshFilter,
            edgeResolveIterations,
            edgeDistThreshold,
            meshResolution);
        Mover mover = new Mover(rb, transform, cam, speed);

        unit = new Unit(transform, rb, weapon, fieldOfView, mover);
        unit.fieldOfView.FindTargetsWithDelay(.3f);
    }

    public void FixedUpdate()
    {
        Vector3 moveInput = GetMoveInput();
        unit.mover.FaceMousePoint();
        unit.mover.Move(moveInput);
    }

    public void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            unit.weapon.Shoot(transform.position, forward);
        }

        unit.fieldOfView.DrawFieldOfView();
    }

    public Vector3 GetMoveInput()
    {
        Vector3 moveInput = new Vector3();
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");
        return moveInput;
    }
}
