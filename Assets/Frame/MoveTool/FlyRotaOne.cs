using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyRotaOne : MonoBehaviour
{
    public float speed = 1000;
    public float maxDis = 50;
    public Transform flyTo;
    // Start is called before the first frame update
    public void BeginMove(System.Action moveOver)
    {
        var one = transform.CopyToTargetOne(transform.parent);
        transform.SetActive(false);
        MonoTool.Instance.StartCor(FlyOne(one, flyTo, moveOver));
    }
    private IEnumerator FlyOne(Transform fly, Transform to, System.Action moveOver)
    {
        float dis = 0;
        float rotaSpeed = 5;
        while (true)
        {
            dis = Vector2.Distance(fly.position, to.position);
            if (rotaSpeed < 300)
            {
                rotaSpeed += 1f;
            }
            if (dis < maxDis)
            {
                GameObject.Destroy(fly.gameObject);
                moveOver();
                yield break;
            }
            fly.position += fly.up * speed * Time.deltaTime;
            var angle = fly.GetPointAngle(to, fly);
            fly.Rotate(new Vector3(0, 0, angle * 0.002f * rotaSpeed));
            yield return null;
        }
    }
}
