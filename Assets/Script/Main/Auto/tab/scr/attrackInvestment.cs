namespace Table{
   [System.Serializable]
    public class attrackInvestment:ITable{
            public int id;
            public float minuteCoe;
            public int minute;
            public string color;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){minuteCoe = float.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){minute = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){color = strs[3];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
