namespace Table{
   [System.Serializable]
    public class channelTB:ITable{
            public int id;
            public string channel;
            public string platform;
            public string gName;
            public string git;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){channel = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){platform = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){gName = strs[3];}
                if(!string.IsNullOrEmpty(strs[4])){git = strs[4];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
