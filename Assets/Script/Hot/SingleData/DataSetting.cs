

public class DataSetting : Single<DataSetting>
{
    [System.Serializable]
    private class LocalDataSetting : ReadAndSaveTool<LocalDataSetting>
    {
        public float music = 1;
        public float sound = 1;
        public int shake = 1;
        public int[] pushInfos = new[] {1,1,1,1,1,1};
        public void Save() { WriteInPhone(this,"setting"); }
        public LocalDataSetting Read() { return ReadByPhone("setting", () => { return new LocalDataSetting(); }); }
    }

    private LocalDataSetting _setting = null;
    public DataSetting()
    {
        _setting = new LocalDataSetting();
        _setting = _setting.Read();
    }

    public void Init()
    {
        MusicMgr.Instance.SetBgVolume(_setting.music);
        MusicMgr.Instance.SetSoundVolume(_setting.sound);
    }

    public float GetMusicVal() { return _setting.music; }
    public float GetSoundVal() { return _setting.sound; }
    public int GetShakeVal() { return _setting.shake; }
    public int[] GetPushInfos() { return _setting.pushInfos; }
    

    public void SetMusic()
    {
        _setting.music = _setting.music == 1 ? 0 : 1;
        _setting.Save();
        MusicMgr.Instance.SetBgVolume(_setting.music);
    }
    
    public void SetSound()
    {
        _setting.sound = _setting.sound == 1 ? 0 : 1;
        _setting.Save();
        MusicMgr.Instance.SetSoundVolume(_setting.sound);
    }
    
    public void SetShake()
    {
        _setting.shake = _setting.shake == 1 ? 0 : 1;
        _setting.Save();
    }
    public bool Shake => _setting.shake == 1;

    public void SetPushInfo(int index)
    {
        _setting.pushInfos[index] = _setting.pushInfos[index] == 0 ? 1 : 0;
        _setting.Save();
    }
}