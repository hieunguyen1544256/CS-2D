using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;

namespace Minigame.CS2D
{
    public enum StateAI
    {
        FindGun,
        FindPlayer,
        FindRandom,
    }

    public class AIController : Controller
    {
        public Transform currentTarget;
        public Seeker seeker;
        public Path path;
        public float nextWayPointDistance = 3;
        public LayerMask layerMask;

        private int currentWayPoint = 0;
        private bool reachedEndOfPath = false;
        private List<Transform> targets = new();
        private float currentRotateZ = 0;

        public float fovAngle = 60;
        public float fovRange = 3f;

        private float timeDelayFindPath;
        private float timeDelayToShoot;
        private StateAI stateAI = StateAI.FindGun;
        [SerializeField] private float maxDistanceToFindPlayer = 10f;

        protected override void Start()
        {
            base.Start();
            foreach (var item in character.team == 0 ? GameManager.Instance.Team2 : GameManager.Instance.Team1)
            {
                targets.Add(item.transform);
            }
            seeker = GetComponent<Seeker>();

            // if (GameData.gameDifficultLevel == GameDifficultLevel.EASY)
            // {
            //     timeDelayFindPath = 3f;
            //     timeDelayToShoot = 1.5f;
            // }
            // else if (GameData.gameDifficultLevel == GameDifficultLevel.MEDIUM)
            // {
            //     timeDelayFindPath = 1.5f;
            //     timeDelayToShoot = 1f;
            // }
            // else
            // {
            //     timeDelayFindPath = 0.5f;
            //     timeDelayToShoot = 0.01f;
            // }

            timeDelayFindPath = 0.5f;
            timeDelayToShoot = 0.01f;

            InvokeRepeating(nameof(UpdatePath), 0, timeDelayFindPath);
        }

        private void UpdatePath()
        {
            CaculateTarget();
            if (currentTarget)
            {
                seeker.StartPath(transform.position, currentTarget.position, OnPathComplete);
            }
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                path = p;
                currentWayPoint = 0;
            }
        }

        protected override void Move()
        {
            base.Move();

            if (path == null) return;

            if (currentWayPoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            Vector2 direction = (Vector2)(path.vectorPath[currentWayPoint] - transform.position).normalized;
            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWayPoint]);

            if (distance < nextWayPointDistance)
            {
                currentWayPoint++;
            }

            character.moverment.move = direction.normalized;
            if (currentTarget)
            {
                character.moverment.rotate = (currentTarget.position - transform.position).normalized;
            }

            if (!character.isDead)
            {
                character.JoystickDown();
                StartCoroutine(HandleShoot());
            }
        }

        private IEnumerator HandleShoot()
        {
            bool isShoot = false;
            if (currentTarget.CompareTag("Player"))
            {
                var characterTarget = currentTarget.GetComponent<Character>();

                if (characterTarget && !characterTarget.isDead && CheckRaycastTarget())
                {
                    isShoot = true;
                    character.JoystickUp();
                }
                else
                {
                    if (character.isShoot)
                    {
                        character.ButtonUpShoot();
                    }
                }
            }
            else
            {
                if (character.isShoot)
                {
                    character.ButtonUpShoot();
                }
            }

            yield return new WaitForSeconds(timeDelayToShoot);

            // var positon = currentTarget.position;

            if (isShoot && currentTarget)
            {
                character.moverment.rotate = (currentTarget.position - transform.position).normalized;
                character.ButtonDownShoot();
            }
        }

        private bool CheckRaycastTarget()
        {
            if (!currentTarget) return false;

            RaycastHit2D hit = Physics2D.Raycast(character.pointGun.position, currentTarget.position - transform.position, 20, layerMask);
            if (hit && hit.collider.CompareTag("Player"))
            {
                var character1 = hit.collider.GetComponent<Character>();
                if (character1 && character.team != character1.team)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsTargetInsideOfView(Transform targetFollow)
        {
            if (!targetFollow) return false;

            Vector2 directionTarget = (targetFollow.position - transform.position).normalized;

            Vector2 lookDirection = character.moverment.GetDirectVector();

            float angleToTarget = Vector2.Angle(lookDirection, directionTarget);


            if (angleToTarget < fovAngle / 2)
            {
                float distance = Vector2.Distance(targetFollow.position, transform.position);

                return distance < fovRange;
            }

            return false;
        }

        private void CaculateTarget()
        {
            // character.team
            float shortestDistance = Mathf.Infinity;

            Transform previousTarget = currentTarget;
            currentTarget = null;

            if (character?.characterGun?.currentWeapon?.weaponSetting.typeWeapon == TypeWeapon.Melee)
            {
                // nhat sung
                stateAI = StateAI.FindGun;
                foreach (Weapon target in MapManager.Instance.currentMap.allWeapons)
                {
                    if (!target || target.owner)
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(Vector2.Distance(transform.position, target.transform.position));
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        currentTarget = target.transform;
                    }
                }
            }
            else
            {
                Transform targetTemple = null;
                foreach (Transform target in targets)
                {
                    if (target.GetComponent<Character>() && target.GetComponent<Character>().isDead)
                    {
                        continue;
                    }

                    float distance = Mathf.Abs(Vector2.Distance(transform.position, target.position));
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        targetTemple = target;
                    }
                }

                if (targetTemple != null && Vector2.Distance(targetTemple.position, transform.position) < maxDistanceToFindPlayer)
                {
                    stateAI = StateAI.FindPlayer;
                    currentTarget = targetTemple;
                }
                else
                {
                    if (previousTarget && stateAI == StateAI.FindRandom && Vector2.Distance(transform.position, previousTarget.position) > 3)
                    {
                        currentTarget = previousTarget;
                    }
                    else
                    {
                        targetTemple = MapManager.Instance.currentMap.randomPositonForAI[Random.Range(0, MapManager.Instance.currentMap.randomPositonForAI.Length)];
                        currentTarget = targetTemple;
                        stateAI = StateAI.FindRandom;
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxDistanceToFindPlayer);
        }
    }
}
