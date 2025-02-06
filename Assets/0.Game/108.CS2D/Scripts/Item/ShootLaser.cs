using System.Collections;
using System.Collections.Generic;
using Minigame.CS2D;
using UnityEngine;

namespace Minigame.FireBoyAndWaterGirl2
{
    public class ShootLaser : ActivationObject
    {
        public Material material;
        [SerializeField] private LineRenderer laser;
        [SerializeField] private LayerMask layerMask;
        public List<Vector3> laserIndices;

        // public TypeColor typeColor;
        public ActivationObject currentActive;

        protected override void Start()
        {
            base.Start();
            laser.startColor = Common.Get(typeColor);
            laser.endColor = Common.Get(typeColor);
        }

        private void Update()
        {
            laserIndices.Clear();
            laserIndices = new List<Vector3>();
            CastRay(transform.position, transform.right, 10);
        }

        public void CastRay(Vector2 pos, Vector2 dir, int reflectionCount)
        {
            laserIndices.Add(pos);

            if (reflectionCount <= 0)
            {
                laserIndices.Add(pos + dir.normalized * 30f);
                UpdateLaser();
                return;
            }

            Ray2D ray = new Ray2D(pos, dir);
            RaycastHit2D hit = Physics2D.Raycast(pos, dir, 30, layerMask);

            if (hit)
            {
                CheckHit(hit, dir, reflectionCount);
            }
            else
            {
                laserIndices.Add(ray.GetPoint(30));
                UpdateLaser();
            }
        }

        public void CheckHit(RaycastHit2D hit, Vector2 direction, int reflectionCount)
        {
            if (hit.collider.CompareTag("Mirror"))
            {
                Vector2 pos = hit.point - direction * 0.01f;
                Vector2 dir = Vector2.Reflect(direction, hit.normal);

                CastRay(pos, dir, reflectionCount - 1);
            }
            else
            {
                laserIndices.Add(hit.point);
                UpdateLaser();
                if (hit.collider.CompareTag("Bubble") && (hit.collider.GetComponent<ActivationObject>().typeColor == typeColor))
                {
                    currentActive = hit.collider.GetComponent<ActivationObject>();
                    currentActive.Active();
                }
                else
                {
                    currentActive?.DeActive();
                    currentActive = null;
                }
            }
        }

        public void UpdateLaser()
        {
            int count = 0;
            laser.positionCount = laserIndices.Count;
            foreach (var idx in laserIndices)
            {
                laser.SetPosition(count, idx);
                count++;
            }
        }
    }
}
