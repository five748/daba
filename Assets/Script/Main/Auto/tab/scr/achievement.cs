namespace Table{
   [System.Serializable]
    public class achievement:ITable{
            public int id;
            public int typeId;
            public string desc;
            public int targetNum;
            public int offLine;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){typeId = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){desc = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){targetNum = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){offLine = int.Parse(strs[4]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
