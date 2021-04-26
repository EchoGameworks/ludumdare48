using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundEffects { EnterWater, CompleteLevel, ActiveClick, ActiveRelease, LightHit, HeavyHit, EyePopOut, ClickDialog, MaxPower }

    public static AudioManager instance;

    [Range(0f, 1f)]
    public float MasterMusicVolume = 1f;
    [Range(0f, 1f)]
    public float MasterSFXVolume = 1f;

    public List<Music> MusicPlaylist;
    private List<Music> UnheardMusicPlaylist;
    private Music currentSong;
    public List<Sound> Sounds;

    [SerializeField] private bool canPlay = true;
    [SerializeField] private bool onHold = false;
    [SerializeField] private bool isNegativeOverruled = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Music m in MusicPlaylist)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.Clip;
            m.source.volume = m.Volume;
            m.source.pitch = m.Pitch;
        }
        UnheardMusicPlaylist = new List<Music>(MusicPlaylist);
        PlaySong();

        foreach (Sound s in Sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.playOnAwake = false;
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
        }
    }

    public void PlaySong()
    {
        if (MasterMusicVolume == 0) return;
        if (currentSong != null && currentSong.source.isPlaying) return;
        if (UnheardMusicPlaylist.Count == 0) UnheardMusicPlaylist = new List<Music>(MusicPlaylist);
        Music song = UnheardMusicPlaylist.FirstOrDefault();
        UnheardMusicPlaylist.Remove(song);
        currentSong = song;
        song.source.volume = MasterMusicVolume * song.Volume;
        float songLength = song.Clip.length;
        if (song.source.time > 0 && song.source.time < songLength - 1f)
        {
            currentSong.source.UnPause();
        }
        else
        {
            song.source.Play();
        }

        LeanTween.delayedCall(songLength + 2f, () => PlaySong());
    }

    public void PlaySound(SoundEffects soundEffect)
    {
        Sound s = Sounds.FirstOrDefault(o => o.SoundName == soundEffect);
        if (s == null || !canPlay || onHold) return;
        s.source.volume = MasterSFXVolume * s.Volume;
        //s.source.pitch = s.Pitch;
        if(soundEffect == SoundEffects.HeavyHit)
        {
            s.source.pitch = Random.Range(s.Pitch * 0.8f, s.Pitch * 1.2f);
        }
        else
        {
            s.source.pitch = Random.Range(s.Pitch * 0.95f, s.Pitch * 1.1f);
        }
        
        //print("playing: " + soundEffect + " at " + s.source.volume + " (" + MasterSFXVolume + " | " + s.Volume + ")");
        s.source.Play();
    }
}
