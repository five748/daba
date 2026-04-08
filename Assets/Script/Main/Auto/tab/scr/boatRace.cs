namespace Table{
   [System.Serializable]
    public class boatRace:ITable{
            public int id;
            public string unlockBoatId;
            public int unlockType;
            public int param;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){unlockBoatId = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){unlockType = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){param = int.Parse(strs[3]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
