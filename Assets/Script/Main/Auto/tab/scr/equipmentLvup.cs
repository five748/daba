namespace Table{
   [System.Serializable]
    public class equipmentLvup:ITable{
            public int id;
            public string name;
            public int forChannelId;
            public int iconId;
            public int needScore;
            public int passSpeed;
            public int shipFlow;
            public int[] cost;
            public int forDamId;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){forChannelId = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){iconId = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){needScore = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){passSpeed = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){shipFlow = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){cost = strs[7].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[8])){forDamId = int.Parse(strs[8]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
