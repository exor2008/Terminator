using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Util = Game.Utils.Util;

namespace Game.Move
{
    public class Mover
    {
        public Rigidbody rb;
        public Transform transform;
        public Camera cam;
        public float speed;

        public Mover(
            Rigidbody _rb,
            Transform _transform,
            Camera _cam,
            float _speed)
        {
            rb = _rb;
            transform = _transform;
            cam = _cam;
            speed = _speed;
        }

        public void Move(Vector3 direction)
        {
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }

        public void FaceMousePoint()
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
}