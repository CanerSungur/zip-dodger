using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script loads matching scene according to Level. Makes game infinite leveled.
/// This is based on scene count that are in build index.
/// </summary>
public static class LevelHandler
{
    public enum SceneType { First = 1, Second = 2, Third = 3, Forth = 4, Fifth = 5, Sixth = 6, Seventh = 7, Eighth = 8, Ninth = 9 }
    public static SceneType _SceneType;

    public static int Level;
    public static int CurrentLevel;
    private static int lastSceneBuildIndex;

    #region Tutorial Section

    public static bool IsTutorialFinished = true;
    public static bool SkippedTutorial = true;

    #endregion

    public static void IncreaseLevel()
    {
        Level++;
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.Save();
    }

    public static void DeleteLevelData() => PlayerPrefs.SetInt("Level", 1);

    #region Button Functions

    public static void NextScene()
    {
        SceneManager.LoadScene(GetSceneBuildIndexToBeLoaded());
    }

    public static void Restart()
    {
        SceneManager.LoadScene(CurrentLevel);
    }

    #endregion

    public static int GetSceneBuildIndexToBeLoaded()
    {
        // Uncomment this and run game once to reset level.
        //DeleteLevelData();

        #region Tutorial Section

        //if (PlayerPrefs.GetInt("TutorialFinished") == 1 || SkippedTutorial)
        //{
        //    IsTutorialFinished = true;
        //    Debug.Log("TUTORIAL HAS FINISHED!");
        //}
        //else
        //    IsTutorialFinished = false;

        #endregion

        #region Load Level

        if (PlayerPrefs.GetInt("Level") <= 1)
            Level = 1;
        else
            Level = PlayerPrefs.GetInt("Level");

        #endregion

        //Level = 11;

        lastSceneBuildIndex = SceneManager.sceneCountInBuildSettings - 1;
        int index = Level % lastSceneBuildIndex;
        if (index == 0)
            CurrentLevel = lastSceneBuildIndex;
        else
            CurrentLevel = index;

        _SceneType = (SceneType)CurrentLevel;

        Debug.Log("Level: " + CurrentLevel);
        Debug.Log("Scene Type: " + _SceneType);

        Debug.Log("LEVEL: " + Level);

        return CurrentLevel;
    }
}
