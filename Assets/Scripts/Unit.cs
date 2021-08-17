using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Moving;
using Game.Shooting;
using Game.FieldOfView;

namespace Game.Units
{
    public abstract class Unit : MonoBehaviour
    {
        public Rigidbody rb;
        public GameObject firePoint;

        // Field of view
        public float viewRaius;
        [Range(0, 360)]
        public float viewAngle;

        public LayerMask targetMak;
        public LayerMask obstacleMask;

        public MeshFilter viewMeshFilter;

        public int edgeResolveIterations;
        public float edgeDstThreshold;
        public float meshResolution;

        // Behaviours
        public IWalkBehaviour walkBehaviour;
        public IShootBehaviour shootBehaviour;
        public IMoveInput moveInputBehaviour;
        public IFieldOfViewBehaviour fieldOfViewBehaviour;

        public void SetWalkBehaviour(IWalkBehaviour behaviour) => walkBehaviour = behaviour;
        public void SetMoveInputBehaviour(IMoveInput behaviour) => moveInputBehaviour = behaviour;
        public void SetShootBehaviour(IShootBehaviour behaviour) => shootBehaviour = behaviour;
        public void SetFieldOfViewBehaviour(IFieldOfViewBehaviour behaviour) => fieldOfViewBehaviour = behaviour;

        public void FacePoint(Vector3 target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);
            rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.fixedDeltaTime);
        }

        protected virtual void Start()
        {
            targetMak = LayerMask.NameToLayer("Mask");
            obstacleMask = LayerMask.NameToLayer("Obstacles");
        }
    }
}
