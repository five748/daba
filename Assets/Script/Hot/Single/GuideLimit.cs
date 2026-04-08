using tableMenu;

/// <summary>
/// 用于一些特殊性质的引导开启检查 例如系统开放限制 等级限制
/// </summary>
public class GuideLimit
{
    public bool CheckPass(int[] types, float[] datas, int tId)
    {
        if (types == null)
        {
            return true;
        }
        
        for (int i = 0; i < types.Length; i++)
        {
            int type = types[i];
            float data = datas[i];
            if (!check(type, data, tId))
            {
                return false;
            }
        }
        
        return true;
    }

    private bool check(int type, float data, int tId)
    {
        bool result = true;
        switch (type)
        {
            case (int)GuideLimitType.task_begin:
                result = task_begin(data);
                break;
            case (int)GuideLimitType.skip_50010:
                result = skip_50010(data);
                break;
            // case (int)GuideLimitType.task_complete:
            //     result = task_complete(data);
            //     break;
            // case (int)GuideLimitType.unlock_star:
            //     result = unlock_star(data);
            //     break;
            // case (int)GuideLimitType.player_die_count:
            //     result = player_die_count(data);
            //     break;
            // case (int)GuideLimitType.boss_die_count:
            //     result = boss_die_count(data);
            //     break;
        }

        return result;
    }
    
    //引导特殊限制判定
    private bool task_begin(float taskId)
    {
        return MTaskData.Instance.currentId >= (int)taskId; //TaskMgr.Instance.CheckIsDoTask((int)task_id);
    }
    
    private bool skip_50010(float data)
    {
        var dataShip = ShipMgr.Instance.data.ships;
        int count = 0;
        foreach (var (key, ship) in dataShip)
        {
            if (ship.unlock)
            {
                count += ship.lv;
            }
        }

        if (count >= data)
        {
            GuideMgr.Instance.CompleteGroup(50010);
            GuideMgr.Instance.CompleteCurGuide();
            return false;
        }

        return true;
    }
}