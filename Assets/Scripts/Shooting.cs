using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shooting
{
    public class Shooting : MonoBehaviour
    {
        public float fireDist;
        public float fireRate;
        private float lastShtTime;

        public void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Shoot(transform.position, forward);
            }
        }

        public void Shoot(Vector3 origin, Vector3 direction)
        {
            if (Time.time - lastShtTime < fireRate) return;

            lastShtTime = Time.time;
            RaycastHit hitInfo;

            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(origin, hitInfo.point, Color.red, .3f);
                //Debug.Log(string.Format("Hit {0}", hitInfo.collider.name));
            }
            else
            {
                Debug.DrawLine(origin, ray.GetPoint(fireDist), Color.grey, .3f);
            }
        }
    }
}
