using UnityEngine;
using System.Collections;

public class ItCastPerson : MonoBehaviour
{
    public Sprite[] Persons=new Sprite[6];

    private SpriteRenderer sr1;
    private int level = 0;

    void Start()
    {
        sr1 = GetComponent<SpriteRenderer>();
    }

    void UpdateLevel()
    {
        level++;
        //修改当前对象的名称
        name = "p" + level;
        //修改显示效果，即精灵
        sr1.sprite = Persons[level];

        if (level >= 5)
        {
            ItCastHelper.IsWin = true;
        }
    }


}
