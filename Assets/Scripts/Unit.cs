using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Moving;
using Game.Shooting;

namespace Game.Units
{
    public abstract class Unit : MonoBehaviour
    {
        public Rigidbody rb;
        public GameObject firePoint;
        
        public float speed = 5.0f;
        protected Vector3 moveInput;
        public IWalkBehaviour walkBehaviour;
        public IShootBehaviour shootBehaviour;

        public void SetWalkBehaviour(IWalkBehaviour behaviour) => walkBehaviour = behaviour;
        public void SetShootBehaviour(IShootBehaviour behaviour) => shootBehaviour = behaviour;

        public void FacePoint(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.fixedDeltaTime);
        }
    }
}
