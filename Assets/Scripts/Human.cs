using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Moving;
using Game.Shooting;
using Game.FieldOfView;

namespace Game.Units
{
    public class Human : Unit
    {
        public float speed = 5.0f;
        public GameObject player;
        public Camera cam;

        Vector3 targetPoint;

        new private void Start()
        {
            base.Start();
            SetMoveInputBehaviour(new MoveInput());
            SetWalkBehaviour(new HumanoidWalkBehaviour(rb, speed));
            SetShootBehaviour(new DefaultShootBehaviour());
            
            FieldOfViewParameters fowParameters = new FieldOfViewParameters(
                viewRaius,
                viewAngle,
                targetMak,
                obstacleMask,
                viewMeshFilter,
                edgeResolveIterations,
                edgeDstThreshold,
                meshResolution
            );
            SetFieldOfViewBehaviour(new MeshFieldOfViewBehaviour(transform, fowParameters));
        }

        void FixedUpdate()
        {
            // Movement
            Vector3 moveInput = moveInputBehaviour.GetMoveInput();
            FaceMousePoint();
            walkBehaviour.Move(moveInput, Time.fixedDeltaTime);

            // Shooting
            if (Input.GetButton("Fire1"))
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                shootBehaviour.Shoot(transform.position, forward);
            }
        }

        private void LateUpdate()
        {
            fieldOfViewBehaviour.DrawFieldOfView();
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