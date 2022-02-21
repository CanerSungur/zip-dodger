using UnityEngine;
using TMPro;

namespace ZestGames.UIUtility
{
    public static class WorldSpace
    {
        /// <summary>
        /// Shows an animated message at given position in world space and destroys itself at given time.
        /// </summary>
        /// <param name="popupPrefab">Enter a World Space Popup Prefab.</param>
        /// <param name="position"></param>
        /// <param name="message">Enter a message to show.</param>
        /// <param name="destroyTime">Default is 2 seconds.</param>
        public static void Popup(GameObject popupPrefab, Vector3 position, string message, float destroyTime = 2f)
        {

            GameObject go = GameObject.Instantiate(popupPrefab, position, Quaternion.identity);
            go.GetComponentInChildren<TextMeshProUGUI>().text = message;
            GameObject.Destroy(go, destroyTime);
        }
    }

    public static class ScreenSpace
    {
        /// <summary>
        /// Shows an animated message at given position on canvas and destroys itself at given time.
        /// </summary>
        /// <param name="popupPrefab">Enter a Screen Space Popup Prefab.</param>
        /// <param name="position"></param>
        /// <param name="message">Enter a message to show.</param>
        /// <param name="destroyTime">Default is 2 seconds.</param>
        public static void Popup(GameObject popupPrefab, Vector3 position, string message, float destroyTime = 2f)
        {
            GameObject go = GameObject.Instantiate(popupPrefab, position, Quaternion.identity);
            go.transform.GetChild(0).position = position;
            go.GetComponentInChildren<TextMeshProUGUI>().text = message;
            GameObject.Destroy(go, destroyTime);
        }
    }
}
