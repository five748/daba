namespace Table{
   [System.Serializable]
    public class bgm:ITable{
            public int id;
            public int bgtype;
            public string path;
            public float volume;
            public float pitch;
            public int isLoop;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){bgtype = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){path = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){volume = float.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){pitch = float.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){isLoop = int.Parse(strs[5]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
