namespace Table{
   [System.Serializable]
    public class report:ITable{
            public int id;
            public string eKey;
            public string des;
            public int type;
            public int mainId;
            public int childId;
            public System.Collections.Generic.Dictionary<string,int> param;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){eKey = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){des = strs[2];}
                if(!string.IsNullOrEmpty(strs[4])){type = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){mainId = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){childId = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){param = strs[7].ToDicStringInt();}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
