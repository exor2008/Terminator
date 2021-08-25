using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Game.Weapons;
using Game.FieldsOfView;
using Game.Move;
using Game.States;
using Game.Healths;

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
        public Rigidbody rb;
        public NavMeshAgent navAgent;
        
        public int edgeResolveIterations;
        public float edgeDistThreshold;
        public float meshResolution;
        public float speed;

        public Mover mover;
        public Weapon weapon;
        public FieldOfView fieldOfView;
        public StateManager stateManager;
        public Health healt;
        public Side side;


        public virtual void Start()
        {
            weapon = new DebugWeapon(targetMask);
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
            StartCoroutine(fieldOfView.FindTargetsWithDelay(.3f));
            mover = new Mover(rb, transform, cam, speed);
            healt = new Health(this);
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
            mover.SetCamera(cam);
        }

        public List<Transform> GetVisibleTargets()
        {
            return fieldOfView.GetVisibleTargets();
        }

        public void StopNavAgent()
        {
            if (navAgent.enabled)
            {
                navAgent.enabled = false;
            }
        }

        public void StartNavAgent()
        {
            if (!navAgent.enabled)
            {
                navAgent.enabled = true;
            }
        }
    }
}

public enum Side
{
    Humans,
    Terminators
}