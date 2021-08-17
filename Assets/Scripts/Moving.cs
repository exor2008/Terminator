using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Moving
{
    public interface IWalkBehaviour
    {
        public float speed { get; set; }
        void Move(Vector3 direction, float dt);
    }

    public interface IMoveInput
    {
        public Vector3 GetMoveInput();
    }

    public class HumanoidWalkBehaviour : IWalkBehaviour
    {
        public Rigidbody rb;
        public float speed { get; set; }

        public HumanoidWalkBehaviour(Rigidbody rb, float speed)
        {
            this.rb = rb;
            this.speed = speed;
        }

        public void Move(Vector3 direction, float dt)
        {
            rb.MovePosition(rb.position + direction * speed * dt); //ime.fixedDeltaTime);
        }
    }

    public class MoveInput: IMoveInput
    {
        public Vector3 GetMoveInput()
        {
            Vector3 moveInput = new Vector3();
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            return moveInput;
        }
    }
}