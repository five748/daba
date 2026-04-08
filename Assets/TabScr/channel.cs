namespace Table{
   [System.Serializable]
    public class channel:ITable{
            public int id;
            public string name;
            public string GitPath;
            public int Platform;
            public string OsType;
            public string Cooperation;
            public int MergeChannel;
            public int CanLogin;
            public string SDKPath;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){GitPath = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){Platform = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){OsType = strs[4];}
                if(!string.IsNullOrEmpty(strs[5])){Cooperation = strs[5];}
                if(!string.IsNullOrEmpty(strs[6])){MergeChannel = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){CanLogin = int.Parse(strs[7]);}
                if(!string.IsNullOrEmpty(strs[8])){SDKPath = strs[8];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
