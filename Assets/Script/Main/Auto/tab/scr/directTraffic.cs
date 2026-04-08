namespace Table{
   [System.Serializable]
    public class directTraffic:ITable{
            public int id;
            public int[][] chipPos;
            public int[] barrierPos;
            public System.Collections.Generic.List<TreeData.item> reward;
            public int score;
            public int icon;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){chipPos = strs[1].TwoStringToArray<int>();}
                if(!string.IsNullOrEmpty(strs[2])){barrierPos = strs[2].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[3])){reward = strs[3].ToItems();}
                if(!string.IsNullOrEmpty(strs[4])){score = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){icon = int.Parse(strs[5]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
