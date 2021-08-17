using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Shooting
{
    public interface IShootBehaviour
    {
        public float fireRate { get; set; }
        void Shoot(Vector3 origin, Vector3 direction);
    }

    public class DefaultShootBehaviour : IShootBehaviour
    {
        float fireDist;
        public DefaultShootBehaviour()
        {
            fireDist = 100f;
            fireRate = .333f;
        }

        public float fireRate { get; set; }
        private float lastShtTime;
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
