using Table;
using System.Collections.Generic;
public class FrameTableCache : Single<FrameTableCache>
{
    private Dictionary<int, basecolor> _basecolorTable;
    private Dictionary<int, color_style> _color_styleTable;
    private Dictionary<int, channel> _channelTable;
    private Dictionary<int, sub_channel> _sub_channelTable;
    private Dictionary<int, sound> _soundTable;
    private Dictionary<int, bgm> _bgmTable;
    private Dictionary<int, frame> _frameTable;
    public Dictionary<int, basecolor> basecolorTable
    {
        get
        {
            if (_basecolorTable == null || _basecolorTable.Count == 0)
                _basecolorTable = TableRead.Instance.ReadTable<basecolor>("basecolor");
            return _basecolorTable;
        }
    }
    public Dictionary<int, color_style> color_styleTable
    {
        get
        {
            if (_color_styleTable == null || _color_styleTable.Count == 0)
                _color_styleTable = TableRead.Instance.ReadTable<color_style>("color_style");
            return _color_styleTable;
        }
    }
    public Dictionary<int, channel> channelTable
    {
        get
        {
            if (_channelTable == null || _channelTable.Count == 0)
                _channelTable = TableRead.Instance.ReadTable<channel>("channel");
            return _channelTable;
        }
    }
    public Dictionary<int, sub_channel> sub_channelTable
    {
        get
        {
            if (_sub_channelTable == null || _sub_channelTable.Count == 0)
                _sub_channelTable = TableRead.Instance.ReadTable<sub_channel>("sub_channel");
            return _sub_channelTable;
        }
    }
    public Dictionary<int, sound> soundTable
    {
        get
        {
            if (_soundTable == null || _soundTable.Count == 0)
                _soundTable = TableRead.Instance.ReadTable<sound>("sound");
            return _soundTable;
        }
    }
    public Dictionary<int, bgm> bgmTable
    {
        get
        {
            if (_bgmTable == null || _bgmTable.Count == 0)
                _bgmTable = TableRead.Instance.ReadTable<bgm>("bgm");
            return _bgmTable;
        }
    }
    public Dictionary<int, frame> frameTable
    {
        get
        {
            if (_frameTable == null || _frameTable.Count == 0)
                _frameTable = TableRead.Instance.ReadTable<frame>("frame");
            return _frameTable;
        }
    }
}
