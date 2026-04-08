namespace Table{
   [System.Serializable]
    public class order:ITable{
            public int id;
            public int dis;
            public string name;
            public float rewardCoe;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){dis = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){name = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){rewardCoe = float.Parse(strs[3]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
