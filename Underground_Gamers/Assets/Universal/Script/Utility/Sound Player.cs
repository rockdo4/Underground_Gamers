using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private AudioSource m_AudioSource;
    [SerializeField]
    private float fadeOutTime = 1f;
    [SerializeField]
    private AudioClip BGM_Title;
    [SerializeField]
    private AudioClip[] BGM_Lobby;
    [SerializeField]
    private AudioClip[] BGM_Game;

    private void Awake()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_AudioSource.clip = BGM_Title;
        m_AudioSource.Play();
    }

    public void EnterLobbyMusic()
    {
        DOTween.defaultTimeScaleIndependent = true;
        m_AudioSource.DOFade(0, fadeOutTime).OnComplete(EnterLobbyMusicPlay);
    }

    public void EnterLobbyMusicPlay()
    {
        m_AudioSource.clip = BGM_Lobby[0];
        m_AudioSource.volume = 1;
        m_AudioSource.Play();
    }

    public void EnterGameMusic()
    {
        DOTween.defaultTimeScaleIndependent = true;
        m_AudioSource.DOFade(0, fadeOutTime).OnComplete(EnterGameMusicPlay);
    }

    public void EnterGameMusicPlay()
    {
        m_AudioSource.clip = BGM_Game[0];
        m_AudioSource.volume = 1;
        m_AudioSource.Play();
    }

    public void PlayMusic(AudioClip music)
    {
        m_AudioSource.clip = music;
        m_AudioSource.Play();
    }

    public void PauseMusic()
    {
        m_AudioSource.Pause();
    }

    public void ResumeMusic()
    {
        m_AudioSource.UnPause();
    }

    public void StopMusic()
    {
        m_AudioSource.Stop();
    }
}
