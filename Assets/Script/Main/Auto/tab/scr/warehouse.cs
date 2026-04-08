namespace Table{
   [System.Serializable]
    public class warehouse:ITable{
            public int id;
            public int capacity;
            public float output;
            public System.Collections.Generic.List<TreeData.item> capacityCost;
            public System.Collections.Generic.List<TreeData.item> outputCost;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){capacity = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){output = float.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[3])){capacityCost = strs[3].ToItems();}
                if(!string.IsNullOrEmpty(strs[4])){outputCost = strs[4].ToItems();}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
