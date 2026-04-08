namespace Table{
   [System.Serializable]
    public class item:ITable{
            public int id;
            public string initCost;
            public string desc;
            public int initNum;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){initCost = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){desc = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){initNum = int.Parse(strs[3]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
