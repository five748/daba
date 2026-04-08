namespace Table{
   [System.Serializable]
    public class CargoPintuType:ITable{
            public int id;
            public float[][] pos;
            public string icon;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){pos = strs[1].TwoStringToArray<float>();}
                if(!string.IsNullOrEmpty(strs[2])){icon = strs[2];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
