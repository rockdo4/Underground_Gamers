using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Button_Click,
    Option_Button,
    Level_Up,
    Xp_Gauge,
    Positive_PopUp,
    Negative_PopUp,
    Hit,
    Shoot,
}

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance
    {
        get
        {
            if (soundPlayer == null)
            {
                soundPlayer = FindObjectOfType<SoundPlayer>();
            }
            return soundPlayer;
        }
    }

    private static SoundPlayer soundPlayer;

    public AudioSource bgmAudio;
    public AudioSource effectAudio;
    [SerializeField]
    private float fadeOutTime = 1f;
    [SerializeField]
    private AudioClip BGM_Title;
    [SerializeField]
    private AudioClip[] BGM_Lobby;
    [SerializeField]
    private AudioClip[] BGM_Game;
    [SerializeField]
    private AudioClip[] EffectSounds;

    private void Awake()
    {
        bgmAudio = GetComponent<AudioSource>();
        bgmAudio.clip = BGM_Title;
        bgmAudio.Play();
    }

    public void EnterLobbyMusic(int index)
    {
        DOTween.defaultTimeScaleIndependent = true;
        bgmAudio.DOFade(0, fadeOutTime).OnComplete(() => EnterLobbyMusicPlay(index));
    }

    public void EnterLobbyMusicPlay(int index)
    {
        bgmAudio.clip = BGM_Lobby[index];
        bgmAudio.volume = 1;
        bgmAudio.Play();
    }

    public void EnterGameMusic()
    {
        DOTween.defaultTimeScaleIndependent = true;
        bgmAudio.DOFade(0, fadeOutTime).OnComplete(EnterGameMusicPlay);
    }

    public void EnterGameMusicPlay()
    {
        bgmAudio.clip = BGM_Game[0];
        bgmAudio.volume = 1;
        bgmAudio.Play();
    }

    public void EnterBattleMusicPlay(int index)
    {
        bgmAudio.clip = BGM_Game[index];
        bgmAudio.volume = 1;
        bgmAudio.Play();
    }

    public void PlayEffectSound(int index)
    {
        effectAudio.PlayOneShot(EffectSounds[index]);
    }

    public void PlayMusic(AudioClip music)
    {
        bgmAudio.clip = music;
        bgmAudio.Play();
    }

    public void PauseMusic()
    {
        bgmAudio.Pause();
    }

    public void ResumeMusic()
    {
        bgmAudio.UnPause();
    }

    public void StopMusic()
    {
        bgmAudio.Stop();
    }
}
