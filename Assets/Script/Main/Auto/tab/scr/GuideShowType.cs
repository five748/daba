namespace Table{
   [System.Serializable]
    public class GuideShowType:ITable{
            public int id;
            public string enumKey;
            public string enumDes;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){enumKey = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){enumDes = strs[2];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
