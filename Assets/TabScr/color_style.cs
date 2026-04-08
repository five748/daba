namespace Table{
   [System.Serializable]
    public class color_style:ITable{
            public int id;
            public string name;
            public string EnumKey;
            public string Code;
            public int GradientWhole;
            public int GradientType;
            public string ColorName;
            public string outline;
            public string outlineRang;
            public string shadow;
            public string shadowRang;
            public string mark;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){EnumKey = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){Code = strs[3];}
                if(!string.IsNullOrEmpty(strs[4])){GradientWhole = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){GradientType = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){ColorName = strs[6];}
                if(!string.IsNullOrEmpty(strs[7])){outline = strs[7];}
                if(!string.IsNullOrEmpty(strs[8])){outlineRang = strs[8];}
                if(!string.IsNullOrEmpty(strs[9])){shadow = strs[9];}
                if(!string.IsNullOrEmpty(strs[10])){shadowRang = strs[10];}
                if(!string.IsNullOrEmpty(strs[11])){mark = strs[11];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
