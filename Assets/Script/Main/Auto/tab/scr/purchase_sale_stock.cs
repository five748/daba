namespace Table{
   [System.Serializable]
    public class purchase_sale_stock:ITable{
            public int id;
            public string eKey;
            public string des;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){eKey = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){des = strs[2];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
