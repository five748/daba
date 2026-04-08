namespace tableMenu{
    public enum SoftGuideEvent{
        when_task_completel_tip = 20101,//有任务完成的时候,有提示
        when_task_completel = 20111,//有任务完成的时候,没有提示
        when_technology_value_enough_dialog = 20201,//科技点够提升科技时出现
        when_technology_value_enough = 20301,//科技点够提升科技时出现
        launch_rocket = 20401,//强制引导9完成,火箭处于可以发射的状态下
        upgrade_station_enough_resource = 20501,//强制引导11完成，资源够升级空间站
        upgrade_goods_station_enough_resource = 20601,//货物仓解锁，资源足够并且在货物仓界面，触发引导，没对话
        unlock_station_then_back_start = 20701,//解锁空间站后，第一次回到地球，触发，纯对话
        unlock_moon_then_back_start = 20801,//解锁月球后，第一次回到任意星球触发，纯对话
    //=======代码自动生成请勿修改=======
    }
}