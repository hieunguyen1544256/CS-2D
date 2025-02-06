using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "2PlayerSoundObject")]
public class SoundObject : ScriptableObject
{
    public AudioClip[] allSoundEffect;
    public Game2PlayerSound[] allSoundEffectByName;
    public Game2PlayerSoundList[] allSoundEffectListByName;
    public AudioClip[] bgClip;
}
