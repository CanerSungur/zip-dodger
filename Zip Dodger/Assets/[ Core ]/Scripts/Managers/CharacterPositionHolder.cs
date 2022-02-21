using System.Collections.Generic;

public static class CharacterPositionHolder
{
    public static Player PlayerInScene;

    private static List<AI> aisInScene;
    public static List<AI> AIsInScene { get { return aisInScene == null ? aisInScene = new List<AI>() : aisInScene; } }

    public static void AddAI(AI ai)
    {
        if (!AIsInScene.Contains(ai))
            AIsInScene.Add(ai);
    }

    public static void RemoveAI(AI ai)
    {
        if (AIsInScene.Contains(ai))
            AIsInScene.Remove(ai);
    }
}
