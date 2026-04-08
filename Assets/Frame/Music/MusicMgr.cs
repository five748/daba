using System.Collections.Generic;
using UnityEngine;
using Table;
public class MusicMgr : SingleMono2<MusicMgr>
{
    public class AudioData
    {
        public bool isBg;
        public string bgName;
        public AudioSource audio;
        public bool isLoop;
        public float volume = 1;
        public float pitch;
        public AudioClip clip;
        public void Play()
        {
            audio.loop = isLoop;
            SetVolume();
            audio.pitch = 1;
            audio.clip = clip;
            //Debug.LogError(audio.clip.name + ":" + audio.clip.loadState);
#if UNITY_EDITOR
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return;
            }
#endif
            clip.LoadAudioData();
            audio.Play();
        }
        public void Pause()
        {
            audio.Pause();
        }
        public void RePlay()
        {
            audio.Play();
        }
        public void Stop()
        {
            audio.Stop();
        }
        public void SetVolume()
        {
            if (isBg)
            {
                audio.volume = volume * MusicMgr.Instance.bgVolume;
            }
            else
            {
                audio.volume = volume * MusicMgr.Instance.soundVolume;
            }
        }
    }
    //主背景音乐
    public AudioData MainBgAudio;
    //其它背景音乐
    public AudioData OtherBgAudio;
    //1音效通道一
    //2音效通道二
    //3音效通道三
    public Dictionary<int, AudioData> SoundAudios;
    public Dictionary<int, bgm> bgTab;
    public Dictionary<int, sound> soundTab;
    public Dictionary<int, List<int>> sceneBgms = new Dictionary<int, List<int>>();
    public Dictionary<int, int> sceneChooseBgms = new Dictionary<int, int>();
    public List<AudioSource> audioSources = new List<AudioSource>();
    private bool isInited = false;
    public float bgVolume = 0f;
    public float soundVolume = 0f;
    public enum MusicType
    {
        bgm = 0,
        sound = 1,
    }
    public AudioData CreateOne(MusicType type)
    {
        AudioData audioData = new AudioData();
        audioData.audio = gameObject.AddComponent<AudioSource>();
        audioSources.Add(audioData.audio);
        audioData.pitch = 1;
        audioData.isBg = type == MusicType.bgm;
        return audioData;
    }
    public void Init(Dictionary<int, bgm> _bgTab, Dictionary<int, sound> _soundTab, bool isInitOther = false)
    {
        if (isInited)
        {
            return;
        }

        isInited = true;
        GameObject.DontDestroyOnLoad(this);
        MainBgAudio = CreateOne(MusicType.bgm);
        OtherBgAudio = CreateOne(MusicType.bgm);
        SoundAudios = new Dictionary<int, AudioData>();
        bgTab = _bgTab;
        soundTab = _soundTab;
        if (isInitOther)
            InitAudios();
    }
    Dictionary<int, AudioData> freeAudios;
    Dictionary<int, AudioData> laserAudios;
    Dictionary<int, AudioData> shootAudios;
    Dictionary<int, AudioData> bulletAudios;
    Dictionary<int, AudioData> resAudios;
    private void InitAudios()
    {
        freeAudios = new Dictionary<int, AudioData>();
        laserAudios = new Dictionary<int, AudioData>();
        shootAudios = new Dictionary<int, AudioData>();
        bulletAudios = new Dictionary<int, AudioData>();
        resAudios = new Dictionary<int, AudioData>();
        for (int i = 0; i < 15; i++)
        {
            freeAudios.Add(-i, CreateOne(MusicType.sound));
        }
        for (int i = 0; i < 15; i++)
        {
            shootAudios.Add(-i, CreateOne(MusicType.sound));
        }
        for (int i = 0; i < 5; i++)
        {
            resAudios.Add(-i, CreateOne(MusicType.sound));
        }
        for (int i = 0; i < 15; i++)
        {
            bulletAudios.Add(-i, CreateOne(MusicType.sound));
        }
    }
    public void PlayLstSound(int mid)
    {
        PlayLstSoundBase(mid, freeAudios);
    }
    public void PlayLaserLstSound(int mid)
    {
        PlayLstSoundBase(mid, laserAudios);
    }
    public void PlayShootLstSound(int mid)
    {
        PlayLstSoundBase(mid, shootAudios);
    }
    public void PlayBulletLstSound(int mid)
    {
        PlayLstSoundBase(mid, bulletAudios);
    }
    public void PlayResLstSound(int mid)
    {
        PlayLstSoundBase(mid, resAudios);
    }
    private void PlayLstSoundBase(int mid, Dictionary<int, AudioData> audios)
    {
        AudioData data = null;
        foreach (var item in audios)
        {
            if (!item.Value.audio.isPlaying)
            {
                data = item.Value;
                break;
            }
        }
        if (data == null)
        {
            return;
        }
        if (!isInited || SoundAudios == null)
        {
            return;
        }
        if (!soundTab.ContainsKey(mid))
        {
            Debug.LogError("找不到声音Id:" + mid);
            return;
        }
        var tabone = soundTab[mid];
        data.Stop();
        SetSound(tabone, data);
    }
    private float oldbgVolume;
    private float oldsoundVolume;
    public void PauseAll() {
        oldbgVolume = bgVolume;
        oldsoundVolume = soundVolume;
        SetBgVolume(0);
        SetSoundVolume(0);
    }
    public void BeginAll() {
        SetBgVolume(oldbgVolume);
        SetSoundVolume(oldsoundVolume);
    }
    public void SetBgVolume(float volume)
    {
        bgVolume = volume;
        MainBgAudio.SetVolume();
        OtherBgAudio.SetVolume();
    }
    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach (var item in SoundAudios)
        {
            item.Value.SetVolume();
        }
    }
    public string GetMusicNameById(int musicId)
    {
        return bgTab[musicId].path;
    }
    public void PlayBgm(int mid)
    {
        if (!isInited)
        {
            return;
        }
        if (!bgTab.ContainsKey(mid))
        {
            Debug.LogError("找不到背景音乐Id:" + mid);
            return;
        }
        var tabone = bgTab[mid];
        if (tabone.bgtype == 0)
        {
            //主背景音乐
            SetBg(tabone, MainBgAudio);
        }
        else
        {
            //副背景音乐
            SetBg(tabone, OtherBgAudio);
            MainBgAudio.Pause();
        }
    }
    public void PlaySound(int mid)
    {
        if (!isInited || SoundAudios == null)
        {
            return;
        }
        if (!soundTab.ContainsKey(mid))
        {
            Debug.Log("找不到声音Id:" + mid);
            return;
        }
        var tabone = soundTab[mid];
        if (!SoundAudios.ContainsKey(tabone.channel))
        {
            SoundAudios.Add(tabone.channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[tabone.channel];
        audio.Stop();
        SetSound(tabone, audio);
    }
    public void PlaySound(int mid, int channel)
    {
        if (!isInited || SoundAudios == null)
        {
            return;
        }
        if (!soundTab.ContainsKey(mid))
        {
            Debug.LogError("找不到声音Id:" + mid);
            return;
        }
        var tabone = soundTab[mid];
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        audio.Stop();
        SetSound(tabone, audio);
    }
    public void PlaySound(string name, int channel)
    {
        if (!isInited)
        {
            return;
        }
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        AssetLoadOld.Instance.LoadBaseAudio(name, (clip) =>
        {
            if (clip == null)
            {
                return;
            }
            audio.Stop();
            audio.clip = clip;
            audio.isLoop = false;
            audio.volume = 1;
            audio.Play();
        });
    }
    public void PlayHero(int heroId, int channel = 5)
    {
        if (!isInited)
        {
            return;
        }
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        audio.Stop();
        audio.isLoop = false;
        audio.pitch = 1;
        AssetLoadOld.Instance.LoadMinMusic(heroId.ToString(), (clip) =>
        {
            audio.clip = clip;
            audio.Play();
        });
    }
    public void PlayLiHuiHero(int heroId, int channel = 4)
    {
        if (!isInited)
        {
            return;
        }
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        audio.Stop();
        audio.isLoop = false;
        audio.pitch = 1;
        AssetLoadOld.Instance.LoadMinLiHuiOnMusic(heroId.ToString(), (clip) =>
        {
            audio.clip = clip;
            audio.Play();
        });
    }
    public void PlayGetHero(int heroId, int channel = 4)
    {
        if (!isInited)
        {
            return;
        }
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        audio.Stop();
        audio.isLoop = false;
        audio.pitch = 1;
        AssetLoadOld.Instance.LoadMinGetHeroOnMusic(heroId.ToString(), (clip) =>
        {
            audio.clip = clip;
            audio.Play();
        });
    }
    public void PlayOnHero(int heroId, int channel = 4)
    {
        if (!isInited)
        {
            return;
        }
        if (!SoundAudios.ContainsKey(channel))
        {
            SoundAudios.Add(channel, CreateOne(MusicType.sound));
        }
        var audio = SoundAudios[channel];
        audio.Stop();
        audio.isLoop = false;
        AssetLoadOld.Instance.LoadMinOnMusic(heroId.ToString(), (clip) =>
        {
            audio.clip = clip;
            audio.Play();
        });
    }
    public void PlaySkill(AudioClip clip, string volumeType, bool isLoop, float volume, float playTime, int channel, float speed = 1)
    {
        if (!isInited)
        {
            return;
        }
        if (volumeType == "暂停背景")
        {
            OtherBgAudio.clip = clip;
            OtherBgAudio.isLoop = true;
            OtherBgAudio.volume = volume;
            OtherBgAudio.pitch = 1;
            MainBgAudio.Pause();
            OtherBgAudio.Play();
            MonoTool.Instance.Wait(playTime, () =>
            {
                StopOtherBgm();
            });
        }
        else
        {
            if (!SoundAudios.ContainsKey(channel))
            {
                SoundAudios.Add(channel, CreateOne(MusicType.sound));
            }
            var audio = SoundAudios[channel];
            audio.Stop();
            audio.clip = clip;
            audio.isLoop = false;
            audio.volume = volume;
            audio.Play();
        }
    }
    public void StopOtherBgm()
    {
        if (!isInited)
        {
            return;
        }
        Debug.LogError("StopOthe");
        OtherBgAudio.Stop();
        MainBgAudio.RePlay();
    }
    public void RePlayBgm()
    {
        if (MainBgAudio != null)
            MainBgAudio.RePlay();
    }
    public void StopBgm()
    {
        if (!isInited)
        {
            return;
        }
        MainBgAudio.Stop();
        MainBgAudio.bgName = "";
    }
    private void SetBg(bgm tabOne, AudioData data)
    {
        if (tabOne.path == data.bgName)
        {
            return;
        }
        data.isLoop = tabOne.isLoop == 1;
        data.volume = tabOne.volume;
        data.pitch = tabOne.pitch;
        data.bgName = tabOne.path;
        AssetLoadOld.Instance.LoadBgAudio(tabOne.path, (clip) =>
        {
            data.clip = clip;
            data.Play();
        });
    }
    private void SetSound(sound tabOne, AudioData data)
    {
        data.isLoop = tabOne.isLoop == 1;
        data.volume = tabOne.volume;
        data.pitch = tabOne.pitch;
        AssetLoadOld.Instance.LoadAudio(tabOne.path, (clip) =>
        {
            data.clip = clip;
            data.Play();
        });
    }
    public new void Clear()
    {
        MainBgAudio = null;
        OtherBgAudio = null;
        SoundAudios = null;
        var components = GetComponents<AudioSource>();
        for (int i = 0; i < components.Length; i++)
        {
            DestroyImmediate(components[i]);
        }
        base.Clear();
    }
}