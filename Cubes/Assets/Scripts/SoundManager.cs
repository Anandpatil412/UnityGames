using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private static GameObject oneShotSoundGameObject;
    private static AudioSource oneShotSoundAudioSourceGameObject;

    private static GameObject oneLoopSoundGameObject;
    private static AudioSource oneLoopSoundAudioSourceGameObject;

    public enum Sound
    {
        GameBG,
        Timeout,
        Burst,
        WrongMove
    }

    public static void PlaySound(Sound sound)
    {
        if (oneShotSoundGameObject == null)
        {
            oneShotSoundGameObject = new GameObject("oneShotSoundGameObject");
            oneShotSoundAudioSourceGameObject = oneShotSoundGameObject.AddComponent<AudioSource>();
        }

        oneShotSoundAudioSourceGameObject.PlayOneShot(GetAudioClip(sound));
    }

    public static void PlayLoopSound(Sound sound)
    {
        if (oneLoopSoundGameObject == null)
        {
            oneLoopSoundGameObject = new GameObject("oneLoopSoundGameObject");
            oneLoopSoundAudioSourceGameObject = oneLoopSoundGameObject.AddComponent<AudioSource>();
        }

        oneLoopSoundAudioSourceGameObject.loop = true;
        oneLoopSoundAudioSourceGameObject.clip = GetAudioClip(sound);

        oneLoopSoundAudioSourceGameObject.Play();
    }

    public static AudioClip GetAudioClip(Sound sound)
    {
        foreach(GameAssets.SoundAudioClip soundClip in GameAssets.i.soundArray)
        {
            if(sound == soundClip.sound)
                return soundClip.audioclip;
        }

        return null;
    }

    public static void StopSound()
    {
        if (oneShotSoundAudioSourceGameObject != null)
            oneShotSoundAudioSourceGameObject.Stop();
    }

    public static void StopLoopSound()
    {
        if (oneLoopSoundAudioSourceGameObject != null)
            oneLoopSoundAudioSourceGameObject.Stop();
    }

}
