using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Transform Content;//对话的内容 
    public Transform Continue;//点击继续
    public Transform NpcLeft;
    public Transform NpcRight;

    public EventTriggerListener BtnSkip;//跳过按钮
    
    [Header("文本输出的速度")]
    public float TextSpeed = 0.1f;
    
    private bool _textFinished = false;//是否完成当前文字的输出
    private bool _cancelTyping = false;//是否取消打字机效果
    
    public YieldResult<string> result;
    
    private int _index;//当前对话的id
    private Dictionary<int, GuideDialogue> _dicDialogItems;//对话配置列表
    private GuideDialogue _item;//当前对话配置

    private Transform tsf_btn;
    private GuideHollowBase guide_hollow;

    private void Start()
    {
        BtnSkip.onClick = OnSkipClick;
    }

    public YieldResult<string> ShowDialogue(int start_index)
    {
        this.tsf_btn = tsf_btn;
        this.guide_hollow = guide_hollow;
        result = new YieldResult<string>();
        Content.SetText("");
        Continue.gameObject.SetActive(false);
        NpcLeft.gameObject.SetActive(false);
        NpcRight.gameObject.SetActive(false);
        
        gameObject.SetActive(true);
        //初始化对话数据
        _index = start_index;
        _dicDialogItems = new Dictionary<int, GuideDialogue>();
        int index = start_index;
        GuideDialogue data;
        while (index != -1)
        {
            data = TableCache.Instance.GuideDialogueTable[index];
            _dicDialogItems.Add(index, data);
            index = data.next;
        }
        
        //开始显示对话内容
        _cancelTyping = false;
        BtnSkip.gameObject.SetActive(true);
        StartCoroutine(UpdateContext());
        
        return result;
    }
    
    IEnumerator UpdateContext()
    {
        Continue.gameObject.SetActive(false);
        _dicDialogItems.TryGetValue(_index, out _item);
        _textFinished = false;
        Content.SetText("");
        
        //显示npc形象
        ShowNpc();

        int letter = 0;
        string begin = "";
        string end = "";
        while(!_cancelTyping && letter < _item.content.Length - 1)
        {
            if (_item.content[letter] == '<' && _item.content.Substring(letter, 8).Equals("<color=#"))
            {
                begin += _item.content.Substring(letter, 15);
                letter += 15;
                end = "</color>";
            }
            else if (_item.content[letter] == '<' && _item.content.Substring(letter, 8).Equals("</color>"))
            {
                begin += "</color>";
                letter += 8;
                end = "";
            }
            begin += _item.content[letter];
            // Debug.LogError($"文本的内容是 begin:{begin} end:{end}");
            Content.SetText(begin + end);
            letter++;

            yield return new WaitForSeconds(TextSpeed);
        }

        // if (_cancelTyping)
        // {
        //     Debug.Log($"跳过对话 =========== ");
        // }
        Content.SetText(_item.content);
        _cancelTyping = false;
        _textFinished = true;

        _index = (int)_item.next;
        Continue.gameObject.SetActive(true);
    }

    private int BracketsOffset(string content, int index)
    {
        if (content[index].ToString() != "<")
        {
            return 0;
        }

        int back_index = content.IndexOf(">", index);

        if (back_index >= content.Length - 1)
        {
            return content.Length - 1;
        }
        return back_index > BracketsOffset(content, back_index + 1) ? back_index : BracketsOffset(content, back_index + 1);
    }

    private void ShowNpc()
    {
        NpcLeft.gameObject.SetActive(_item.npc_pos == 1);
        NpcRight.gameObject.SetActive(_item.npc_pos == 2);
        // if (_item.npc_pos == 1)
        // {
        //     NpcLeft.transform.SetImage(_item.npc_img_path);
        //     var rect = NpcLeft.GetComponent<RectTransform>();
        //     rect.localScale = new Vector3(_item.flip, rect.localScale.y, rect.localScale.z);
        // }
        // else
        // {
        //     NpcRight.transform.SetImage(_item.npc_img_path);
        //     var rect = NpcRight.GetComponent<RectTransform>();
        //     rect.localScale = new Vector3(_item.flip, rect.localScale.y, rect.localScale.z);
        // }
    }

    private void OnSkipClick(GameObject go)
    {
        if (!gameObject || !gameObject.activeSelf)
        {
            return;
        }
        if (_textFinished)
        {
            if (_index != -1)
            {
                //进行下一段的对话
                StartCoroutine(UpdateContext());
            }
            else
            {
                Debug.Log($"当前这段对话结束了");
                Content.SetText("");
                Continue.gameObject.SetActive(false);
                result.Done();
                gameObject.SetActive(false);
            }            
        }
        else
        {
            // Debug.Log($"点击了跳过对话 =========== ");
            _cancelTyping = true;
        }
    }

    public void EndDialogue()
    {
        if (!gameObject || !gameObject.activeSelf)
        {
            return;
        }

        _cancelTyping = true;
        result.Done();
        gameObject.SetActive(false);
    }
    
    private void try_show_hollow()
    {
        if (tsf_btn == null)
        {
            return;
        }
        
        
    }
}