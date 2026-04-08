namespace Table{
   [System.Serializable]
    public class loadingOfCargo:ITable{
            public int id;
            public int icon;
            public int[] itemId;
            public int[][] barrierPos;
            public int reward;
            public int score;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){icon = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){itemId = strs[2].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[3])){barrierPos = strs[3].TwoStringToArray<int>();}
                if(!string.IsNullOrEmpty(strs[4])){reward = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){score = int.Parse(strs[5]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
