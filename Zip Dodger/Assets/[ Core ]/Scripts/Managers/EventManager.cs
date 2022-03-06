using System;
using System.Collections.Generic;

public static class EventManager
{
    public static event Action OnZipperCollected;

    static Dictionary<StaticEvents, Action> events = new Dictionary<StaticEvents, Action>(){
            { StaticEvents.ZipperCollected, OnZipperCollected }
        };

    public static void InvokeEvent(StaticEvents e) => events[e]?.Invoke();
    public static void Invoke(StaticEvents e) => OnZipperCollected?.Invoke();
}

public enum StaticEvents
{
    ZipperCollected
}
