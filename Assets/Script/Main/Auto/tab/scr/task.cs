namespace Table{
   [System.Serializable]
    public class task:ITable{
            public int id;
            public System.Collections.Generic.List<TreeData.item> reward;
            public int typeId;
            public int targetNum;
            public int param;
            public string desc1;
            public string desc0;
            public string desc2;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){reward = strs[1].ToItems();}
                if(!string.IsNullOrEmpty(strs[2])){typeId = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){targetNum = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){param = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[6])){desc1 = strs[6];}
                if(!string.IsNullOrEmpty(strs[7])){desc0 = strs[7];}
                if(!string.IsNullOrEmpty(strs[8])){desc2 = strs[8];}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
