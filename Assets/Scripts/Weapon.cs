using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Units;
using Game.States;

namespace Game.Weapons
{
    public abstract class Weapon
    {
        public float fireDist;
        public float fireRate;
        public int damage;
        protected float lastShtTime;

        public Weapon(
            float _fireDist,
            float _fireRate)
        {
            fireDist = _fireDist;
            fireRate = _fireRate;
        }

        public virtual void Shoot(Vector3 origin, Vector3 direction)
        { }

        public void DealDamage(Unit unit)
        {
            unit.healt.TakeDamage(damage);
        }
        public void AwareAttacked(Unit unit, Vector3 hitPos)
        {
            if (unit.GetCurrentState() is PlayerControlState)
            {
                return;
            }
            unit.SwitchState(new AttackedState(unit, hitPos));
            Debug.Log("Attacked");
        }
    }

    public class DebugWeapon: Weapon
    {
        const float FIRE_DIST = 10;
        const float FIRE_RATE = .5f;

        public DebugWeapon(
            float _fireDist = FIRE_DIST,
            float _fireRate = FIRE_RATE)
            : base(
                _fireDist,
                _fireRate)
        { }

        public override void Shoot(Vector3 origin, Vector3 direction)
        {
            if (Time.time - lastShtTime < fireRate) return;

            lastShtTime = Time.time;
            RaycastHit hitInfo;

            Ray ray = new Ray(origin, direction);
            if (Physics.Raycast(ray, out hitInfo, fireDist))
            {
                Debug.DrawLine(origin, hitInfo.point, Color.red, .3f);
                //Debug.Log(string.Format("Hit {0}", hitInfo.collider.name));
                Unit attacked = hitInfo.transform.GetComponentInParent<Unit>();
                if (attacked != null)
                {
                    AwareAttacked(attacked, hitInfo.point);
                }
            }
            else
            {
                Debug.DrawLine(origin, ray.GetPoint(fireDist), Color.grey, .3f);
            }
        }
    }
}