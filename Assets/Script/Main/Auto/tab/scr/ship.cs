namespace Table{
   [System.Serializable]
    public class ship:ITable{
            public int id;
            public string name;
            public int icon;
            public int type;
            public string typeName;
            public int[] lvupReduceTime;
            public int[] lvupCost;
            public int passTime;
            public int unlockType;
            public int score;
            public int toll;
            public float size;
            public float[] btn_offset;
            public float[] ship_size;
            public string isFrameAni;
            public string moveAni;
            public string qiehuanAni;
            public string idleAni;
            public string move2Ani;
            public float[] offset;
            public float scale_size;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){icon = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){type = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){typeName = strs[4];}
                if(!string.IsNullOrEmpty(strs[5])){lvupReduceTime = strs[5].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[6])){lvupCost = strs[6].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[8])){passTime = int.Parse(strs[8]);}
                if(!string.IsNullOrEmpty(strs[9])){unlockType = int.Parse(strs[9]);}
                if(!string.IsNullOrEmpty(strs[10])){score = int.Parse(strs[10]);}
                if(!string.IsNullOrEmpty(strs[11])){toll = int.Parse(strs[11]);}
                if(!string.IsNullOrEmpty(strs[12])){size = float.Parse(strs[12]);}
                if(!string.IsNullOrEmpty(strs[13])){btn_offset = strs[13].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[14])){ship_size = strs[14].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[15])){isFrameAni = strs[15];}
                if(!string.IsNullOrEmpty(strs[16])){moveAni = strs[16];}
                if(!string.IsNullOrEmpty(strs[17])){qiehuanAni = strs[17];}
                if(!string.IsNullOrEmpty(strs[18])){idleAni = strs[18];}
                if(!string.IsNullOrEmpty(strs[19])){move2Ani = strs[19];}
                if(!string.IsNullOrEmpty(strs[20])){offset = strs[20].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[21])){scale_size = float.Parse(strs[21]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
