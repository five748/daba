using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 序列帧播放
/// </summary>
public class FrameEffect : MonoBehaviour {

	private RawImage rawImage;
	public bool IsBack = false;
	public int width;
	public int heigh;
	public float oneFrameTime = 0.1f;
	private float widthFloat = 0;
	private float heighFloat = 0;
	private float maxWidthFloat = 0;
	private float maxHeighFloat = 0;
	private int maxCount = 0;
	// Use this for initialization
	void Start () {
		rawImage = GetComponent<RawImage>();
		widthFloat = 1.0f / width;
		heighFloat = 1.0f / heigh;
		maxWidthFloat = 1 - widthFloat;
		maxHeighFloat = 1 - heighFloat;
		maxCount = width * heigh;
		StartCoroutine(Begin());
	}
	int index = 0;
	private IEnumerator Begin() {
		index = 0;
		while(true) {
			float x = widthFloat * (index % width);
			float y = maxHeighFloat - heighFloat * Mathf.FloorToInt(1.0f * index/ width);
			if(IsBack)
			{
				x = maxWidthFloat - x;
				y = maxHeighFloat - y;
			}
			//Debug.LogError(x + ":" + y + ":" + index);
			rawImage.uvRect = new Rect(x, y, widthFloat, heighFloat);
			index++;
			if(index >= maxCount - 1) {
				index = 0;
			}
			yield return new WaitForSeconds(oneFrameTime);
		}
	}
}
