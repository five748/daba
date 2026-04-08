namespace Table{
   [System.Serializable]
    public class tollCollector:ITable{
            public int id;
            public string name;
            public string aniId;
            public int score;
            public int forDamId;
            public int forChannelId;
            public int[] lvupRate;
            public int[] lvupCost;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){aniId = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){score = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){forDamId = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){forChannelId = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){lvupRate = strs[6].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[7])){lvupCost = strs[7].SplitToIntArray(',');}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
