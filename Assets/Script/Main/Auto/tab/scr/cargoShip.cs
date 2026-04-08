namespace Table{
   [System.Serializable]
    public class cargoShip:ITable{
            public int id;
            public string name;
            public int icon;
            public int unlockType;
            public int score;
            public int speed;
            public int capacity;
            public int capacityLvup;
            public System.Collections.Generic.List<TreeData.item> cost;
            public float[] ship_size;
            public string isFrameAni;
            public string moveAni;
            public string move2Ani;
            public float[] offset;
            public float scale_size;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){icon = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){unlockType = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){score = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){speed = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){capacity = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){capacityLvup = int.Parse(strs[7]);}
                if(!string.IsNullOrEmpty(strs[8])){cost = strs[8].ToItems();}
                if(!string.IsNullOrEmpty(strs[9])){ship_size = strs[9].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[10])){isFrameAni = strs[10];}
                if(!string.IsNullOrEmpty(strs[11])){moveAni = strs[11];}
                if(!string.IsNullOrEmpty(strs[12])){move2Ani = strs[12];}
                if(!string.IsNullOrEmpty(strs[13])){offset = strs[13].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[14])){scale_size = float.Parse(strs[14]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
