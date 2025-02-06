using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;
[Serializable]
public class Game2PlayerSound
{
    public string name;
    public AudioClip clip;
}
[Serializable]
public class Game2PlayerSoundList
{
    public string name;
    public AudioClip[] clip;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] allSoundEffect;
    public Game2PlayerSound[] allSoundEffectByName;
    public Game2PlayerSoundList[] allSoundEffectListByName;
    public AudioClip[] bgClip;
    public AudioSource bgSound;
    public AudioSource soundEffect;
    public AudioClip uiBtnSound;
    public AudioMixer audioMixer;
    public bool isPlayedBgRandom = true;

    public bool useSoundObject = false;
    public SoundObject soundObject;
    private void Awake()
    {
        instance = this;

    }

    // Start is called before the first frame update
    void Start()
    {
        if ((useSoundObject == false ? bgClip.Length > 0 : soundObject.bgClip.Length > 0) && isPlayedBgRandom == true)
        {
            if (!useSoundObject)
                bgSound.clip = bgClip[UnityEngine.Random.Range(0, bgClip.Length)];
            else
                bgSound.clip = soundObject.bgClip[UnityEngine.Random.Range(0, soundObject.bgClip.Length)];
            bgSound.Play();
            if (PlayerPrefs.GetInt("SoundBGEnable") != 0)
                bgSound.mute = true;
            else
                bgSound.mute = false;
        }
        if (audioMixer != null)
            AudiosMixerHandle("GameSFX");
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void PlayBGSound(int index, float defautVolumn = 1)
    {
        if (!useSoundObject)
            bgSound.clip = bgClip[index];
        else
            bgSound.clip = soundObject.bgClip[index];
        bgSound.Play();
        if (PlayerPrefs.GetInt("SoundBGEnable") != 0)
            bgSound.mute = true;
        else
            bgSound.mute = false;
    }
    public void PlaySoundEffect(int index, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            soundEffect.volume = defautVolumn;
            if (!useSoundObject)
                soundEffect.PlayOneShot(allSoundEffect[index]);
            else
                soundEffect.PlayOneShot(soundObject.allSoundEffect[index]);

        }
    }
    public void PlaySoundEffect(string name, float defautVolumn = 1)
    {
        Game2PlayerSound s = Array.Find(useSoundObject == false ? allSoundEffectByName : soundObject.allSoundEffectByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip);
            }
        }
    }
    public void PlaySoundEffect(string name, Game2PlayerSound[] allSoundEffectByName, float defautVolumn = 1)
    {
        Game2PlayerSound s = Array.Find(allSoundEffectByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip);
            }
        }
    }
    public void PlaySoundEffect(string name, Game2PlayerSound[] allSoundEffectByName, AudioSource soundEffect, float defautVolumn = 1)
    {
        Game2PlayerSound s = Array.Find(allSoundEffectByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip);
            }
        }
    }
    public void PlaySoundEffectList(string name, float defautVolumn = 1)
    {
        Game2PlayerSoundList s = Array.Find(useSoundObject == false ? allSoundEffectListByName : soundObject.allSoundEffectListByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip[UnityEngine.Random.Range(0, s.clip.Length)]);
            }
        }
    }
    public void PlaySoundEffectLists(string name, AudioSource soundEffect, float defautVolumn = 1)
    {
        Game2PlayerSoundList s = Array.Find(useSoundObject == false ? allSoundEffectListByName : soundObject.allSoundEffectListByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip[UnityEngine.Random.Range(0, s.clip.Length)]);
            }
        }
    }
    public void PlaySoundEffectLists(string name, Game2PlayerSoundList[] allSoundEffectListByName, AudioSource soundEffect, float defautVolumn = 1)
    {
        Game2PlayerSoundList s = Array.Find(allSoundEffectListByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.volume = defautVolumn;
                soundEffect.PlayOneShot(s.clip[UnityEngine.Random.Range(0, s.clip.Length)]);
            }
        }
    }

    public void PlaySoundEffect(AudioSource audioSource, List<AudioClip> audioClips, int index, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            soundEffect.volume = defautVolumn;
            soundEffect.PlayOneShot(audioClips[index]);

        }
    }

    public void PlayLoopSound(int index, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            if (!useSoundObject)
                soundEffect.clip = allSoundEffect[index];
            else
                soundEffect.clip = soundObject.allSoundEffect[index];
            soundEffect.volume = defautVolumn;
            soundEffect.loop = true;
            soundEffect.Play();
        }
    }

    public void PlayLoopSound(string name, float defautVolumn = 1)
    {
        Game2PlayerSound s = Array.Find(useSoundObject == false ? allSoundEffectByName : soundObject.allSoundEffectByName, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound Not Found: " + name);
        }
        else
        {
            if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            {
                soundEffect.clip = s.clip;
                soundEffect.volume = defautVolumn;
                soundEffect.loop = true;
                soundEffect.Play();
            }
        }
    }


    //---Hiep---
    public void PlayLoopSound(AudioSource audioSource, List<AudioClip> audioClips, int index, float defautVolumn = 1, bool loop = true)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            audioSource.clip = audioClips[index];
            audioSource.volume = defautVolumn;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    public void PlayLoopSound(AudioSource audioSource, AudioClip audioClip, float defautVolumn = 1, bool loop = true)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            audioSource.clip = audioClip;
            audioSource.volume = defautVolumn;
            audioSource.loop = loop;
            audioSource.Play();
        }
    }

    public void PlayLoopSound(AudioClip audioClip, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            bgSound.clip = audioClip;
            bgSound.volume = defautVolumn;
            bgSound.loop = true;
            bgSound.Play();
        }
    }
    public void PlaySoundEffect(AudioSource audioSource, AudioClip clip, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            audioSource.volume = defautVolumn;
            audioSource.PlayOneShot(clip);

        }
    }
    public void PlaySoundEffect(AudioClip clip, float defautVolumn = 1)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            soundEffect.volume = defautVolumn;
            soundEffect.PlayOneShot(clip);

        }
    }

    //----------
    public void StopLoopSound()
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            soundEffect.clip = null;
            soundEffect.loop = false;
            soundEffect.Stop();
        }
    }
    //---Hiep---
    public void StopLoopSound(AudioSource audioSource)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
        {
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.Stop();

        }
    }
    //----------
    public void PlayUIButtonSound()
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") == 0)
            soundEffect.PlayOneShot(uiBtnSound);
    }

    void AudiosMixerHandle(string key)
    {
        if (PlayerPrefs.GetInt("SoundEffectEnable") is 1)
        {
            audioMixer.SetFloat(key, 0);
        }
        else if (PlayerPrefs.GetInt("SoundEffectEnable") is -1)
        {
            audioMixer.SetFloat(key, -80);
        }
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
