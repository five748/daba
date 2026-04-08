using Table;
using System.Collections.Generic;
public class TableCache:Single<TableCache>{
    private Dictionary<int,channelTB> _channelTBTable;
    private Dictionary<int,config> _configTable;
    private Dictionary<int,achievement> _achievementTable;
    private Dictionary<int,achievementType> _achievementTypeTable;
    private Dictionary<int,ship> _shipTable;
    private Dictionary<int,collection> _collectionTable;
    private Dictionary<int,collectionOutput> _collectionOutputTable;
    private Dictionary<int,dam> _damTable;
    private Dictionary<int,report> _reportTable;
    private Dictionary<int,reportInfo> _reportInfoTable;
    private Dictionary<int,purchase_sale_stock> _purchase_sale_stockTable;
    private Dictionary<int,item> _itemTable;
    private Dictionary<int,ad> _adTable;
    private Dictionary<int,zanzhu> _zanzhuTable;
    private Dictionary<int,lingjiang> _lingjiangTable;
    private Dictionary<int,lingjiangBox> _lingjiangBoxTable;
    private Dictionary<int,meirifuli> _meirifuliTable;
    private Dictionary<int,meirifuliBox> _meirifuliBoxTable;
    private Dictionary<int,cargoShip> _cargoShipTable;
    private Dictionary<int,warehouse> _warehouseTable;
    private Dictionary<int,order> _orderTable;
    private Dictionary<int,orderCoe> _orderCoeTable;
    private Dictionary<int,loadingOfCargo> _loadingOfCargoTable;
    private Dictionary<int,CargoPintuType> _CargoPintuTypeTable;
    private Dictionary<int,repairDungeon> _repairDungeonTable;
    private Dictionary<int,repair> _repairTable;
    private Dictionary<int,trainProp> _trainPropTable;
    private Dictionary<int,channelClean> _channelCleanTable;
    private Dictionary<int,channelCleanCoe> _channelCleanCoeTable;
    private Dictionary<int,buildingItem> _buildingItemTable;
    private Dictionary<int,coe> _coeTable;
    private Dictionary<int,boatRace> _boatRaceTable;
    private Dictionary<int,frame> _frameTable;
    private Dictionary<int,system> _systemTable;
    private Dictionary<int,progressCoe> _progressCoeTable;
    private Dictionary<int,equipmentLvup> _equipmentLvupTable;
    private Dictionary<int,HardGuide> _HardGuideTable;
    private Dictionary<int,GuideDialogue> _GuideDialogueTable;
    private Dictionary<int,GuideShowType> _GuideShowTypeTable;
    private Dictionary<int,GuideWindow> _GuideWindowTable;
    private Dictionary<int,GuideWindownBtn> _GuideWindownBtnTable;
    private Dictionary<int,GuideLimitType> _GuideLimitTypeTable;
    private Dictionary<int,GuideSpecialEvent> _GuideSpecialEventTable;
    private Dictionary<int,SoftGuide> _SoftGuideTable;
    private Dictionary<int,SoftGuideEvent> _SoftGuideEventTable;
    private Dictionary<int,GameSet> _GameSetTable;
    private Dictionary<int,sound> _soundTable;
    private Dictionary<int,bgm> _bgmTable;
    private Dictionary<int,task> _taskTable;
    private Dictionary<int,taskType> _taskTypeTable;
    private Dictionary<int,attrackInvestment> _attrackInvestmentTable;
    private Dictionary<int,attrackInvestmentType> _attrackInvestmentTypeTable;
    private Dictionary<int,attrackInvestmentUnlock> _attrackInvestmentUnlockTable;
    private Dictionary<int,attrackInvestmentCoe> _attrackInvestmentCoeTable;
    private Dictionary<int,directTraffic> _directTrafficTable;
    private Dictionary<int,tollCollector> _tollCollectorTable;
    private Dictionary<int,prefabUI> _prefabUITable;
    private Dictionary<int,changeImga> _changeImgaTable;
    private Dictionary<int,changeframe> _changeframeTable;
    private Dictionary<int,changeTable> _changeTableTable;
    public Dictionary<int,channelTB> channelTBTable{
        get{
            if(_channelTBTable == null ||_channelTBTable.Count == 0)
                _channelTBTable = TableRead.Instance.ReadTable<channelTB>("channelTB");
            return _channelTBTable;
        }
    }
    public Dictionary<int,config> configTable{
        get{
            if(_configTable == null ||_configTable.Count == 0)
                _configTable = TableRead.Instance.ReadTable<config>("config");
            return _configTable;
        }
    }
    public Dictionary<int,achievement> achievementTable{
        get{
            if(_achievementTable == null ||_achievementTable.Count == 0)
                _achievementTable = TableRead.Instance.ReadTable<achievement>("achievement");
            return _achievementTable;
        }
    }
    public Dictionary<int,achievementType> achievementTypeTable{
        get{
            if(_achievementTypeTable == null ||_achievementTypeTable.Count == 0)
                _achievementTypeTable = TableRead.Instance.ReadTable<achievementType>("achievementType");
            return _achievementTypeTable;
        }
    }
    public Dictionary<int,ship> shipTable{
        get{
            if(_shipTable == null ||_shipTable.Count == 0)
                _shipTable = TableRead.Instance.ReadTable<ship>("ship");
            return _shipTable;
        }
    }
    public Dictionary<int,collection> collectionTable{
        get{
            if(_collectionTable == null ||_collectionTable.Count == 0)
                _collectionTable = TableRead.Instance.ReadTable<collection>("collection");
            return _collectionTable;
        }
    }
    public Dictionary<int,collectionOutput> collectionOutputTable{
        get{
            if(_collectionOutputTable == null ||_collectionOutputTable.Count == 0)
                _collectionOutputTable = TableRead.Instance.ReadTable<collectionOutput>("collectionOutput");
            return _collectionOutputTable;
        }
    }
    public Dictionary<int,dam> damTable{
        get{
            if(_damTable == null ||_damTable.Count == 0)
                _damTable = TableRead.Instance.ReadTable<dam>("dam");
            return _damTable;
        }
    }
    public Dictionary<int,report> reportTable{
        get{
            if(_reportTable == null ||_reportTable.Count == 0)
                _reportTable = TableRead.Instance.ReadTable<report>("report");
            return _reportTable;
        }
    }
    public Dictionary<int,reportInfo> reportInfoTable{
        get{
            if(_reportInfoTable == null ||_reportInfoTable.Count == 0)
                _reportInfoTable = TableRead.Instance.ReadTable<reportInfo>("reportInfo");
            return _reportInfoTable;
        }
    }
    public Dictionary<int,purchase_sale_stock> purchase_sale_stockTable{
        get{
            if(_purchase_sale_stockTable == null ||_purchase_sale_stockTable.Count == 0)
                _purchase_sale_stockTable = TableRead.Instance.ReadTable<purchase_sale_stock>("purchase_sale_stock");
            return _purchase_sale_stockTable;
        }
    }
    public Dictionary<int,item> itemTable{
        get{
            if(_itemTable == null ||_itemTable.Count == 0)
                _itemTable = TableRead.Instance.ReadTable<item>("item");
            return _itemTable;
        }
    }
    public Dictionary<int,ad> adTable{
        get{
            if(_adTable == null ||_adTable.Count == 0)
                _adTable = TableRead.Instance.ReadTable<ad>("ad");
            return _adTable;
        }
    }
    public Dictionary<int,zanzhu> zanzhuTable{
        get{
            if(_zanzhuTable == null ||_zanzhuTable.Count == 0)
                _zanzhuTable = TableRead.Instance.ReadTable<zanzhu>("zanzhu");
            return _zanzhuTable;
        }
    }
    public Dictionary<int,lingjiang> lingjiangTable{
        get{
            if(_lingjiangTable == null ||_lingjiangTable.Count == 0)
                _lingjiangTable = TableRead.Instance.ReadTable<lingjiang>("lingjiang");
            return _lingjiangTable;
        }
    }
    public Dictionary<int,lingjiangBox> lingjiangBoxTable{
        get{
            if(_lingjiangBoxTable == null ||_lingjiangBoxTable.Count == 0)
                _lingjiangBoxTable = TableRead.Instance.ReadTable<lingjiangBox>("lingjiangBox");
            return _lingjiangBoxTable;
        }
    }
    public Dictionary<int,meirifuli> meirifuliTable{
        get{
            if(_meirifuliTable == null ||_meirifuliTable.Count == 0)
                _meirifuliTable = TableRead.Instance.ReadTable<meirifuli>("meirifuli");
            return _meirifuliTable;
        }
    }
    public Dictionary<int,meirifuliBox> meirifuliBoxTable{
        get{
            if(_meirifuliBoxTable == null ||_meirifuliBoxTable.Count == 0)
                _meirifuliBoxTable = TableRead.Instance.ReadTable<meirifuliBox>("meirifuliBox");
            return _meirifuliBoxTable;
        }
    }
    public Dictionary<int,cargoShip> cargoShipTable{
        get{
            if(_cargoShipTable == null ||_cargoShipTable.Count == 0)
                _cargoShipTable = TableRead.Instance.ReadTable<cargoShip>("cargoShip");
            return _cargoShipTable;
        }
    }
    public Dictionary<int,warehouse> warehouseTable{
        get{
            if(_warehouseTable == null ||_warehouseTable.Count == 0)
                _warehouseTable = TableRead.Instance.ReadTable<warehouse>("warehouse");
            return _warehouseTable;
        }
    }
    public Dictionary<int,order> orderTable{
        get{
            if(_orderTable == null ||_orderTable.Count == 0)
                _orderTable = TableRead.Instance.ReadTable<order>("order");
            return _orderTable;
        }
    }
    public Dictionary<int,orderCoe> orderCoeTable{
        get{
            if(_orderCoeTable == null ||_orderCoeTable.Count == 0)
                _orderCoeTable = TableRead.Instance.ReadTable<orderCoe>("orderCoe");
            return _orderCoeTable;
        }
    }
    public Dictionary<int,loadingOfCargo> loadingOfCargoTable{
        get{
            if(_loadingOfCargoTable == null ||_loadingOfCargoTable.Count == 0)
                _loadingOfCargoTable = TableRead.Instance.ReadTable<loadingOfCargo>("loadingOfCargo");
            return _loadingOfCargoTable;
        }
    }
    public Dictionary<int,CargoPintuType> CargoPintuTypeTable{
        get{
            if(_CargoPintuTypeTable == null ||_CargoPintuTypeTable.Count == 0)
                _CargoPintuTypeTable = TableRead.Instance.ReadTable<CargoPintuType>("CargoPintuType");
            return _CargoPintuTypeTable;
        }
    }
    public Dictionary<int,repairDungeon> repairDungeonTable{
        get{
            if(_repairDungeonTable == null ||_repairDungeonTable.Count == 0)
                _repairDungeonTable = TableRead.Instance.ReadTable<repairDungeon>("repairDungeon");
            return _repairDungeonTable;
        }
    }
    public Dictionary<int,repair> repairTable{
        get{
            if(_repairTable == null ||_repairTable.Count == 0)
                _repairTable = TableRead.Instance.ReadTable<repair>("repair");
            return _repairTable;
        }
    }
    public Dictionary<int,trainProp> trainPropTable{
        get{
            if(_trainPropTable == null ||_trainPropTable.Count == 0)
                _trainPropTable = TableRead.Instance.ReadTable<trainProp>("trainProp");
            return _trainPropTable;
        }
    }
    public Dictionary<int,channelClean> channelCleanTable{
        get{
            if(_channelCleanTable == null ||_channelCleanTable.Count == 0)
                _channelCleanTable = TableRead.Instance.ReadTable<channelClean>("channelClean");
            return _channelCleanTable;
        }
    }
    public Dictionary<int,channelCleanCoe> channelCleanCoeTable{
        get{
            if(_channelCleanCoeTable == null ||_channelCleanCoeTable.Count == 0)
                _channelCleanCoeTable = TableRead.Instance.ReadTable<channelCleanCoe>("channelCleanCoe");
            return _channelCleanCoeTable;
        }
    }
    public Dictionary<int,buildingItem> buildingItemTable{
        get{
            if(_buildingItemTable == null ||_buildingItemTable.Count == 0)
                _buildingItemTable = TableRead.Instance.ReadTable<buildingItem>("buildingItem");
            return _buildingItemTable;
        }
    }
    public Dictionary<int,coe> coeTable{
        get{
            if(_coeTable == null ||_coeTable.Count == 0)
                _coeTable = TableRead.Instance.ReadTable<coe>("coe");
            return _coeTable;
        }
    }
    public Dictionary<int,boatRace> boatRaceTable{
        get{
            if(_boatRaceTable == null ||_boatRaceTable.Count == 0)
                _boatRaceTable = TableRead.Instance.ReadTable<boatRace>("boatRace");
            return _boatRaceTable;
        }
    }
    public Dictionary<int,frame> frameTable{
        get{
            if(_frameTable == null ||_frameTable.Count == 0)
                _frameTable = TableRead.Instance.ReadTable<frame>("frame");
            return _frameTable;
        }
    }
    public Dictionary<int,system> systemTable{
        get{
            if(_systemTable == null ||_systemTable.Count == 0)
                _systemTable = TableRead.Instance.ReadTable<system>("system");
            return _systemTable;
        }
    }
    public Dictionary<int,progressCoe> progressCoeTable{
        get{
            if(_progressCoeTable == null ||_progressCoeTable.Count == 0)
                _progressCoeTable = TableRead.Instance.ReadTable<progressCoe>("progressCoe");
            return _progressCoeTable;
        }
    }
    public Dictionary<int,equipmentLvup> equipmentLvupTable{
        get{
            if(_equipmentLvupTable == null ||_equipmentLvupTable.Count == 0)
                _equipmentLvupTable = TableRead.Instance.ReadTable<equipmentLvup>("equipmentLvup");
            return _equipmentLvupTable;
        }
    }
    public Dictionary<int,HardGuide> HardGuideTable{
        get{
            if(_HardGuideTable == null ||_HardGuideTable.Count == 0)
                _HardGuideTable = TableRead.Instance.ReadTable<HardGuide>("HardGuide");
            return _HardGuideTable;
        }
    }
    public Dictionary<int,GuideDialogue> GuideDialogueTable{
        get{
            if(_GuideDialogueTable == null ||_GuideDialogueTable.Count == 0)
                _GuideDialogueTable = TableRead.Instance.ReadTable<GuideDialogue>("GuideDialogue");
            return _GuideDialogueTable;
        }
    }
    public Dictionary<int,GuideShowType> GuideShowTypeTable{
        get{
            if(_GuideShowTypeTable == null ||_GuideShowTypeTable.Count == 0)
                _GuideShowTypeTable = TableRead.Instance.ReadTable<GuideShowType>("GuideShowType");
            return _GuideShowTypeTable;
        }
    }
    public Dictionary<int,GuideWindow> GuideWindowTable{
        get{
            if(_GuideWindowTable == null ||_GuideWindowTable.Count == 0)
                _GuideWindowTable = TableRead.Instance.ReadTable<GuideWindow>("GuideWindow");
            return _GuideWindowTable;
        }
    }
    public Dictionary<int,GuideWindownBtn> GuideWindownBtnTable{
        get{
            if(_GuideWindownBtnTable == null ||_GuideWindownBtnTable.Count == 0)
                _GuideWindownBtnTable = TableRead.Instance.ReadTable<GuideWindownBtn>("GuideWindownBtn");
            return _GuideWindownBtnTable;
        }
    }
    public Dictionary<int,GuideLimitType> GuideLimitTypeTable{
        get{
            if(_GuideLimitTypeTable == null ||_GuideLimitTypeTable.Count == 0)
                _GuideLimitTypeTable = TableRead.Instance.ReadTable<GuideLimitType>("GuideLimitType");
            return _GuideLimitTypeTable;
        }
    }
    public Dictionary<int,GuideSpecialEvent> GuideSpecialEventTable{
        get{
            if(_GuideSpecialEventTable == null ||_GuideSpecialEventTable.Count == 0)
                _GuideSpecialEventTable = TableRead.Instance.ReadTable<GuideSpecialEvent>("GuideSpecialEvent");
            return _GuideSpecialEventTable;
        }
    }
    public Dictionary<int,SoftGuide> SoftGuideTable{
        get{
            if(_SoftGuideTable == null ||_SoftGuideTable.Count == 0)
                _SoftGuideTable = TableRead.Instance.ReadTable<SoftGuide>("SoftGuide");
            return _SoftGuideTable;
        }
    }
    public Dictionary<int,SoftGuideEvent> SoftGuideEventTable{
        get{
            if(_SoftGuideEventTable == null ||_SoftGuideEventTable.Count == 0)
                _SoftGuideEventTable = TableRead.Instance.ReadTable<SoftGuideEvent>("SoftGuideEvent");
            return _SoftGuideEventTable;
        }
    }
    public Dictionary<int,GameSet> GameSetTable{
        get{
            if(_GameSetTable == null ||_GameSetTable.Count == 0)
                _GameSetTable = TableRead.Instance.ReadTable<GameSet>("GameSet");
            return _GameSetTable;
        }
    }
    public Dictionary<int,sound> soundTable{
        get{
            if(_soundTable == null ||_soundTable.Count == 0)
                _soundTable = TableRead.Instance.ReadTable<sound>("sound");
            return _soundTable;
        }
    }
    public Dictionary<int,bgm> bgmTable{
        get{
            if(_bgmTable == null ||_bgmTable.Count == 0)
                _bgmTable = TableRead.Instance.ReadTable<bgm>("bgm");
            return _bgmTable;
        }
    }
    public Dictionary<int,task> taskTable{
        get{
            if(_taskTable == null ||_taskTable.Count == 0)
                _taskTable = TableRead.Instance.ReadTable<task>("task");
            return _taskTable;
        }
    }
    public Dictionary<int,taskType> taskTypeTable{
        get{
            if(_taskTypeTable == null ||_taskTypeTable.Count == 0)
                _taskTypeTable = TableRead.Instance.ReadTable<taskType>("taskType");
            return _taskTypeTable;
        }
    }
    public Dictionary<int,attrackInvestment> attrackInvestmentTable{
        get{
            if(_attrackInvestmentTable == null ||_attrackInvestmentTable.Count == 0)
                _attrackInvestmentTable = TableRead.Instance.ReadTable<attrackInvestment>("attrackInvestment");
            return _attrackInvestmentTable;
        }
    }
    public Dictionary<int,attrackInvestmentType> attrackInvestmentTypeTable{
        get{
            if(_attrackInvestmentTypeTable == null ||_attrackInvestmentTypeTable.Count == 0)
                _attrackInvestmentTypeTable = TableRead.Instance.ReadTable<attrackInvestmentType>("attrackInvestmentType");
            return _attrackInvestmentTypeTable;
        }
    }
    public Dictionary<int,attrackInvestmentUnlock> attrackInvestmentUnlockTable{
        get{
            if(_attrackInvestmentUnlockTable == null ||_attrackInvestmentUnlockTable.Count == 0)
                _attrackInvestmentUnlockTable = TableRead.Instance.ReadTable<attrackInvestmentUnlock>("attrackInvestmentUnlock");
            return _attrackInvestmentUnlockTable;
        }
    }
    public Dictionary<int,attrackInvestmentCoe> attrackInvestmentCoeTable{
        get{
            if(_attrackInvestmentCoeTable == null ||_attrackInvestmentCoeTable.Count == 0)
                _attrackInvestmentCoeTable = TableRead.Instance.ReadTable<attrackInvestmentCoe>("attrackInvestmentCoe");
            return _attrackInvestmentCoeTable;
        }
    }
    public Dictionary<int,directTraffic> directTrafficTable{
        get{
            if(_directTrafficTable == null ||_directTrafficTable.Count == 0)
                _directTrafficTable = TableRead.Instance.ReadTable<directTraffic>("directTraffic");
            return _directTrafficTable;
        }
    }
    public Dictionary<int,tollCollector> tollCollectorTable{
        get{
            if(_tollCollectorTable == null ||_tollCollectorTable.Count == 0)
                _tollCollectorTable = TableRead.Instance.ReadTable<tollCollector>("tollCollector");
            return _tollCollectorTable;
        }
    }
    public Dictionary<int,prefabUI> prefabUITable{
        get{
            if(_prefabUITable == null ||_prefabUITable.Count == 0)
                _prefabUITable = TableRead.Instance.ReadTable<prefabUI>("prefabUI");
            return _prefabUITable;
        }
    }
    public Dictionary<int,changeImga> changeImgaTable{
        get{
            if(_changeImgaTable == null ||_changeImgaTable.Count == 0)
                _changeImgaTable = TableRead.Instance.ReadTable<changeImga>("changeImga");
            return _changeImgaTable;
        }
    }
    public Dictionary<int,changeframe> changeframeTable{
        get{
            if(_changeframeTable == null ||_changeframeTable.Count == 0)
                _changeframeTable = TableRead.Instance.ReadTable<changeframe>("changeframe");
            return _changeframeTable;
        }
    }
    public Dictionary<int,changeTable> changeTableTable{
        get{
            if(_changeTableTable == null ||_changeTableTable.Count == 0)
                _changeTableTable = TableRead.Instance.ReadTable<changeTable>("changeTable");
            return _changeTableTable;
        }
    }
    public void ReadAllTab(){
            _channelTBTable = TableRead.Instance.ReadTable<channelTB>("channelTB");
            _configTable = TableRead.Instance.ReadTable<config>("config");
            _achievementTable = TableRead.Instance.ReadTable<achievement>("achievement");
            _achievementTypeTable = TableRead.Instance.ReadTable<achievementType>("achievementType");
            _shipTable = TableRead.Instance.ReadTable<ship>("ship");
            _collectionTable = TableRead.Instance.ReadTable<collection>("collection");
            _collectionOutputTable = TableRead.Instance.ReadTable<collectionOutput>("collectionOutput");
            _damTable = TableRead.Instance.ReadTable<dam>("dam");
            _reportTable = TableRead.Instance.ReadTable<report>("report");
            _reportInfoTable = TableRead.Instance.ReadTable<reportInfo>("reportInfo");
            _purchase_sale_stockTable = TableRead.Instance.ReadTable<purchase_sale_stock>("purchase_sale_stock");
            _itemTable = TableRead.Instance.ReadTable<item>("item");
            _adTable = TableRead.Instance.ReadTable<ad>("ad");
            _zanzhuTable = TableRead.Instance.ReadTable<zanzhu>("zanzhu");
            _lingjiangTable = TableRead.Instance.ReadTable<lingjiang>("lingjiang");
            _lingjiangBoxTable = TableRead.Instance.ReadTable<lingjiangBox>("lingjiangBox");
            _meirifuliTable = TableRead.Instance.ReadTable<meirifuli>("meirifuli");
            _meirifuliBoxTable = TableRead.Instance.ReadTable<meirifuliBox>("meirifuliBox");
            _cargoShipTable = TableRead.Instance.ReadTable<cargoShip>("cargoShip");
            _warehouseTable = TableRead.Instance.ReadTable<warehouse>("warehouse");
            _orderTable = TableRead.Instance.ReadTable<order>("order");
            _orderCoeTable = TableRead.Instance.ReadTable<orderCoe>("orderCoe");
            _loadingOfCargoTable = TableRead.Instance.ReadTable<loadingOfCargo>("loadingOfCargo");
            _CargoPintuTypeTable = TableRead.Instance.ReadTable<CargoPintuType>("CargoPintuType");
            _repairDungeonTable = TableRead.Instance.ReadTable<repairDungeon>("repairDungeon");
            _repairTable = TableRead.Instance.ReadTable<repair>("repair");
            _trainPropTable = TableRead.Instance.ReadTable<trainProp>("trainProp");
            _channelCleanTable = TableRead.Instance.ReadTable<channelClean>("channelClean");
            _channelCleanCoeTable = TableRead.Instance.ReadTable<channelCleanCoe>("channelCleanCoe");
            _buildingItemTable = TableRead.Instance.ReadTable<buildingItem>("buildingItem");
            _coeTable = TableRead.Instance.ReadTable<coe>("coe");
            _boatRaceTable = TableRead.Instance.ReadTable<boatRace>("boatRace");
            _frameTable = TableRead.Instance.ReadTable<frame>("frame");
            _systemTable = TableRead.Instance.ReadTable<system>("system");
            _progressCoeTable = TableRead.Instance.ReadTable<progressCoe>("progressCoe");
            _equipmentLvupTable = TableRead.Instance.ReadTable<equipmentLvup>("equipmentLvup");
            _HardGuideTable = TableRead.Instance.ReadTable<HardGuide>("HardGuide");
            _GuideDialogueTable = TableRead.Instance.ReadTable<GuideDialogue>("GuideDialogue");
            _GuideShowTypeTable = TableRead.Instance.ReadTable<GuideShowType>("GuideShowType");
            _GuideWindowTable = TableRead.Instance.ReadTable<GuideWindow>("GuideWindow");
            _GuideWindownBtnTable = TableRead.Instance.ReadTable<GuideWindownBtn>("GuideWindownBtn");
            _GuideLimitTypeTable = TableRead.Instance.ReadTable<GuideLimitType>("GuideLimitType");
            _GuideSpecialEventTable = TableRead.Instance.ReadTable<GuideSpecialEvent>("GuideSpecialEvent");
            _SoftGuideTable = TableRead.Instance.ReadTable<SoftGuide>("SoftGuide");
            _SoftGuideEventTable = TableRead.Instance.ReadTable<SoftGuideEvent>("SoftGuideEvent");
            _GameSetTable = TableRead.Instance.ReadTable<GameSet>("GameSet");
            _soundTable = TableRead.Instance.ReadTable<sound>("sound");
            _bgmTable = TableRead.Instance.ReadTable<bgm>("bgm");
            _taskTable = TableRead.Instance.ReadTable<task>("task");
            _taskTypeTable = TableRead.Instance.ReadTable<taskType>("taskType");
            _attrackInvestmentTable = TableRead.Instance.ReadTable<attrackInvestment>("attrackInvestment");
            _attrackInvestmentTypeTable = TableRead.Instance.ReadTable<attrackInvestmentType>("attrackInvestmentType");
            _attrackInvestmentUnlockTable = TableRead.Instance.ReadTable<attrackInvestmentUnlock>("attrackInvestmentUnlock");
            _attrackInvestmentCoeTable = TableRead.Instance.ReadTable<attrackInvestmentCoe>("attrackInvestmentCoe");
            _directTrafficTable = TableRead.Instance.ReadTable<directTraffic>("directTraffic");
            _tollCollectorTable = TableRead.Instance.ReadTable<tollCollector>("tollCollector");
            _prefabUITable = TableRead.Instance.ReadTable<prefabUI>("prefabUI");
            _changeImgaTable = TableRead.Instance.ReadTable<changeImga>("changeImga");
            _changeframeTable = TableRead.Instance.ReadTable<changeframe>("changeframe");
            _changeTableTable = TableRead.Instance.ReadTable<changeTable>("changeTable");
    }//=======代码自动生成请勿修改=======
}
