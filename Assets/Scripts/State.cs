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
        public IdleState(Unit unit) : base(unit) 
        {
            unit.StopNavAgent();
        }
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
            unit.StartNavAgent();
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
        public PlayerControlState(Unit unit) : base(unit) 
        {
            unit.StopNavAgent();
        }
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
        public AimState(Unit unit) : base(unit) 
        {
            unit.StopNavAgent();
        }
        public override State Update()
        {
            visibleTargets = unit.GetVisibleTargets();
            if (visibleTargets.Count == 0)
            {
                Debug.Log("Aaiming -> Roaming");
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
            RaycastHit hit;
            Physics.Raycast(
                unit.transform.position, 
                unit.transform.TransformDirection(Vector3.forward), out hit);

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
            unit.StopNavAgent();
        }
        public override State Update()
        {
            bool canSee = (target != null) && SeeTarget(unit.transform.eulerAngles.y, target);

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

    public class AttackedState: State
    {
        Vector3 hitPos;
        Quaternion rotationLeft;
        const float angleToTarget = 5f;
        public AttackedState(Unit _unit, Vector3 _hitPos): base(_unit)
        {
            hitPos = _hitPos;
            _unit.StopNavAgent();
            rotationLeft = Quaternion.Euler(0, 90, 0);
        }
        public override State Update()
        {
            Vector3 diff = unit.rb.rotation.eulerAngles - rotationLeft.eulerAngles;
            if (Mathf.Abs(diff.y) < angleToTarget)
            {
                // no longer can see target, start aiming
                return new AimState(unit);
            }
            else
            {
                return this;
            }
        }
        public override void FixedUpdate() 
        {
            rotationLeft = Util.FacePoint(unit.rb, unit.transform, hitPos);
        }
        public override void LateUpdate() { }
    }
}