using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SelfComponent
{
    public enum EnumListViewDirection
    {
        Horizontal,
        Vertical,
    }

    /// <summary>
    /// 循环复用列表
    /// </summary>
    [RequireComponent(typeof(ScrollRect)), ExecuteAlways, DisallowMultipleComponent]
    public class ListView : MonoBehaviour
    {
        [Header("用于布局的子物体")]
        public GameObject cellGameObject;
        [Header("布局的方向 所有方向下content的pivot都会置为(0,1) 且按照左上角为原点来进行对齐")]
        public EnumListViewDirection listViewDirection;
        [Header("content和scroll的间距")]
        public RectOffset padding;
        [Header("子物体之间的间距")]
        public float spacing;
        [Header("多行或多列的时候填写 不能少于1")]
        public int multiple = 1;
        [Header("是否启用分帧加载")]
        public bool frameLoad = false;
        [Header("分帧加载时每帧加载的数量")]
        public int perFrameCount = 0;
        [Header("左右滑动箭头")]
        public Transform leftArrow;
        public Transform rightArrow;
        [Header("item的尺寸是否会发生变化")] 
        public bool changeItemSize;
        
        [Header("布局测试的时候使用, 测试时不能启用分帧加载")]
        public int testCount = 2;

        protected ScrollRect Scroll;
        protected RectTransform RectScroll;
        protected RectTransform RectContent;
        protected float ScrollHeight;
        protected float ScrollWidth;
        protected float CellObjectHeight;
        protected float CellObjectWidth;
        protected float ContentHeight;
        protected float ContentWidth;
        protected int MaxIndex = -1;
        protected int MinIndex = -1;
        protected int MaxCount = 0;
        protected bool IsClearList = false;
        //记录 物体的坐标 和 物体 
        protected class CellInfo
        {
            public Vector3 pos;
            public GameObject obj;
            public float width = 0;
            public float height = 0;
        };
        protected CellInfo[] CellInfos;

        private bool _bInitial = false;
        private bool _bInitialItem = false;
        private Action<int, GameObject> _funUpdateItemUI;
        private GameObjectPool _pool;
        private float _movePos;
        
        private GameObject _cell_object;
        
        //pool
        class GameObjectPool
        {
            private GameObject _one;
            private Stack<GameObject> _pool;

            public GameObjectPool(GameObject go)
            {
                if (go == null)
                {
                    Debug.LogError("节点池传入物体为null");
                }
                _one = go;
                _pool = new Stack<GameObject>();
            }
        
            public GameObject getOne(Transform parent) {
                GameObject cell = null;
                if (_pool.Count > 0)
                {
                    cell = _pool.Pop();
                }

                if (cell == null)
                {
                    cell = GameObject.Instantiate(_one);
                }
                cell.transform.SetParent(parent);
                cell.transform.localScale = Vector3.one;
                cell.SetActive(true);

                return cell;
            }
        
            public void recOne(GameObject cell) {
                if (cell != null)
                {
                    _pool.Push(cell);
                    cell.SetActive(false);
                }
            }
        }

        public virtual void Init(Action<int, GameObject> fun_update_item_ui)
        {
            _funUpdateItemUI = fun_update_item_ui;

            if (_bInitial)
            {
                return;
            }
            
            if (cellGameObject == null)
            {
                cellGameObject = RectContent.GetChild(0).gameObject;
            }
            
            _cell_object = Instantiate(cellGameObject, transform);
            _cell_object.gameObject.SetActive(false);
            _pool = new GameObjectPool(_cell_object);
            RectTransform rect_cell = cellGameObject.GetComponent<RectTransform>();
            _pool.recOne(cellGameObject);
            CellObjectHeight = rect_cell.rect.height;
            CellObjectWidth = rect_cell.rect.width;
            rect_cell.anchorMin = new Vector2(0, 1);
            rect_cell.anchorMax = new Vector2(0, 1);
            
            RectScroll = gameObject.GetComponent<RectTransform>();
            ScrollHeight = RectScroll.rect.height;
            ScrollWidth = RectScroll.rect.width;

            Scroll = GetComponent<ScrollRect>();
            RectContent = Scroll.content;
            ContentHeight = RectContent.rect.height;
            ContentWidth = RectContent.rect.width;
            
            //reset content position, insure the pivot of content is (0,1)
            RectContent.pivot = new Vector2(0, 1);
            RectContent.anchorMin = new Vector2(0, 1);
            RectContent.anchorMax = new Vector2(0, 1);
            RectContent.anchoredPosition = new Vector2(0, 0);
            
            Scroll.onValueChanged.RemoveAllListeners();
            Scroll.onValueChanged.AddListener(value=>
            {
                scrollRectListener(value);
            });
            if (listViewDirection == EnumListViewDirection.Horizontal)
            {
                Scroll.horizontal = true;
                Scroll.vertical = false;
            }
            else
            {
                Scroll.horizontal = false;
                Scroll.vertical = true;
            }
            init_button();
            _bInitial = true;
        }

        private void init_button()
        {
            if (leftArrow == null || rightArrow == null)
            {
                return;
            }

            var com_left = leftArrow.GetComponent<Button>();
            if (com_left == null)
            {
                com_left = leftArrow.gameObject.AddComponent<Button>();
            }

            var com_right = rightArrow.GetComponent<Button>();
            if (com_right == null)
            {
                com_right = rightArrow.gameObject.AddComponent<Button>();
            }
            
            com_left.onClick.AddListener(() =>
            {
                if (MinIndex == 0 || !_bInitialItem)
                {
                    return;
                }

                var pos = RectContent.anchoredPosition;
                var cell_info = CellInfos[MinIndex - 1];
                RectTransform rect_cell;
                if (cell_info.obj != null)
                {
                    rect_cell = cell_info.obj.GetComponent<RectTransform>();
                }
                else
                {
                    rect_cell = cellGameObject.GetComponent<RectTransform>();
                }
                float pivot_x = rect_cell.pivot.x * rect_cell.sizeDelta.x;
                float pivot_y = (rect_cell.pivot.y - 1) * rect_cell.sizeDelta.y;
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    RectContent.anchoredPosition = new Vector2(pos.x, -cell_info.pos.y + pivot_y);
                }
                else
                {
                    RectContent.anchoredPosition = new Vector2(-cell_info.pos.x + pivot_x, pos.y);
                }
            });
            
            com_right.onClick.AddListener(() =>
            {
                if (MaxIndex == CellInfos.Length - 1 || !_bInitialItem)
                {
                    return;
                }

                var pos = RectContent.anchoredPosition;
                var cell_info = CellInfos[MinIndex + 1];
                RectTransform rect_cell;
                if (cell_info.obj != null)
                {
                    rect_cell = cell_info.obj.GetComponent<RectTransform>();
                }
                else
                {
                    rect_cell = cellGameObject.GetComponent<RectTransform>();
                }
                float pivot_x = rect_cell.pivot.x * rect_cell.sizeDelta.x;
                float pivot_y = (rect_cell.pivot.y - 1) * rect_cell.sizeDelta.y;
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    RectContent.anchoredPosition = new Vector2(pos.x, -cell_info.pos.y + pivot_y);
                }
                else
                {
                    RectContent.anchoredPosition = new Vector2(-cell_info.pos.x + pivot_x, pos.y);
                }
            });
        }

        public virtual void ShowList(int count, bool is_change_name = true)
        {
           MonoTool.Instance.StartCor(show_list(count, is_change_name));
        }
        
        private IEnumerator show_list(int count, bool is_change_name = true)
        {
            if (cellGameObject == null)
            {
                Debug.LogError($"scroll content 中没有子物体 或 脚本上没有挂载");
                yield return null;
            }

            MaxIndex = -1;
            MinIndex = -1;
            int per_frame_count = 0;
            
            //calculate content size
            if (listViewDirection == EnumListViewDirection.Vertical)
            {
                int quotient = Mathf.CeilToInt((float)count / multiple);
                float content_size = CellObjectHeight * quotient + spacing * (quotient - 1) + padding.top + padding.bottom;
                ContentHeight = Mathf.Max(content_size, RectScroll.sizeDelta.y);
                ContentWidth = Mathf.Max(RectScroll.sizeDelta.x, CellObjectWidth + padding.left + padding.right);
            }
            else
            {
                int quotient = Mathf.CeilToInt((float)count / multiple);
                float content_size = CellObjectWidth * quotient + spacing * (quotient - 1) + padding.left + padding.right;
                ContentHeight = Math.Max(RectScroll.sizeDelta.y, CellObjectHeight + padding.top + padding.bottom);
                ContentWidth = Math.Max(content_size, RectScroll.sizeDelta.x);
            }

            if (!_bInitialItem)
            {
                RectContent.sizeDelta = new Vector2(ContentWidth, ContentHeight);
            }

            int last_end_index = 0;
            if (_bInitialItem)
            {
                last_end_index = count - MaxCount > 0 ? MaxCount : count;
                last_end_index = IsClearList ? 0 : last_end_index;
                int total = IsClearList ? CellInfos.Length : MaxCount;
                for (int i = last_end_index; i < total; i++)
                {
                    if (CellInfos[i].obj != null)
                    {
                        _pool.recOne(CellInfos[i].obj);
                        CellInfos[i].obj = null;
                    }
                }
            }

            CellInfo[] tempCellInfos = CellInfos;
            CellInfos = new CellInfo[count];
            for (int i = 0; i < count; i++)
            {
                if (MaxCount != -1 && i < last_end_index)
                {
                    CellInfo tempCellInfo = tempCellInfos[i];
                    tempCellInfo.width = CellObjectWidth;
                    tempCellInfo.height = CellObjectHeight;
                    float edge_pos = listViewDirection == EnumListViewDirection.Vertical ? tempCellInfo.pos.y : tempCellInfo.pos.x;
                    if (!is_out_range(edge_pos, i))
                    {
                        MinIndex = MinIndex == -1 ? i : MinIndex;
                        MaxIndex = i;

                        if (tempCellInfo.obj == null)
                        {
                            tempCellInfo.obj = _pool.getOne(RectContent);
                        }
                        if (is_change_name)
                        {
                            tempCellInfo.obj.name = i.ToString();
                        }
                        tempCellInfo.obj.SetActive(true);
                        CellInfos[i] = tempCellInfo;
                        callUpdateFun(_funUpdateItemUI, tempCellInfo.obj, i);//需要用到cellinfo
                    }
                    else
                    {
                        _pool.recOne(tempCellInfo.obj);
                        tempCellInfo.obj = null;
                        CellInfos[i] = tempCellInfo;
                    }
                    continue;
                }

                CellInfo cell_info = new CellInfo();
                float pos = 0;
                float aixs_pos = 0;

                if (multiple == 1)
                {
                    if (i > 0)
                    {
                        CellInfo previous = CellInfos[i - 1];
                        RectTransform rect_previous;
                        if (previous.obj == null)
                        {
                            rect_previous = cellGameObject.GetComponent<RectTransform>();
                        }
                        else
                        {
                            rect_previous = previous.obj.GetComponent<RectTransform>();
                        }
                        if (listViewDirection == EnumListViewDirection.Vertical)
                        {
                            var distance = rect_previous.sizeDelta.y - (rect_previous.sizeDelta.y - CellObjectHeight) * (1 - rect_previous.pivot.y);
                            cell_info.pos = new Vector3(previous.pos.x, previous.pos.y - spacing - distance);
                        }
                        else
                        {
                            var distance = rect_previous.sizeDelta.x - (rect_previous.sizeDelta.x - CellObjectWidth) * rect_previous.pivot.x;
                            cell_info.pos = new Vector3(previous.pos.x + spacing + distance, previous.pos.y);
                        }
                        // Debug.Log($"{i}的初始位置为 {cell_info.pos.ToString()}");
                    }
                    else
                    {
                        cell_info.pos = init_pos(i);
                    }    
                }
                else
                {
                    cell_info.pos = init_pos(i);
                }

                cell_info.height = CellObjectHeight;
                cell_info.width = CellObjectWidth;
                float cell_pos = listViewDirection == EnumListViewDirection.Vertical ? cell_info.pos.y : cell_info.pos.x;
                if (is_out_range(cell_pos, i))
                {
                    cell_info.obj = null;
                    CellInfos[i] = cell_info;
                    continue;
                }
                
                MinIndex = MinIndex == -1 ? i : MinIndex;
                MaxIndex = i;

                GameObject cell = _pool.getOne(RectContent);
                cell.transform.SetParent(RectContent, true);
                if (is_change_name)
                {
                    cell.gameObject.name = i.ToString();
                }
                cell_info.obj = cell;
                CellInfos[i] = cell_info;
                callUpdateFun(_funUpdateItemUI, cell, i);
                //adjust position
                update_pos(i, true);
                RectTransform rect_cell = cell.GetComponent<RectTransform>();
                rect_cell.localPosition = cell_info.pos;
                per_frame_count++;
                if (frameLoad && per_frame_count >= perFrameCount)
                {
                    yield return new WaitForFixedUpdate();
                }
            }

            MaxCount = count;
            update_content_size();
            update_arrow();
            _bInitialItem = true;
        }

        private Vector2 init_pos(int index)
        {
            float pos = 0;
            float aixs_pos = 0;
            RectTransform rect_cell = cellGameObject.GetComponent<RectTransform>();
            float pivot_x = rect_cell.pivot.x * rect_cell.sizeDelta.x;
            float pivot_y = (rect_cell.pivot.y - 1) * rect_cell.sizeDelta.y;
            
            if (listViewDirection == EnumListViewDirection.Vertical)
            {
                pos = -(CellObjectHeight + spacing) * Mathf.FloorToInt(index / multiple) + pivot_y - padding.top;
                aixs_pos = (CellObjectWidth + spacing) * (index % multiple) + pivot_x + padding.left;
                return new Vector3(aixs_pos, pos, 0);
            }

            pos = (CellObjectWidth + spacing) * Mathf.FloorToInt(index / multiple) + pivot_x + padding.left;
            aixs_pos = -(CellObjectHeight + spacing) * (index % multiple) + pivot_y - padding.top;
            return new Vector3(pos, aixs_pos, 0);
        }

        private bool is_out_range(float pos, int index)
        {
            Vector3 content_pos = RectContent.anchoredPosition;
            RectTransform rect_cell = cellGameObject.GetComponent<RectTransform>();
            var cell_info = CellInfos[index];
            if (cell_info == null)
            {
                float pivot_x = rect_cell.pivot.x * rect_cell.sizeDelta.x;
                float pivot_y = (rect_cell.pivot.y - 1) * rect_cell.sizeDelta.y;
            
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    if (pos + content_pos.y > 0 || pos + content_pos.y - pivot_y < -RectScroll.rect.height)
                    {
                        return true;
                    }
                }
                else
                {
                    if (pos + content_pos.x < 0 || pos + content_pos.x - pivot_x > RectScroll.rect.width)
                    {
                        return true;
                    }
                }
            }
            else
            {
                float width = cell_info.width == 0 ? CellObjectWidth : cell_info.width;
                float height = cell_info.height == 0 ? CellObjectHeight : cell_info.height;
                float pivot_x = rect_cell.pivot.x * width;
                float pivot_y = (rect_cell.pivot.y - 1) * height;
                
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    //上边界
                    if (pos + pivot_y - spacing + content_pos.y > 0)
                    {
                        return true;
                    }
                    //下边界
                    if (pos + height - pivot_y < -content_pos.y - RectScroll.rect.height)
                    {
                        return true;
                    }
                }
                else
                {
                    //左边界
                    if (pos + (width - pivot_x) + spacing + content_pos.x < 0)
                    {
                        return true;
                    }
                    //右边界
                    if (pos + content_pos.x - pivot_x > RectScroll.rect.width)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected void callUpdateFun(Action<int, GameObject> fun, GameObject select_object, int index)
        {
            // int index = int.Parse(select_object.name);
            fun?.Invoke(index, select_object);
            var cell_info = CellInfos[index];
            RectTransform rect_cell = select_object.GetComponent<RectTransform>();
            cell_info.width = rect_cell.sizeDelta.x;
            cell_info.height = rect_cell.sizeDelta.y;
        }

        private void update_pos(int index, bool move_forward)
        {
            if (!changeItemSize)
            {
                return;
            }
            
            var cell_info = CellInfos[index];
            if (cell_info.obj == null)
            {
                Debug.LogError($"cell info hasn't bind object index is {index}");
                return;
            }

            // Debug.Log($"update_pos {index}");

            //invalid modify return
            var rect_cell = cell_info.obj.GetComponent<RectTransform>();
            if (rect_cell.sizeDelta.x == CellObjectWidth && listViewDirection == EnumListViewDirection.Horizontal && !move_forward)
            {
                return;
            }
            if (rect_cell.sizeDelta.y == CellObjectHeight && listViewDirection == EnumListViewDirection.Vertical && !move_forward)
            {
                return;
            }

            int modify_index = Mathf.Min(index + 1, CellInfos.Length - 1);
            var cell_next = CellInfos[modify_index];
            if (cell_next == null)
            {
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    var size = rect_cell.sizeDelta.y - CellObjectHeight;
                    float distance = size * (1 - rect_cell.pivot.y);
                    cell_info.pos = new Vector3(cell_info.pos.x, cell_info.pos.y - distance);
                }
                else
                {
                    var size = rect_cell.sizeDelta.x - CellObjectWidth;
                    float distance = size * rect_cell.pivot.x;
                    cell_info.pos = new Vector3(cell_info.pos.x + distance, cell_info.pos.y);
                }
                return;
            }

            if (!move_forward)
            {
                return;
            }

            var previous_cell = CellInfos[index - 1];
            if (previous_cell.obj == null)
            {
                Debug.Log($"previous_cell.obj == null");
                return;
            }
            var rect_previous = previous_cell.obj.GetComponent<RectTransform>();
            if (listViewDirection == EnumListViewDirection.Vertical)
            {
                var distance_self = (rect_cell.sizeDelta.y - CellObjectHeight) * (1 - rect_cell.pivot.y);
                var distance_previous = rect_previous.sizeDelta.y - (rect_previous.sizeDelta.y - CellObjectHeight) * (1 - rect_previous.pivot.y);
                var pos_y = previous_cell.pos.y - spacing - distance_previous - distance_self;
                if (cell_info.pos.y == pos_y)
                {
                    return;
                }
                var aim_distance = pos_y - cell_info.pos.y;
                cell_info.pos = new Vector3(cell_info.pos.x, pos_y);
                if (aim_distance == 0)
                {
                    return;
                }
                for (int i = index + 1; i < MaxCount; i++)
                {
                    CellInfos[i].pos.y += aim_distance;
                }
                
                var item_pos = Mathf.Abs(pos_y - rect_cell.sizeDelta.y * rect_cell.pivot.y);
                if (item_pos > RectScroll.sizeDelta.y)
                {
                    RectContent.sizeDelta = new Vector2(RectContent.sizeDelta.x, item_pos + padding.bottom);
                    // Debug.Log($"测算高度为{item_pos} 实际高度为{RectContent.sizeDelta.y}");
                }
            }
            else
            {
                var distance_self = (rect_cell.sizeDelta.x - CellObjectWidth) * rect_cell.pivot.x;
                var distance_previous = rect_previous.sizeDelta.x - (rect_previous.sizeDelta.x - CellObjectWidth) * rect_previous.pivot.x;
                var pos_x = previous_cell.pos.x + spacing + distance_previous + distance_self;
                if (cell_info.pos.x == pos_x)
                {
                    return;
                }
                var aim_distance = pos_x - cell_info.pos.x;
                cell_info.pos = new Vector3(pos_x, cell_info.pos.y);
                if (aim_distance == 0)
                {
                    return;
                }
                for (int i = index + 1; i < MaxCount; i++)
                {
                    CellInfos[i].pos.x += aim_distance;
                }

                var item_pos = Mathf.Abs(pos_x + rect_cell.sizeDelta.x * (1 - rect_cell.pivot.x));
                if (item_pos > RectScroll.sizeDelta.x)
                {
                    RectContent.sizeDelta = new Vector2(item_pos + padding.right, RectContent.sizeDelta.y);
                }
            }
        }

        private void update_content_size()
        {
            var last_cell = CellInfos[CellInfos.Length - 1];
            
            float width = last_cell.width == 0 ? CellObjectWidth : last_cell.width;
            float height = last_cell.height == 0 ? CellObjectHeight : last_cell.height;
            var rect_cell = cellGameObject.GetComponent<RectTransform>();
            if (last_cell.obj != null)
            {
                rect_cell = last_cell.obj.GetComponent<RectTransform>();

            }
            float pivot_x = rect_cell.pivot.x * width;
            float pivot_y = rect_cell.pivot.y * height;

            if (listViewDirection == EnumListViewDirection.Vertical)
            {
                // var cell_size_y = CellObjectHeight * (1 - cellGameObject.GetComponent<RectTransform>().pivot.y);
                var size_y = Mathf.Abs(last_cell.pos.y) + (height - pivot_y) + padding.bottom;
                if (size_y > RectScroll.sizeDelta.y)
                {
                    RectContent.sizeDelta = new Vector2(RectContent.sizeDelta.x, size_y);
                }
            }
            else
            {
                // var cell_size_x = CellObjectWidth * (1 - cellGameObject.GetComponent<RectTransform>().pivot.x);
                var size_x = Mathf.Abs(last_cell.pos.x) + (width - pivot_x) + padding.right;
                if (size_x > RectScroll.sizeDelta.x)
                {
                    RectContent.sizeDelta = new Vector2(size_x, RectContent.sizeDelta.y);
                }
            }
        }
        
        //滑动事件
        protected virtual void scrollRectListener(Vector2 value)
        {
            float content_pos = listViewDirection == EnumListViewDirection.Vertical ? RectContent.anchoredPosition.y : RectContent.anchoredPosition.x;
            bool move_forward = (listViewDirection == EnumListViewDirection.Vertical && content_pos > _movePos) || (listViewDirection == EnumListViewDirection.Horizontal && content_pos < _movePos);
            _movePos = content_pos;
            StartCoroutine(update_check(move_forward));
            update_arrow();
        }

        private IEnumerator update_check(bool move_forward)
        {
            if (CellInfos == null)
            {
                yield return null;
            }

            int per_frame_count = 0;
            MinIndex = -1;
            for (int i = 0; i < CellInfos.Length; i++)
            {
                CellInfo cell_info = CellInfos[i];
                GameObject obj = cell_info.obj;
                Vector3 pos = cell_info.pos;

                float range_pos = listViewDirection == EnumListViewDirection.Vertical ? pos.y : pos.x;
                if (is_out_range(range_pos, i))
                {
                    if (obj != null)
                    {
                        _pool.recOne(obj);
                        CellInfos[i].obj = null;
                    }
                }
                else
                {
                    MinIndex = MinIndex == -1 ? i : MinIndex;
                    MaxIndex = i;
                    if (obj == null)
                    {
                        GameObject cell = _pool.getOne(RectContent);
                        cell.transform.SetParent(RectContent);
                        cell.name = i.ToString();
                        CellInfos[i].obj = cell;
                        callUpdateFun(_funUpdateItemUI, cell, i);
                        var previous_cell = CellInfos[Mathf.Max(0, i - 1)];
                        while (previous_cell.obj == null || CellInfos[i].obj == null)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                        update_pos(i, move_forward);
                        cell.GetComponent<RectTransform>().localPosition = CellInfos[i].pos;
                        per_frame_count++;
                        if (frameLoad && per_frame_count >= perFrameCount)
                        {
                            yield return new WaitForFixedUpdate();
                        }
                    }
                    else
                    {
                        update_pos(i, move_forward);
                        obj.GetComponent<RectTransform>().localPosition = CellInfos[i].pos;
                    }
                }
            }
        }
        
        //更新箭头
        private void update_arrow()
        {
            if (leftArrow == null || rightArrow == null)
            {
                return;
            }
            
            leftArrow.gameObject.SetActive(MinIndex != 0);
            rightArrow.gameObject.SetActive(MaxIndex != CellInfos.Length - 1);
        }

        /// <summary>
        /// 更新某一项
        /// </summary>
        /// <param name="index"></param>
        public virtual void updateItem(int index)
        {
            CellInfo cell_info = CellInfos[index];
            if (cell_info.obj != null)
            {
                float range_pos = listViewDirection == EnumListViewDirection.Vertical ? cell_info.pos.y : cell_info.pos.x;
                if (!is_out_range(range_pos, index))
                {
                    callUpdateFun(_funUpdateItemUI, cell_info.obj, index);
                }
            }
        }

        /// <summary>
        /// 移动到指定的item位置 对于尺寸变化的item,在第一次item刷新前就跳转,计算出的位置会有问题.
        /// </summary>
        /// <param name="index">对应的item下标</param>
        public virtual void MoveTo(int index)
        {
            if (index <= 0)
            {
                if (listViewDirection == EnumListViewDirection.Vertical)
                {
                    RectContent.anchoredPosition = new Vector2(5, 0);
                }
                else
                {
                    RectContent.anchoredPosition = new Vector2(5, 0);
                }
                scrollRectListener(Vector2.zero);
                return;
            }
            
            if (index >= CellInfos.Length)
            {
                // Debug.LogError($"跳转位置错误{index}");
                index = CellInfos.Length - 1;
            }
            
            CellInfo cell_info = CellInfos[index];
            
            float width = cell_info.width == 0 ? CellObjectWidth : cell_info.width;
            float height = cell_info.height == 0 ? CellObjectHeight : cell_info.height;
            var rect_cell = cellGameObject.GetComponent<RectTransform>();
            if (cell_info.obj != null)
            {
                rect_cell = cell_info.obj.GetComponent<RectTransform>();

            }
            float pivot_x = rect_cell.pivot.x * width;
            float pivot_y = rect_cell.pivot.y * height;
            
            if (listViewDirection == EnumListViewDirection.Vertical)
            {
                float pos = Mathf.Max(-cell_info.pos.y + pivot_y - ScrollHeight / 2, 0);
                pos = Mathf.Min(RectContent.sizeDelta.y - ScrollHeight, pos);
                RectContent.anchoredPosition = new Vector2(RectContent.anchoredPosition.x, pos);
            }
            else
            {
                float pos = Mathf.Max(-cell_info.pos.x - pivot_x + ScrollWidth / 2, -RectContent.sizeDelta.x + ScrollWidth);
                pos = Mathf.Min(0, pos);
                RectContent.anchoredPosition = new Vector2(pos, RectContent.anchoredPosition.y);
            }

            scrollRectListener(Vector2.zero);
        }

    #if UNITY_EDITOR
        [InspectorButton("布局测试")]
        private void show_test()
        {
            var com = gameObject.GetComponent<ListView>();
            var scroll = gameObject.GetComponent<ScrollRect>();
            _bInitial = false;
            _bInitialItem = false;
            for (int i = scroll.content.transform.childCount - 1; i >= 1; i--)
            {
                var child = scroll.content.transform.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
            com.Init((index, gameObject) => { });
            ShowList(testCount, false);
        }
    #endif
    }
}

