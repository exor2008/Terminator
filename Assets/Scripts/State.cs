using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Units;

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
        public override void LateUpdate() 
        { 

        }

        public Vector3 GetMoveInput()
        {
            Vector3 moveInput = new Vector3();
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            return moveInput;
        }
    }
}