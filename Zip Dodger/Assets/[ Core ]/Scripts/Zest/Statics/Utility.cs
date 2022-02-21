using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZestGames.Utility
{
    public static class Utils
    {
        #region Function Delayer

        /// <summary>
        /// Call this function if you want to delay a function.
        /// Any function can be but into this with lambda expression like '() =>'
        /// USAGE: this.DoActionAfterDelay(...);
        /// </summary>
        /// <param name="mono">This is required because Coroutine requires MonoBehaviour.</param>
        /// <param name="delayTime">Function will be executed after this time.</param>
        /// <param name="action">Function you want to delay.</param>
        public static void DoActionAfterDelay(this MonoBehaviour mono, float delayTime, UnityAction action)
        {
            mono.StartCoroutine(ExecuteAction(delayTime, action));
        }

        private static IEnumerator ExecuteAction(float delayTime, UnityAction action)
        {
            yield return new WaitForSeconds(delayTime);
            action.Invoke();
            yield break;
        }

        #endregion

        #region String Shortener (For Gold Quantity like 10K, 98M)

        /// <summary>
        /// This function converts given int value to K and M type.
        /// i.e. 10.000 to 10K, 1.500.000 to 1.5M
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string IntToStringShortener(int value)
        {
            if (value < 10000)
                return value.ToString();
            else if (value >= 10000 && value < 1000000)
                return (value / 1000).ToString() + "K";
            else if (value >= 1000000 && value < 1000000000)
                return (value / 1000000).ToString() + "M";
            else if (value >= 1000000000)
                return (value / 1000000000).ToString() + "B";
            else
                return "";
        }

        /// <summary>
        /// This function converts given float value to K and M type.
        /// i.e. 10.000 to 10K, 1.500.000 to 1.5M
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FloatToStringShortener(float value)
        {
            if (value < 10000)
                return value.ToString();
            else if (value >= 10000 && value < 1000000)
                return (value / 1000).ToString() + "K";
            else if (value >= 1000000 && value < 1000000000)
                return (value / 1000000).ToString() + "M";
            else if (value >= 1000000000)
                return (value / 1000000000).ToString() + "B";
            else
                return "";
        }

        #endregion

        #region RNG (Random Number Generator)

        /// <summary>
        /// Rolls dice and returns true according to chance you enter.
        /// </summary>
        /// <param name="chance">Enter between 0 - 100</param>
        public static bool RollDice(int chance)
        {
            int dice = UnityEngine.Random.Range(1, 101);

            if (dice <= chance)
                return true;
            else
                return false;
        }

        #endregion

        /// <summary>
        /// Shuffles the element order of the specified list.
        /// </summary>
        public static void ShuffleList<T>(this List<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }

        /// <summary>
        /// Makes the object billboard.
        /// Cache main cam transform and put this function in LateUpdate
        /// </summary>
        /// <param name="thisMonoBehaviour">Put in 'this'. Function has to take a MonoBehaviour.</param>
        /// <param name="thisTransform"></param>
        /// <param name="camera">Cache Main Camera to a transform for this.</param>
        public static void Billboard(MonoBehaviour thisMonoBehaviour, Transform thisTransform, Transform camera)
        {
            thisMonoBehaviour.transform.LookAt(thisTransform.position + camera.forward);
        }
    }
}
