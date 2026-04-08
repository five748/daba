namespace Table{
   [System.Serializable]
    public class repairDungeon:ITable{
            public int id;
            public string title;
            public int shipId;
            public int repairNum;
            public string desc;
            public int score;
            public int[] items;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){title = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){shipId = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){repairNum = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){desc = strs[4];}
                if(!string.IsNullOrEmpty(strs[5])){score = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){items = strs[6].SplitToIntArray(',');}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
