using System;
using UnityEngine;
using UnityEngine.UI;

public class SpriteState : MonoBehaviour
{

    public Image image = null;

    public Sprite[] sprites = null;
    public string[] names = null;

    public int defualtIndex = 0;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        defualtIndex = -1;
    }

    private void OnEnable()
    {
        // UpdateSprite(defualtIndex);
    }


    public void UpdateSprite(int index)
    {
        if (defualtIndex == index)
        {
            return;
        }
        if (sprites.Length <= index || index < 0)
        {
            Debug.LogError("图片不存在");
            return;
        }

        defualtIndex = index;
        image.sprite = sprites[index];
        image.SetNativeSize();
    }

    public void UpdateSpriteByName(string name)
    {
        if (names == null)
        {
            return;
        }
        int index = Array.IndexOf(names, name);
        if (index >= 0)
        {
            UpdateSprite(index);
        }
    }

    public void SetSprite(int index)
    {
        if (sprites.Length <= index || index < 0)
        {
            Debug.LogError("图片不存在");
            return;
        }

        defualtIndex = index;
        image.sprite = sprites[index];
        image.SetNativeSize();
    }
}