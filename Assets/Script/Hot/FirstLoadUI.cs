using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoadUI
{
    public static void Load() {
        UIManager.OpenUI("UILogin", "", () => {
            var setMask = UIManager.Root.GetComponent<SetMaskImageToMainCam>();
            if (setMask != null)
            {
                setMask.Init(UIManager.CurrentGo);
            }
        });
        //var battleFather = GameObject.Find("BattleCanvas");
        //AssetLoadOld.Instance.LoadPrefab("UIPrefab/UIBattle", (go) =>
        //{
        //    var uibattle = GameObject.Instantiate(go);
        //    uibattle.transform.SetParentOverride(battleFather.transform);
        //    uibattle.GetComponent<RectTransform>().SetSiblingIndex(2);
        //    uibattle.SetActive(false);
        //    uibattle.name = "UIBattle";
        //});
        //AssetLoadOld.Instance.LoadPrefab("Other/BgCanvas", (go) =>
        //{
        //    var bgCanvas = GameObject.Instantiate(go);
        //    bgCanvas.name = "BgCanvas";
        //    //UIManager.UICamera.clearFlags = CameraClearFlags.Depth;
        //});
    }
}
