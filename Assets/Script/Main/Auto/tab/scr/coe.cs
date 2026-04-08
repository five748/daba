namespace Table{
   [System.Serializable]
    public class coe:ITable{
            public int id;
            public string name;
            public int forChannelId;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[3])){forChannelId = int.Parse(strs[3]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
