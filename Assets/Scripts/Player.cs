using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Moving;
using Game.Shooting;

namespace Game.Units
{
    public class Player : Unit
    {
        public GameObject player;
        public Camera cam;

        Vector3 targetPoint;

        private void Start()
        {
            SetWalkBehaviour(new HumanoidWalkBehaviour(rb, speed));
            SetShootBehaviour(new DefaultShootBehaviour());
        }

        void FixedUpdate()
        {
            // Movement
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            FaceMousePoint();
            walkBehaviour.Move(moveInput, Time.fixedDeltaTime);

            // Shooting
            if (Input.GetButton("Fire1"))
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                shootBehaviour.Shoot(transform.position, forward);
            }
        }

        void FaceMousePoint()
        {
            // Facing the mouse
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;
            if (playerPlane.Raycast(ray, out hitDist))
            {
                targetPoint = ray.GetPoint(hitDist);
                FacePoint(targetPoint);
            }
        }
    }
}