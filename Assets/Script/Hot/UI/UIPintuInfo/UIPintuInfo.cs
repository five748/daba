using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using Unity.VisualScripting;
using UnityEngine.UI;
using YuZiSdk;

public class UIPintuInfo:BaseMonoBehaviour{
    public UIPintuInfoAuto Auto = new UIPintuInfoAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIPintuInfo");
    }
    public void ClickBtn_sure(GameObject button){
        Debug.Log("click" + button.name);
        if (gridItems.Count <= 0)
        {
            Msg.Instance.Show("还未进行摆放物资！");
            return;
        }
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumLoadUpNum);
        UIManager.OpenTipNoText("UIPintuResult",config.id + "-" + gridItems.Count);
    }
    public void ClickBtn_ad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(7, b =>
        {
            if (!b)
            {
                return;
            }
            for (int i = 0; i < Auto.Block.childCount; i++)
            {
                var index = i;
                Auto.Block.GetChild(i).GetComponent<Image>().DOFade(0, 0.2f).SetLoops(4, LoopType.Yoyo).OnComplete(() =>
                {
                    if (index == Auto.Block.childCount - 1)
                    {
                        blocks.Clear();
                        UpdateBlock();
                    }
                });
            }
            
          
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public static UIPintuInfo ins;
    private void Init(string param){
        UIManager.FadeOut();
        ins = this;
        int id = param.ToInt();
        config = TableCache.Instance.loadingOfCargoTable[id];
        gridItems.Clear();
        bags.Clear();
        blocks.Clear();
        bagItems.Clear();
        for (int i = 0; i < config.barrierPos.Length; i++)
        {
            blocks.Add(config.barrierPos[i]);            
        }
        for (int i = 0; i < config.itemId.Length; i++)
        {
            bags.Add(config.itemId[i]);            
        }
        UpdateTop();
        UpdateBlock();
        InitBag();
        
        GuideMgr.Instance.SetHideBtnList(new List<Transform>(){Auto.Btnclose.transform});
        GuideMgr.Instance.SetSlipBtn(Auto.Bag.GetChild(0), Auto.Guide_target);
    }
    public static int size = 146;
    public static int spac = 50;
    private loadingOfCargo config;
    private List<int[]> blocks = new List<int[]>();
    private List<int> bags = new List<int>();
    private Dictionary<int,float> itemSize = new Dictionary<int, float>();
    private List<UIPinTuItem> gridItems = new List<UIPinTuItem>();
    private List<UIPinTuItem> bagItems = new List<UIPinTuItem>();
    
    private void UpdateTop()
    {
        Auto.Reward1.text = $"每装卸一件货物奖励:<color=#24d39f>  1</color>";
        // Auto.Reward2.text = $"每装卸一件货物奖励:<color=#24d39f>  {TableCache.Instance.configTable[201].param}</color>";
        Auto.Reward2.text = $"全部摆放额外奖励:  <color=#24d39f>  {config.reward}</color>";
    }

    public void Success()
    {
        if (bagItems.Count > 0)
        {
            return;
        }
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumLoadUpNum);
        UIManager.OpenTipNoText("UIPintuResult",config.id + "-" + gridItems.Count);
    }

    private void UpdateBlock()
    {
        var items = Auto.Block.AddChilds(blocks.Count);
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var p = blocks[i];
            item.localPosition = new Vector3(p[0] * size,p[1] * size,0);
        }
    }

    private void InitBag()
    {
        var items = Auto.Bag.AddChilds(bags.Count);
        float x = 0;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var move = item.GetComponent<UIPinTuItem>();
            move.Init(bags[i]);
            var width = move.Width();
            itemSize[bags[i]] = width;
            item.localPosition = new Vector3(x + width / 2,0,0);
            x += width;
            x += spac;
            move.SetBag();
            bagItems.Add(move);
        }
        Auto.Bag.GetComponent<RectTransform>().sizeDelta = new Vector2(x + 100,300);
    }
    
   

    private void UpdateBag()
    {
        var posList = new List<float>();
        float x = 0;
        for (int i = 0; i < bagItems.Count; i++)
        {
            var w = bagItems[i].Width();
            posList.Add( x + w / 2);
            x += w + spac;
        }
        Auto.Bag.GetComponent<RectTransform>().sizeDelta = new Vector2(x + 100,300);
        for (int i = 0; i < bagItems.Count; i++)
        {
            bagItems[i].transform.DOLocalMove(new Vector3(posList[i], 0,0), 0.2f);
        }
        
    }

    
    //是否有障碍
    private bool CheckIsBlock(int x,int y)
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i][0] == x && blocks[i][1] == y)
            {
                return true;
            }            
        }
        return false;
    }

    private bool CheckRound(List<UIPinTuItem> res,float x, float y)
    {
        if (!CheckInCenter(x, y))
        {
            return false;
        }

        int x2 = (int) x;
        int y2 = (int) y;
        for (int i = 0; i < gridItems.Count; i++)
        {
            if (!res.Contains(gridItems[i]) && gridItems[i].Contanins(x2, y2))
            {
                res.Add(gridItems[i]);
            }
        }
        return true;
    }

    private bool CheckInCenter(float x, float y)
    {
        float x1 = x - (int) x;
        float y1 = y - (int) y;

        if (x1 > 0.9f || x1 < 0.1f || y1 > 0.9f || y1 < 0.1f)
        {
            
            return false;
        }

        return true;

    }
    
    private bool isMove = false;
    private UIPinTuItem moveItem;
    private UIPinTuItem beMoveItem;
    private Vector3 endMovePos;//移动到的位置
    
    
 
    public bool CheckEndPos()
    {
        if (moveItem == null)
        {
            return false;
        }

        var pos = moveItem.Round();
        List<UIPinTuItem> repeat = new List<UIPinTuItem>();
        for (int i = 0; i < pos.Count; i++)
        {
            var p = pos[i];
            float x = p.x / size;
            float y = p.y / size;
            if (x < 0 || y < 0 || x > 7 || y > 5)
            {
                return false;
            }
            if (CheckIsBlock((int) x, (int)y))
            {
                return false;
            }

            if (!CheckRound(repeat, x, y))
            {
                return false;
            }
        }

        if (moveItem.type == 1)
        {
            bagItems.Remove(beMoveItem);
            GameObject.Destroy(beMoveItem.gameObject);
            gridItems.Add(moveItem);
            moveItem.SetGrid(pos);
        }
        else
        {
            gridItems.Add(moveItem);
            moveItem.SetGrid(pos);
        }
        
        beMoveItem = null;
        moveItem = null;
        isMove = false;
        // Msg.Instance.Show("摆放成功");
        for (int i = 0; i < repeat.Count; i++)
        {
            GridToBag(repeat[i],false);
        }

        UpdateBag();
        Auto.List.GetComponent<ScrollRect>().horizontal = true;
        Success();
        if (GuideMgr.Instance.CheckInGuide())
        {
            GuideMgr.Instance.RecoverySpecialEvent();
            GuideMgr.Instance.SetSlipBtn(null, null);
            GuideMgr.Instance.SetHideBtnList(new List<Transform>());
            GuideMgr.Instance.CompleteCurGuide();
        }
        return true;
    }



    private int GetListIndex()
    {
        var x = Mathf.Abs(Auto.Bag.localPosition.x);
        if (bagItems.Count <= 0)
        {
            return 0;
        }
        if (x < bagItems[0].Width())
        {
            return 1;
        }
        for (int i = 0; i < bagItems.Count; i++)
        {
            if (bagItems[i].transform.position.x > Auto.View.position.x)
            {
                return Mathf.Min(i + 1,bagItems.Count - 1);
            }
        }
        return 0;
    }

    
    public void OnMoveItem(UIPinTuItem item,int itemId)
    {
        Debug.Log("....长按...");
        if (isMove) return;
        Auto.List.GetComponent<ScrollRect>().horizontal = false;
        isMove = true;

        if (item.type == 1)
        {
            moveItem = GetMoveItem();
            moveItem.Init(itemId);
            moveItem.transform.position = item.transform.position;
            moveItem.gameObject.SetActive(true);
            beMoveItem = item;
            item.gameObject.SetActive(false);
        }
        else
        {
            gridItems.Remove(item);
            item.transform.SetAsLastSibling();
            moveItem = item;
            beMoveItem = null;
        }
        
    }

    public void GridToBag(UIPinTuItem item,bool isUpdate)
    {
        if (item.type == 1)
        {
            return;
        }

        var pos = item.transform.position;
        var index = GetListIndex();
        bagItems.Insert(index,item);
        gridItems.Remove(item);
        item.SetBag();
        item.transform.SetParent(Auto.Bag);
        item.transform.position = pos;
        if (isUpdate)
        {
            UpdateBag();
        }
    }

    private bool isAction = false;
    public override void Update()
    {
        if (isAction || !isMove || moveItem == null)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            endMovePos = new Vector3(pos.x,pos.y,0);
            moveItem.transform.position = endMovePos;
            UpdateLight();

        }
        else
        {
            if (!CheckEndPos())
            {
                if (moveItem.type == 1)
                {
                    isAction = true;
                    moveItem.transform.DOMove(beMoveItem.transform.position, 0.1f).OnComplete(() =>
                    {
                        Destroy(moveItem.gameObject);
                        isMove = false;
                        isAction = false;
                        moveItem = null;
                        beMoveItem.gameObject.SetActive(true);
                        Auto.List.GetComponent<ScrollRect>().horizontal = true;
                    });
                }
                else
                {
                    GridToBag(moveItem,true);
                    moveItem = null;
                    Auto.List.GetComponent<ScrollRect>().horizontal = true;
                    isMove = false;
                }
            }
            UpdateLight();

        }
    }
    
       
    private void UpdateLight()
    {
        Auto.Light.gameObject.SetActive(moveItem != null);
        if (moveItem == null)
            return;
        
        var pos = moveItem.Round();
        var items = Auto.Light.AddChilds(pos.Count);
        for (int i = 0; i < items.Length; i++)
        {
            var p = pos[i];
            float x = (p.x / size);
            float y = (p.y / size);
            if (x < 0 || y < 0 || x > 7 || y > 5)
            {
                items[i].gameObject.SetActive(false);
                Auto.Light.gameObject.SetActive(false);
                return;
            }else if (CheckIsBlock((int)x,(int)y))
            {
                items[i].gameObject.SetActive(false);
                Auto.Light.gameObject.SetActive(false);
                return;
            }else if (!CheckInCenter(x,y))
            {
                Auto.Light.gameObject.SetActive(false);
                return;
            }
            else
            {
                items[i].gameObject.SetActive(true);
                items[i].localPosition = new Vector3((int)x * size,(int)y * (size + 0.8f),0);
            }
        }
    }

    
    private UIPinTuItem GetMoveItem()
    {
        var obj = GameObject.Instantiate(Auto.Grid.GetChild(0));
        var item = obj.transform.GetComponent<UIPinTuItem>();
        item.SetBag();
        item.transform.SetParent(Auto.Grid);
        return item;
    }

    public override void Destory()
    {
        base.Destory();
        for (int i = 0; i < Auto.Block.childCount; i++)
        {
            DOTween.Kill(Auto.Block.GetChild(i).GetComponent<Image>(), false);
        }
    }
}







