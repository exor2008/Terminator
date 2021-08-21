using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Util = Game.Utils.Util;

namespace Game.FieldsOfView
{
    public class FieldOfView
    {
        public Transform transform;
        public float viewRaius;
        [Range(0, 360)]
        public float viewAngle;

        public LayerMask targetMak;
        public LayerMask obstacleMask;

        public MeshFilter viewMeshFilter;

        public int edgeResolveIterations;
        public float edgeDstThreshold;
        public float meshResolution;

        Mesh viewMesh;

        [HideInInspector]
        public List<Transform> visibleTargets = new List<Transform>();

        public FieldOfView(
            Transform _transform,
            float _viewRaius,
            float _viewAngle,
            LayerMask _targetMak,
            LayerMask _obstacleMask,
            MeshFilter _viewMeshFilter,
            int _edgeResolveIterations,
            float _edgeDstThreshold,
            float _meshResolution)
        {
            transform = _transform;
            viewRaius = _viewRaius;
            viewAngle = _viewAngle;
            targetMak = _targetMak;
            obstacleMask = _obstacleMask;
            viewMeshFilter = _viewMeshFilter;
            edgeResolveIterations = _edgeResolveIterations;
            edgeDstThreshold = _edgeDstThreshold;
            meshResolution = _meshResolution;

            viewMesh = new Mesh();
            viewMesh.name = "View Mesh";
            viewMeshFilter.mesh = viewMesh;
        }

        public IEnumerator FindTargetsWithDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindVisibleTargets();
            }
        }
        public void FindVisibleTargets()
        {
            visibleTargets.Clear();
            Collider[] targetsInViewRadius = Physics.OverlapSphere(
                transform.position, 
                viewRaius, 
                targetMak);

            foreach (Collider ctarget in targetsInViewRadius)
            {
                Transform target = ctarget.transform;
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float distToTarget = Vector3.Distance(transform.position, target.position);
                    if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }

        public List<Transform> GetVisibleTargets()
        {
            return visibleTargets;
        }

        public void DrawFieldOfView()
        {
            int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
            float stepAngleSize = viewAngle / stepCount;
            List<Vector3> viewPoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for (int i = 0; i <= stepCount; i++)
            {
                float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if (i > 0)
                {
                    bool edgeDstThresholdExceeded = Mathf.Abs(
                        oldViewCast.dist - newViewCast.dist) > edgeDstThreshold;

                    if (oldViewCast.hit != newViewCast.hit 
                        || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);

                        if (edge.pointA != Vector3.zero)
                            viewPoints.Add(edge.pointA);
                        if (edge.pointB != Vector3.zero)
                            viewPoints.Add(edge.pointB);
                    }
                }

                viewPoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            int vertexCount = viewPoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;

            for (int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

                if (i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            viewMesh.Clear();
            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.RecalculateNormals();
        }

        private EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeResolveIterations; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDstThresholdExceeded = Mathf.Abs(
                    minViewCast.dist - newViewCast.dist) > edgeDstThreshold;

                if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        private ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = Util.DirFromAngle(transform.eulerAngles.y, globalAngle, true);
            RaycastHit hit;

            if (Physics.Raycast(
                transform.position, 
                dir, 
                out hit, 
                viewRaius, 
                obstacleMask))
            {
                return new ViewCastInfo(
                    true, hit.point, hit.distance, globalAngle);
            }
            else
            {
                return new ViewCastInfo(
                    false, transform.position + dir * viewRaius, viewRaius, globalAngle);
            }
        }

        private struct ViewCastInfo
        {
            public bool hit;
            public Vector3 point;
            public float dist;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle)
            {
                hit = _hit;
                point = _point;
                dist = _dist;
                angle = _angle;
            }
        }

        private struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }
    }
}