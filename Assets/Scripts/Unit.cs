using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;
using Game.States;

namespace Game.Units
{
    public class Unit : MonoBehaviour
    {
        public float viewRadius;
        [Range(0, 360)]
        public float viewAngle;
        public Camera cam;
        public LayerMask targetMask;
        public LayerMask obstacleMask;
        public MeshFilter viewMeshFilter;
        Rigidbody rb;
        
        public int edgeResolveIterations;
        public float edgeDistThreshold;
        public float meshResolution;
        public float speed;

        public Mover mover;
        public Weapon weapon;
        public FieldOfView fieldOfView;
        public StateManager stateManager;


        public void Start()
        {
            weapon = new DebugWeapon();
            fieldOfView = new FieldOfView(
                transform,
                viewRadius,
                viewAngle,
                targetMask,
                obstacleMask,
                viewMeshFilter,
                edgeResolveIterations,
                edgeDistThreshold,
                meshResolution);
            rb = GetComponent<Rigidbody>();
            mover = new Mover(rb, transform, cam, speed);
            stateManager = new StateManager(new IdleState(this));
        }

        public void Update()
        {
            stateManager.Updtae();
        }

        public void FixedUpdate()
        {
            stateManager.FixedUpdate();
        }

        public void LateUpdate()
        {
            stateManager.LateUpdate();
        }

        public void ChangeWeapon(Weapon _weapon)
        {
            weapon = _weapon;
        }

        public void SwitchState(State state)
        {
            stateManager.SwitchState(state);
        }

        public State GetCurrentState()
        {
            return stateManager.GetCurrentState();
        }

        public void SetCamera(Camera _cam)
        {
            cam = _cam;
        }
    }

    //public struct UnitParameters
    //{
    //    float viewRadius;
    //    float viewAngle;

    //    public LayerMask targetMask;
    //    public LayerMask obstacleMask;

    //    public MeshFilter viewMeshFilter;
    //    Rigidbody rb;

    //    public int edgeResolveIterations;
    //    public float edgeDistThreshold;
    //    public float meshResolution;
    //    public float speed;
    //}
}