namespace Table{
   [System.Serializable]
    public class orderCoe:ITable{
            public int id;
            public int coe;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){coe = int.Parse(strs[1]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
