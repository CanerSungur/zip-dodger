using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZestGames.AIBrain
{
    public static class AIBrain
    {
        /// <summary>
        /// Moves object towards given Vector3 Target with given speed.
        /// </summary>
        /// <param name="thisTransform">Transform of this object.</param>
        /// <param name="target">Where to move.</param>
        /// <param name="speed"></param>
        public static void MoveToTarget(Transform thisTransform, Vector3 target, float speed)
        {
            Vector3 direction = target - thisTransform.position;

            thisTransform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }

        /// <summary>
        /// Makes object look towards the given Vector3 Target with given turn speed.
        /// </summary>
        /// <param name="thisTransform">Transform of this object.</param>
        /// <param name="target">Where to look.</param>
        /// <param name="turnSpeed"></param>
        public static void LookAtTarget(Transform thisTransform, Vector3 target, float turnSpeed)
        {
            Vector3 direction = target - thisTransform.position;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(thisTransform.rotation, lookRotation, turnSpeed * Time.deltaTime).eulerAngles;
            thisTransform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        /// <summary>
        /// Cheks if object has reached Target point according to given distance limit.
        /// </summary>
        /// <param name="thisTransform"></param>
        /// <param name="target">This point will be checked if reached or not.</param>
        /// <param name="limit">This is distance limit. If we're closer than this limit we've reached. Default value: 2f</param>
        /// <returns>True if reached, False if not.</returns>
        public static bool IsTargetReached(Transform thisTransform, Vector3 target, float limit = 2f)
        {
            float distance = (target - thisTransform.position).sqrMagnitude;
            if (distance <= limit)
                return true;

            else return false;
        }

        /// <summary>
        /// This function contains MoveToTarget, LookAtTarget, IsTargetReached functions.
        /// Makes object go to Target Transform. Checks limit for distance if we've reached or not. 
        /// This function is suitable for Attacking, Building, Upgrading and Converting Decisions.
        /// </summary>
        /// <param name="thisTransform"></param>
        /// <param name="target">Target Transform</param>
        /// <param name="speed"></param>
        /// <param name="turnSpeed"></param>
        /// <param name="limit">Distance Limit to check we've reached or not. Default value: 0.1f</param>
        public static void ChaseTarget(Transform thisTransform, Transform target, float speed, float turnSpeed, float limit = .1f)
        {
            if (!IsTargetReached(thisTransform, target.position, limit))
            {
                MoveToTarget(thisTransform, target.position, speed);
                LookAtTarget(thisTransform, target.position, turnSpeed);
            }
            else
            {
                LookAtTarget(thisTransform, target.position, turnSpeed);
            }
        }

        /// <summary>
        /// Finds and returns the closest Transform from given list of transforms.
        /// </summary>
        /// <param name="thisTransform">Transform who is doing this search.</param>
        /// <param name="list">Transform list that we want to find the closest. Like a target list.</param>
        /// <returns></returns>
        public static Transform FindClosestTransform(Transform thisTransform, List<Transform> list)
        {
            if (list == null || list.Count == 0) return null;

            float shortestDistance = Mathf.Infinity;
            Transform closestTransform = null;

            for (int i = 0; i < list.Count; i++)
            {
                float distanceToTransform = (thisTransform.position - list[i].position).sqrMagnitude;
                if (distanceToTransform < shortestDistance && thisTransform != list[i])
                {
                    shortestDistance = distanceToTransform;
                    closestTransform = list[i];
                }
            }

            return closestTransform;
        }
    }
}
