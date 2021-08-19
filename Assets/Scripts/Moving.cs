using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Util = Game.Utils.Util;


    public class Moving : MonoBehaviour
{
        public Rigidbody rb;
        public Camera cam;
        public float speed;

        public void Move(Vector3 direction)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }

        public Vector3 GetMoveInput()
        {
            Vector3 moveInput = new Vector3();
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            return moveInput;
        }

        public void FixedUpdate()
        {
            Vector3 moveInput = GetMoveInput();
            FaceMousePoint();
            Move(moveInput);

        }
        void FaceMousePoint()
        {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;
            if (playerPlane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Util.FacePoint(rb, transform, targetPoint);
            }
        }
    }
