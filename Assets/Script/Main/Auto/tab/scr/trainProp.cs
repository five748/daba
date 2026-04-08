namespace Table{
   [System.Serializable]
    public class trainProp:ITable{
            public int id;
            public string propLv;
            public int prop;
            public string color;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){propLv = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){prop = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){color = strs[3];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
