namespace Table{
   [System.Serializable]
    public class sub_channel:ITable{
            public int id;
            public string name;
            public int Platform;
            public string OsType;
            public string SDKPath;
            public int CanLogin;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){Platform = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){OsType = strs[3];}
                if(!string.IsNullOrEmpty(strs[4])){SDKPath = strs[4];}
                if(!string.IsNullOrEmpty(strs[5])){CanLogin = int.Parse(strs[5]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
