using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EmojiText : Text 
{
	private const float ICON_SCALE_OF_DOUBLE_SYMBOLE = 0.95f;
    public override float preferredWidth => cachedTextGeneratorForLayout.GetPreferredWidth(emojiText, GetGenerationSettings(rectTransform.rect.size)) / pixelsPerUnit;
	public override float preferredHeight => cachedTextGeneratorForLayout.GetPreferredHeight(emojiText, GetGenerationSettings(rectTransform.rect.size)) / pixelsPerUnit;
	
	private string emojiText {
		get
		{
			var _emojiTextCache = "";
			var emojiTextIndex = 0;
			MatchCollection matches = Regex.Matches(text, "【[a-z0-9A-Z]+】");
			for (int i = 0; i < matches.Count; i++)
			{
				EmojiInfo info;
				var kkk = matches[i].Value;
				if (kkk.Length > 3)
				{
					var numStr = kkk.Substring(1, kkk.Length - 2);
					if (Regex.IsMatch(numStr, @"^\d+$"))
					{
						int _num;
                        try
						{
							_num = int.Parse(numStr);
                        }
                        catch
                        {
							_num = -1;
						}
						if (_num < 36)
							kkk = "【" + (char)('a' + _num - 10) + "】";
						else if (_num < 62)
							kkk = "【" + (char)('A' + _num - 36) + "】";
					}
				}
				if (m_EmojiIndexDict.TryGetValue(kkk, out info))
				{
					_emojiTextCache += text.Substring(emojiTextIndex, matches[i].Index - emojiTextIndex) + "😀😀 ";
					emojiTextIndex = matches[i].Index + matches[i].Length;
				}
				else
				{
					_emojiTextCache += text.Substring(emojiTextIndex, matches[i].Index - emojiTextIndex + matches[i].Length);
					emojiTextIndex = matches[i].Index + matches[i].Length;
				}
			}
			_emojiTextCache += text.Substring(emojiTextIndex, text.Length - emojiTextIndex);
			return _emojiTextCache;
		}
	} //=> Regex.Replace(text, "【[a-z0-9A-Z]+】", "😀😀 ");
	private static Dictionary<string,EmojiInfo> m_EmojiIndexDict = null;

	protected override void Awake()
	{
		base.Awake();
		m_EmojiIndexDict = new Dictionary<string, EmojiInfo>();
		//load emoji data, and you can overwrite this segment code base on your project.
		TextAsset emojiContent = Resources.Load<TextAsset>("other/emoji");
		string[] lines = emojiContent.text.Split('\n');
		for (int i = 1; i < lines.Length; i++)
		{
			if (!string.IsNullOrEmpty(lines[i]))
			{
				string[] strs = lines[i].Split('\t');
				EmojiInfo info;
				info.x = float.Parse(strs[3]);
				info.y = float.Parse(strs[4]);
				info.size_w = float.Parse(strs[5]);
				info.size_h = float.Parse(strs[6]);
				var key = Regex.Replace(strs[1], "(\\[)([a-z0-9A-Z]+)(\\])", "【$2】");
				m_EmojiIndexDict.Add(key, info);
			}
		}
	}

	struct EmojiInfo
	{
		public float x;
		public float y;
		public float size_w;
		public float size_h;
	}

	readonly UIVertex[] m_TempVerts = new UIVertex[4];

    //public void Init(InputField inputfield)
    //{
    //    inputField = inputfield;
    //    inputField.onEndEdit.AddListener((str) =>
    //    {
    //        caretPosition = inputField.caretPosition;
    //    });
    //}

    protected override void OnPopulateMesh(VertexHelper toFill)
	{
		if (font == null)
        {
            return;
        }

		Dictionary<int,EmojiInfo> emojiDic = new Dictionary<int, EmojiInfo> ();
		if (supportRichText)
        {
			//int nParcedCount = 0;
			//[1] [123] 替换成#的下标偏移量			
			//int nOffset = 0;
			MatchCollection matches = Regex.Matches(text, "【[a-z0-9A-Z]+】");
			for (int i = 0; i < matches.Count; i++)
            {
				EmojiInfo info;
				var kkk = matches[i].Value;
                if (kkk.Length > 3)
                {
					var numStr = kkk.Substring(1, kkk.Length - 2);
					if (Regex.IsMatch(numStr, @"^\d+$"))
                    {
						var _num = int.Parse(numStr);
						if (_num < 36)
							kkk = "【" + (char)('a' + _num - 10) + "】";
						else if (_num < 62)
							kkk = "【" + (char)('A' + _num - 36) + "】";
					}
                }
                if (m_EmojiIndexDict.TryGetValue(kkk, out info))
                {
                    emojiDic.Add(matches[i].Index /*- nOffset + nParcedCount*/, info);
					//nOffset += matches[i].Length - 1;
					//nParcedCount++;
                }
			}
		}

		// We don't care if we the font Texture changes while we are doing our Update.
		// The end result of cachedTextGenerator will be valid for this instance.
		// Otherwise we can get issues like Case 619238.
		m_DisableFontTextureRebuiltCallback = true;

		Vector2 extents = rectTransform.rect.size;

		var settings = GetGenerationSettings(extents);
		cachedTextGenerator.Populate(emojiText, settings);		

		Rect inputRect = rectTransform.rect;

		// get the text alignment anchor point for the text in local space
		Vector2 textAnchorPivot = GetTextAnchorPivot(alignment);
		Vector2 refPoint = Vector2.zero;
		refPoint.x = Mathf.Lerp(inputRect.xMin, inputRect.xMax, textAnchorPivot.x);
		refPoint.y = Mathf.Lerp(inputRect.yMin, inputRect.yMax, textAnchorPivot.y);

        // Apply the offset to the vertices
        IList<UIVertex> verts = cachedTextGenerator.verts;
		float unitsPerPixel = 1 / pixelsPerUnit;
		int vertCount = verts.Count;

        // We have no verts to process just return (case 1037923)
        if (vertCount <= 0)
        {
            toFill.Clear();
            return;
        }

        Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
        roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
        toFill.Clear();
		if (roundingOffset != Vector2.zero)
		{
		    for (int i = 0; i < vertCount; ++i)
		    {
		        int tempVertsIndex = i & 3;
		        m_TempVerts[tempVertsIndex] = verts[i];
		        m_TempVerts[tempVertsIndex].position *= unitsPerPixel;
		        m_TempVerts[tempVertsIndex].position.x += roundingOffset.x;
		        m_TempVerts[tempVertsIndex].position.y += roundingOffset.y;
		        if (tempVertsIndex == 3)
                {
                    toFill.AddUIVertexQuad(m_TempVerts);
                }
            }
		}
		else
		{
			for (int i = 0; i < vertCount; ++i)
            {
				EmojiInfo info;
				int index = i / 4;
				if (emojiDic.TryGetValue (index, out info))
                {
                    //compute the distance of '[' and get the distance of emoji 
                    //计算替换符的距离
                    //TODO 临时代码
                    if (verts.Count <= i + 9)
                    {
						Debug.LogError("出现异常表情");
						return;
                    }
					float emojiSize = (verts[i + 1].position.x - verts[i].position.x +
						verts[i + 5].position.x - verts[i + 4].position.x +
						verts[i + 9].position.x - verts[i + 8].position.x)
						* ICON_SCALE_OF_DOUBLE_SYMBOLE;

					float fCharHeight = verts[i + 1].position.y - verts[i + 2].position.y;
                    float fCharWidth = verts[i + 1].position.x - verts[i].position.x;

                    float fHeightOffsetHalf = (emojiSize - fCharHeight) * 0.5f;
					float fStartOffset = emojiSize * (1 - ICON_SCALE_OF_DOUBLE_SYMBOLE) / 2;

                    m_TempVerts [3] = verts [i];//1
					m_TempVerts [2] = verts [i + 1];//2
					m_TempVerts [1] = verts [i + 2];//3
					m_TempVerts [0] = verts [i + 3];//4

                    m_TempVerts[0].position += new Vector3(fStartOffset, -fHeightOffsetHalf, 0)+new Vector3(0,10,0);
                    m_TempVerts[1].position += new Vector3(fStartOffset - fCharWidth + emojiSize, -fHeightOffsetHalf, 0) + new Vector3(0, 10, 0);
					m_TempVerts[2].position += new Vector3(fStartOffset - fCharWidth + emojiSize, fHeightOffsetHalf, 0) + new Vector3(0, 10, 0);
					m_TempVerts[3].position += new Vector3(fStartOffset, fHeightOffsetHalf, 0) + new Vector3(0, 10, 0);

					m_TempVerts [0].position *= unitsPerPixel;
					m_TempVerts [1].position *= unitsPerPixel;
					m_TempVerts [2].position *= unitsPerPixel;
					m_TempVerts [3].position *= unitsPerPixel;

					float pixelOffset_w = info.size_w / 32 / 2;
					float pixelOffset_h = info.size_h / 32 / 2;
					m_TempVerts[0].uv1 = new Vector2(info.x + pixelOffset_w, info.y + pixelOffset_h);
					m_TempVerts[1].uv1 = new Vector2(info.x - pixelOffset_w + info.size_w, info.y + pixelOffset_h);
					m_TempVerts[2].uv1 = new Vector2(info.x - pixelOffset_w + info.size_w, info.y - pixelOffset_h + info.size_h);
					m_TempVerts[3].uv1 = new Vector2(info.x + pixelOffset_w, info.y - pixelOffset_h + info.size_h);

					toFill.AddUIVertexQuad (m_TempVerts);

                    i += 4 * 3 - 1;
                }
                else
                {					
					int tempVertsIndex = i & 3;
					m_TempVerts [tempVertsIndex] = verts [i];
					m_TempVerts [tempVertsIndex].position *= unitsPerPixel;
					if (tempVertsIndex == 3)
                    {
                        toFill.AddUIVertexQuad(m_TempVerts);
                    }
                }
			}

		}
		m_DisableFontTextureRebuiltCallback = false;
	}
}
