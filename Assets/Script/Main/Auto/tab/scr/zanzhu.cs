namespace Table{
   [System.Serializable]
    public class zanzhu:ITable{
            public int id;
            public int score;
            public int reward;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){score = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[3])){reward = int.Parse(strs[3]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
