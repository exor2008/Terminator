using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Units;
using Util = Game.Utils.Util;

namespace Game.States
{
    public abstract class State
    {
        protected Unit unit;
        public State(Unit _unit)
        {
            unit = _unit;
        }
        public abstract State Update();
        public abstract void FixedUpdate();
        public abstract void LateUpdate();

    }

    public class IdleState: State
    {
        public IdleState(Unit unit) : base(unit) { }
        public override State Update()
        {
            return this;
        }
        public override void FixedUpdate() { }
        public override void LateUpdate() { }
    }

    public class RoamingState : State
    {
        private Vector3 desiredDestination;
        public RoamingState(Unit unit) : base(unit) 
        {
            desiredDestination = GetNewDestination();
        }
        public override State Update()
        {
            float distLeft = Vector3.Distance(unit.transform.position, desiredDestination);
            if (distLeft < 0.6f)
            {
                desiredDestination = GetNewDestination();
            }
            unit.navAgent.SetDestination(desiredDestination);

            if (unit.GetVisibleTargets().Count == 0)
            {
                return this;
            }
            else 
            {
                Debug.Log("Roaming -> Aaiming");
                unit.navAgent.isStopped = true;
                return new AimState(unit);
            }
            
        }
        public Vector3 GetNewDestination()
        {
            return new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
        }
        public override void FixedUpdate() { }
        public override void LateUpdate() { }
    }
    

    public class PlayerControlState : State
    {
        public PlayerControlState(Unit unit) : base(unit) { }
        public override State Update()
        {
            if (Input.GetButton("Fire1"))
            {
                Vector3 forward = unit.transform.TransformDirection(Vector3.forward);
                unit.weapon.Shoot(unit.transform.position, forward);
            }

            unit.fieldOfView.DrawFieldOfView();
            return this;
        }
        public override void FixedUpdate() 
        {
            Vector3 moveInput = GetMoveInput();
            unit.mover.FaceMousePoint();
            unit.mover.Move(moveInput);
        }
        public override void LateUpdate() { }

        public Vector3 GetMoveInput()
        {
            Vector3 moveInput = new Vector3();
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            return moveInput;
        }
    }

    public class AimState : State
    {
        protected Transform target;
        protected List<Transform> visibleTargets;
        public AimState(Unit unit) : base(unit) { }
        public override State Update()
        {
            visibleTargets = unit.GetVisibleTargets();
            if (visibleTargets.Count == 0)
            {
                Debug.Log("Aaiming -> Roaming");
                unit.navAgent.isStopped = false;
                return new RoamingState(unit);
            }
            else
            {
                target = Choosetarget(visibleTargets);
                bool canSee = SeeTarget(unit.transform.eulerAngles.y, target);

                if (canSee)
                {
                    Debug.Log("Aaiming -> Attacking");
                    return new AttackState(unit, target);
                }
                else
                {
                    return this;
                }
            }
        }
        public override void FixedUpdate()
        {
            if (target != null)
            {
                Util.FacePoint(unit.rb, unit.transform, target.position);
            }
        }
        public override void LateUpdate() { }

        public bool SeeTarget(float angle, Transform target)
        {
            //ViewCastInfo hit = unit.fieldOfView.ViewCast(unit.transform.eulerAngles.y);
            RaycastHit hit;
            Physics.Raycast(
                unit.transform.position, 
                unit.transform.TransformDirection(Vector3.forward), out hit);

            //Debug.Log(hit.transform);
            return hit.transform == target;
        }

        public Transform Choosetarget(List<Transform> targets)
        {
            return visibleTargets[0];
        }
    }

    public class AttackState : AimState
    {
        public AttackState(Unit unit, Transform _target) : base(unit) 
        {
            target = _target;
        }
        public override State Update()
        {
            bool canSee = SeeTarget(unit.transform.eulerAngles.y, target);

            if (canSee)
            {
                Vector3 forward = unit.transform.TransformDirection(Vector3.forward);
                unit.weapon.Shoot(unit.transform.position, forward);
                return this;
            }
            else
            {
                Debug.Log("Attacking -> Aiming");
                return new AimState(unit);
            }
        }
        public override void FixedUpdate() { }
        public override void LateUpdate() { }
    }
}