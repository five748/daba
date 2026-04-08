namespace Table{
   [System.Serializable]
    public class SoftGuide:ITable{
            public int id;
            public int dialogue_id;
            public string tip;
            public int show_type;
            public int times;
            public int[] hard_complete;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){dialogue_id = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){tip = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){show_type = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){times = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){hard_complete = strs[5].SplitToIntArray(',');}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
