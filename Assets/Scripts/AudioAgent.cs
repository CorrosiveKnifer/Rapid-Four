﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Michael Jordan
/// </summary>
public class AudioAgent : MonoBehaviour
{
    public AudioClip[] AudioClips;

    //Agent's local volume
    public float AgentSEVolume = 1f;
    public float AgentBGVolume = 1f;

    public float SoundRange = 50.0f;

    private float savedSEVolume = 1f;
    private float savedBGVolume = 1f;

    public class AudioPlayer
    {
        public AudioPlayer(AudioSource _source) { isSoundEffect = false; source = _source; }
        public bool isSoundEffect { get; set; }
        public bool is3DSound { get; set; } = false;
        public AudioSource source { get; private set; }

        public float volume = 1.0f; //Local volume to the player
    }

    public Dictionary<string, AudioPlayer> AudioLibrary { get; private set; }

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        AudioManager.GetInstance().AddAgent(this);

        AudioLibrary = new Dictionary<string, AudioPlayer>();
        for (int i = 0; i < AudioClips.Length; i++)
        {
            InitialiseAudio(AudioClips[i].name, AudioClips[i]);
        }
    }

    protected virtual void Update()
    {
        foreach (var item in AudioLibrary)
        {
            if (item.Value.isSoundEffect)
            {
                float ratio = 1.0f;
                if (item.Value.is3DSound)
                {
                    float dist = GameManager.GetInstance().ClosestPlayerDistance(transform.position);
                    ratio = Mathf.Clamp(1.0f - dist / SoundRange, 0.0f, 1.0f);
                }
                item.Value.source.volume = GetSoundEffectVolume() * item.Value.volume * AgentSEVolume * ratio;
            }  
            else
                item.Value.source.volume = GetBackgroundVolume() * item.Value.volume * AgentBGVolume;
        }
    }

    private void OnDestroy()
    {
        AudioManager.GetInstance().RemoveAgent(this);
    }

    private void InitialiseAudio(string title, AudioClip clip)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.volume = 1.0f;
        source.priority = 255;

        AudioLibrary.Add(title, new AudioPlayer(source));
    }

    public bool PlaySoundEffect(string title, bool isLooping = false, int priority = 255, float pitch = 1.0f)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].source.loop = isLooping;
            AudioLibrary[title].source.priority = priority;
            AudioLibrary[title].isSoundEffect = true;
            AudioLibrary[title].source.pitch = pitch;
            AudioLibrary[title].source.Play();
            return true;
        }
        return false;
    }
    public bool Play3DSoundEffect(string title, bool isLooping = false, int priority = 255, float pitch = 1.0f)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].source.loop = isLooping;
            AudioLibrary[title].source.priority = priority;
            AudioLibrary[title].isSoundEffect = true;
            AudioLibrary[title].is3DSound = true;
            AudioLibrary[title].source.pitch = pitch;
            AudioLibrary[title].source.Play();
            return true;
        }
        return false;
    }
    public bool PlayBackground(string title, bool isLooping = false, int priority = 255)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].source.loop = isLooping;
            AudioLibrary[title].source.priority = priority;
            AudioLibrary[title].isSoundEffect = false;
            AudioLibrary[title].source.Play();
            return true;
        }
        return false;
    }

    public bool SetLocalVolume(string title, float volume)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            AudioLibrary[title].volume = volume;
            return true;
        }
        return false;
    }

    public bool SetPitch(string title, float pitch)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            player.source.pitch = pitch;
            return true;
        }
        return false;
    }

    public bool IsAudioStopped(string title)
    {
        AudioPlayer player;

        if (AudioLibrary.TryGetValue(title, out player))
        {
            return !player.source.isPlaying;
        }
        Debug.LogError($"StopAudio failed because {title} source was not found.");
        return true;
    }

    public void StopAudio(string title)
    {
        AudioPlayer player;
        if (AudioLibrary.TryGetValue(title, out player))
        {
            player.source.Stop();
            return;
        }
        Debug.LogError($"StopAudio failed because audio source was not found.");
    }

    public void StopAllAudio()
    {
        foreach (var item in AudioLibrary)
        {
            item.Value.source.Stop();
        }
    }

    private float GetSoundEffectVolume()
    {
        return GameManager.MasterVolume * GameManager.SoundEffectVolume;
    }

    private float GetBackgroundVolume()
    {
        return GameManager.MasterVolume * GameManager.BackGroundVolume;
    }

    public IEnumerator PlaySoundEffectDelayed(string title, float secondsDelayed, bool IsLooped = false)
    {
        yield return new WaitForSecondsRealtime(secondsDelayed);

        PlaySoundEffect(title, IsLooped);

        yield return null;
    }
    public IEnumerator PlayBackgroundDelayed(string title, float secondsDelayed, bool IsLooped = false)
    {
        yield return new WaitForSecondsRealtime(secondsDelayed);

        PlayBackground(title, IsLooped);

        yield return null;
    }

    public IEnumerator PlaySoundEffectSolo(string title)
    {
        AudioManager.GetInstance().MakeSolo(this);

        PlaySoundEffect(title);
        do
        {
            yield return new WaitForEndOfFrame();
        } while (!IsAudioStopped(title));

        AudioManager.GetInstance().UnMuteAll();

        yield return null;
    }

    public void Mute()
    {
        if (AgentBGVolume == 0f || AgentSEVolume == 0f)
            return;

        savedBGVolume = AgentBGVolume;
        savedSEVolume = AgentSEVolume;

        AgentBGVolume = 0f;
        AgentSEVolume = 0f;
    }

    public void UnMute()
    {
        if(AgentBGVolume == 0f && AgentSEVolume == 0f)
        {
            AgentBGVolume = savedBGVolume;
            AgentSEVolume = savedSEVolume;
        }
    }
}
