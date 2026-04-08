namespace Table{
   [System.Serializable]
    public class SF:ITable{
            public int id;
            public string name;
            public int icon;
            public int type;
            public string typeName;
            public int[] lvupReduceTime;
            public int[] lvupCost;
            public int passTime;
            public int unlockType;
            public int score;
            public int toll;
            public float size;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){icon = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){type = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){typeName = strs[4];}
                if(!string.IsNullOrEmpty(strs[5])){lvupReduceTime = strs[5].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[6])){lvupCost = strs[6].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[8])){passTime = int.Parse(strs[8]);}
                if(!string.IsNullOrEmpty(strs[9])){unlockType = int.Parse(strs[9]);}
                if(!string.IsNullOrEmpty(strs[10])){score = int.Parse(strs[10]);}
                if(!string.IsNullOrEmpty(strs[11])){toll = int.Parse(strs[11]);}
                if(!string.IsNullOrEmpty(strs[12])){size = float.Parse(strs[12]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
