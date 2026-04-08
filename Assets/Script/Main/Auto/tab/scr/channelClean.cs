namespace Table{
   [System.Serializable]
    public class channelClean:ITable{
            public int id;
            public int[] pool;
            public int time;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){pool = strs[1].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[2])){time = int.Parse(strs[2]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
