namespace Table{
   [System.Serializable]
    public class collection:ITable{
            public int id;
            public string name;
            public string desc;
            public int quality;
            public int propType;
            public int prop;
            public int shipType;
            public System.Collections.Generic.List<TreeData.item> salePrice;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){name = strs[1];}
                if(!string.IsNullOrEmpty(strs[2])){desc = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){quality = int.Parse(strs[3]);}
                if(!string.IsNullOrEmpty(strs[4])){propType = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){prop = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){shipType = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){salePrice = strs[7].ToItems();}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
