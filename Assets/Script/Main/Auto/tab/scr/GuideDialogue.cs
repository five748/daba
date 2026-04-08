namespace Table{
   [System.Serializable]
    public class GuideDialogue:ITable{
            public int id;
            public int next;
            public string content;
            public string npc_img_path;
            public int npc_pos;
            public float flip;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){next = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){content = strs[2];}
                if(!string.IsNullOrEmpty(strs[3])){npc_img_path = strs[3];}
                if(!string.IsNullOrEmpty(strs[4])){npc_pos = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){flip = float.Parse(strs[5]);}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
