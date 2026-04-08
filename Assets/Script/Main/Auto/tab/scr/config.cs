namespace Table{
   [System.Serializable]
    public class config:ITable{
            public int id;
            public string key;
            public string param;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){key = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){param = strs[2];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
