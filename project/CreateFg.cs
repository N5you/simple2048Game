using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CreateFg : MonoBehaviour
{
    [SerializeField] private SpriteManager spriteManager = new SpriteManager(); //2048管理器

    private void Start()
    {
        for (int x = 0; x < 4; x++)
        {
            for (int y = 0; y < 4; y++)
            {
                spriteManager.GetCard(new Vector3(ItCastHelper.XStartPos + x * ItCastHelper.XOffset, ItCastHelper.YStartPos - y * ItCastHelper.YOffset, 0)); //生成卡片的同时设置位置
                //fg.name = "fg"+x+"_"+y;
            }
        }
    }
}

[System.Serializable]
public sealed class SpriteManager //这个是2018卡片管理器（精灵管理器）
{
    [SerializeField] [Header("精灵的背景")] [Tooltip("2048卡片背景")] private Sprite spriteBg; //
    [SerializeField] private float spriteBgWide = 2f; //宽
    [SerializeField] private float spriteBgHigh = 2f; //高

    public GameObject GetCard(Vector3 position) //获取卡片
    {
        GameObject go = SpriteGenerate(position);
        return go;
    }

    private GameObject SpriteGenerate(Vector3 position) //生成
    {
        GameObject go = new GameObject(position.ToString());
        SpriteRenderer spriteRenderer = go.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = spriteBg;
        go.transform.localScale = new Vector3(spriteBgWide, spriteBgHigh, 1);
        go.transform.position = position;
        return go; 
    }
}