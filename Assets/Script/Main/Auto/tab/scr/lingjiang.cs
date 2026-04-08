namespace Table{
   [System.Serializable]
    public class lingjiang:ITable{
            public int id;
            public int cd;
            public int reward;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){cd = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){reward = int.Parse(strs[2]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
