namespace Table{
   [System.Serializable]
    public class res:ITable{
            public int id;
            public string resPath;
            public string resName;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){resPath = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){resName = strs[2];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
