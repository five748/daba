using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadUIScript : MonoBehaviour {
	public string uiName = "";
	// Use this for initialization
	void Start() {
		Launch.Instance.Init();
		Launch.InitUI(transform, uiName);
	}
}
