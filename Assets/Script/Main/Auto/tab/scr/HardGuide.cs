namespace Table{
   [System.Serializable]
    public class HardGuide:ITable{
            public int id;
            public int next;
            public int group;
            public int delay;
            public int posY;
            public int can_pass;
            public int dialogue_id;
            public string tip;
            public int show_type;
            public int can_click_outer;
            public int recovery_window;
            public int exhibition;
            public int win_id;
            public string open_param;
            public int win_btn;
            public int[] open_limit_type;
            public float[] open_limit_param;
            public int[] special_event_type;
            public float[] special_event_param;
            public int Init(string str){
                string[] strs = str.Split('↕');
                if(!string.IsNullOrEmpty(strs[0])){id = int.Parse(strs[0]);}
                if(!string.IsNullOrEmpty(strs[1])){next = int.Parse(strs[1]);}
                if(!string.IsNullOrEmpty(strs[2])){group = int.Parse(strs[2]);}
                if(!string.IsNullOrEmpty(strs[4])){delay = int.Parse(strs[4]);}
                if(!string.IsNullOrEmpty(strs[5])){posY = int.Parse(strs[5]);}
                if(!string.IsNullOrEmpty(strs[6])){can_pass = int.Parse(strs[6]);}
                if(!string.IsNullOrEmpty(strs[7])){dialogue_id = int.Parse(strs[7]);}
                if(!string.IsNullOrEmpty(strs[8])){tip = strs[8];}
                if(!string.IsNullOrEmpty(strs[9])){show_type = int.Parse(strs[9]);}
                if(!string.IsNullOrEmpty(strs[10])){can_click_outer = int.Parse(strs[10]);}
                if(!string.IsNullOrEmpty(strs[11])){recovery_window = int.Parse(strs[11]);}
                if(!string.IsNullOrEmpty(strs[12])){exhibition = int.Parse(strs[12]);}
                if(!string.IsNullOrEmpty(strs[13])){win_id = int.Parse(strs[13]);}
                if(!string.IsNullOrEmpty(strs[14])){open_param = strs[14];}
                if(!string.IsNullOrEmpty(strs[15])){win_btn = int.Parse(strs[15]);}
                if(!string.IsNullOrEmpty(strs[16])){open_limit_type = strs[16].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[17])){open_limit_param = strs[17].SplitToFloatArray(',');}
                if(!string.IsNullOrEmpty(strs[18])){special_event_type = strs[18].SplitToIntArray(',');}
                if(!string.IsNullOrEmpty(strs[19])){special_event_param = strs[19].SplitToFloatArray(',');}
                return int.Parse(strs[0]);
            }//=======代码自动生成请勿修改=======
        }
    }
