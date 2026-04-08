using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameObjectPool
{
    private GameObject one;
    private List<GameObject> Lst;
    public GameObjectPool(string path)
    {
        AssetLoadOld.Instance.LoadPrefabResources(path, (go) => {
            one = go;
        });
        Lst = new List<GameObject>();
    }
    public GameObject GetOne() {
        GameObject go = null;
        if (Lst.Count == 0)
        {
            go = GameObject.Instantiate(one);
        }
        else
        {
            go = Lst[0];
            Lst.RemoveAt(0);
        }
        if (go != null)
        {
            return go;
        }
        return GetOne();
    }
    public void RecOne(GameObject go) {
        //GameObject.Destroy(go);
        go.SetActive(false);
        Lst.Add(go);
    }
    public void RecOneFire(GameObject go)
    {
        go.transform.DisAlph(0.04f, () => {
           var can =  go.transform.GetOrAddComponent<CanvasGroup>();
            can.alpha = 1;
            go.SetActive(false);
            Lst.Add(go);
        });
       
    }
}
