using System.Collections.Generic;
using UnityEngine;

public class AudioHandler
{
    public enum AudioType
    {
        Game_Win,
        Game_Fail,
        Player_Jump,
        Player_Land,
        Pickup_Coin,
        Pickup_Trap,
        Button_Click,
        Testing_PlayerMove
    }

    private static Dictionary<AudioType, float> audioTimerDictionary;
    private static GameObject oneShotGameObject;
    private static AudioSource oneShotAudioSource;

    // we initialized dictionary for delayed sound.
    // Initalize this in Awake which script you want to use delayed sound.
    public static void Initalize()
    {
        audioTimerDictionary = new Dictionary<AudioType, float>();
        audioTimerDictionary[AudioType.Testing_PlayerMove] = 0;
    }

    public static void PlayAudio(AudioType audioType, Vector3 position)
    {
        if (!SettingsUI.IsSoundOn) return;

        if (CanPlayAudio(audioType))
        {
            GameObject audioGameObject = new GameObject("Audio");
            audioGameObject.transform.position = position;

            AudioSource audioSource = audioGameObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(audioType);
            audioSource.Play();

            Object.Destroy(audioGameObject, audioSource.clip.length);// Destroy when clip is finished
        }
    }

    public static void PlayAudio(AudioType audioType, float volume = 1f, float pitch = 1f)
    {
        if (!SettingsUI.IsSoundOn) return;

        if (CanPlayAudio(audioType))
        {
            if (oneShotGameObject == null)
            {
                oneShotGameObject = new GameObject("One Shot Audio");
                oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();
            }

            oneShotAudioSource.volume = volume;
            oneShotAudioSource.pitch = pitch;
            oneShotAudioSource.PlayOneShot(GetAudioClip(audioType));
        }
    }

    // Add delayed audios here.
    private static bool CanPlayAudio(AudioType audioType)
    {
        switch (audioType)
        {
            default:
                return true;
            case AudioType.Testing_PlayerMove:
                if (audioTimerDictionary.ContainsKey(audioType))
                {
                    float lastTimePlayed = audioTimerDictionary[audioType];
                    float playerMoveTimerMax = .15f;
                    if (lastTimePlayed + playerMoveTimerMax < Time.time)
                    {
                        audioTimerDictionary[audioType] = Time.time;
                        return true;
                    }
                    else return false;
                }
                else return true;
                //break;
        }
    }

    private static AudioClip GetAudioClip(AudioType audioType)
    {
        List<AudioClip> multipleAudioClips = new List<AudioClip>();
        foreach (Audio audio in AudioData.Instance.Audios)
        {
            if (audio.type == audioType)
                multipleAudioClips.Add(audio.clip);
        }

        if (multipleAudioClips.Count == 1)
            return multipleAudioClips[0];
        else if (multipleAudioClips.Count > 1)
            return multipleAudioClips[Random.Range(0, multipleAudioClips.Count)];
        else
        {
            Debug.LogError("Audio " + audioType + " not found!");
            return null;
        }
    }
}
