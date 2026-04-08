namespace Table{
   [System.Serializable]
    public class buildingItem:ITable{
            public int id;
            public string name;
            public int score;
            public int unlock_cost;
            public float effectSize;
            public int[] effectPos;
            public int forDamId;
            public int forChannelId;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){score = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){unlock_cost = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){effectSize = float.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){effectPos = strs[5].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[6])){forDamId = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){forChannelId = int.Parse(strs[7]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
