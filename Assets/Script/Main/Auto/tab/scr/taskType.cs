namespace Table{
   [System.Serializable]
    public class taskType:ITable{
            public int id;
            public int type;
            public string desc;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[4])){type = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){desc = strs[5];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
