using System.Security.Cryptography;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItCastManger : MonoBehaviour
{
    [SerializeField] private Texture2D Win;
    [SerializeField] private Sprite[] sprites;

    private Person[,] PersonList = new Person[4, 4]; //位置
    private Person person; //当前对象
    private Person personBefore; //

    private void Start()
    {
        CreatePerson();
        CreatePerson();
    }

    private void CreatePerson() //生成游戏对象
    {
        int x1 = -1, y1 = -1; //随机生成位置
        do
        {
            x1 = Random.Range(0, 4);
            y1 = Random.Range(0, 4);
        } while (PersonList[x1, y1] != null);
        Person p = GameObjectManager.GetPerson();
        p.SetPosition(new Vector3(ItCastHelper.XStartPos + x1 * ItCastHelper.XOffset, ItCastHelper.YStartPos - y1 * ItCastHelper.YOffset, -1));
        p.UpdateLevel(sprites.Length, sprites[0]);
        PersonList[x1, y1] = p; //记录当前位置存在了游戏对象
    }

    void Update()
    {
        if (ItCastHelper.IsWin)
        {
            return; //如果胜利，不需要再响应用户输入
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            #region 向上移动
            //对于第0行不需要移动，已经是最上行了
            for (int y = 1; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    person = PersonList[x, y];
                    if (person == null)
                    {
                        continue; //如果当前位置没有对象，则不需要进行任何操作
                    }
                    //如果当前位置有对象，则执行以下代码
                    //判断当前位置的前面所有位置是否有对象
                    int destPos = -1;
                    for (int y1 = y - 1; y1 >= 0; y1--)
                    {
                        personBefore = PersonList[x, y1];
                        if (personBefore != null)
                        {
                            if (person.IsEquals(personBefore)) //如果有，则不再向前找
                            {
                                destPos = -1; //如果是相同的对象则合并，不再需要移动当前对象
                                person.Abolition();
                                PersonList[x, y] = null;
                                personBefore.UpdateLevel(sprites.Length, sprites[personBefore.ID]);  //personBefore对象更新等级（ID）
                            }
                            break;
                        }
                        else
                        {
                            destPos = y1; //如果没有，则继续向前找
                        }
                    }
                    if (destPos > -1) //完成移动
                    {
                        //1、更新位置矩阵信息
                        PersonList[x, y] = null;
                        PersonList[x, destPos] = person;
                        person.SetPosition(new Vector3(0, (y - destPos) * ItCastHelper.YOffset, 0)); //2、更改当前游戏对象的位置
                    }
                }
            }
            //新生成游戏对象
            CreatePerson();
            #endregion
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            #region 向下移动
            for (int y = 2; y >= 0; y--) //对于第3行不需要移动，已经是最下行了
            {
                for (int x = 0; x < 4; x++)
                {
                    person = PersonList[x, y];
                    if (person == null)
                    {
                        continue; //如果当前位置没有对象，则不需要进行任何操作
                    }
                    //如果当前位置有对象，则执行以下代码
                    //判断当前位置的前面所有位置是否有对象
                    int destPos = -1;
                    for (int y1 = y + 1; y1 < 4; y1++)
                    {
                        personBefore = PersonList[x, y1];
                        if (personBefore != null)
                        {
                            if (person.IsEquals(personBefore)) //如果有，则不再向前找
                            {
                                //如果是相同的对象则合并，不再需要移动当前对象
                                destPos = -1;
                                person.Abolition();
                                PersonList[x, y] = null;
                                personBefore.UpdateLevel(sprites.Length, sprites[personBefore.ID]);  //personBefore对象更新等级（ID）
                            }
                            break;
                        }
                        else
                        {
                            destPos = y1; //如果没有，则继续向前找
                        }
                    }
                    if (destPos > -1) //完成移动
                    {
                        PersonList[x, y] = null; //1、更新位置矩阵信息
                        PersonList[x, destPos] = person;
                        person.SetPosition(new Vector3(0, (y - destPos) * ItCastHelper.YOffset, 0)); //2、更改当前游戏对象的位置
                    }
                }
            }
            CreatePerson(); //新生成游戏对象
            #endregion

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            #region 向左移动
            for (int x = 1; x < 4; x++) //对于第0列不需要移动，已经是最左行了
            {
                for (int y = 0; y < 4; y++)
                {
                    person = PersonList[x, y];
                    if (person == null)
                    { 
                        continue; //如果当前位置没有对象，则不需要进行任何操作
                    }
                    //如果当前位置有对象，则执行以下代码
                    //判断当前位置的前面所有位置是否有对象
                    int destPos = -1;
                    for (int x1 = x - 1; x1 >= 0; x1--)
                    {
                        personBefore = PersonList[x1, y];
                        if (personBefore != null)
                        {
                            if (person.IsEquals(personBefore)) //如果有，则不再向前找
                            {
                                destPos = -1; //如果是相同的对象则合并，不再需要移动当前对象
                                person.Abolition(); //消毁
                                PersonList[x, y] = null;
                                personBefore.UpdateLevel(sprites.Length, sprites[personBefore.ID]);  //personBefore对象更新等级（ID）
                            }
                            break;
                        }
                        else
                        {
                            //如果没有，则继续向前找
                            destPos = x1;
                        }
                    }
                    if (destPos > -1) //完成移动
                    {
                        PersonList[x, y] = null; //1、更新位置矩阵信息
                        PersonList[destPos, y] = person;
                        person.SetPosition(new Vector3((x-destPos)*ItCastHelper.XOffset, 0, 0)); //2、更改当前游戏对象的位置
                    }
                }
            }
            CreatePerson(); //新生成游戏对象
            #endregion

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            #region 向右移动
            for (int x = 2; x >=0; x--) //对于第4列不需要移动，已经是最右行了
            {
                for (int y = 0; y < 4; y++)
                {
                    person = PersonList[x, y];
                    if (person == null)
                    {
                        continue; //如果当前位置没有对象，则不需要进行任何操作
                    }
                    //如果当前位置有对象，则执行以下代码
                    //判断当前位置的前面所有位置是否有对象
                    int destPos = -1;
                    for (int x1 = x + 1; x1 <4; x1++)
                    {
                        personBefore = PersonList[x1, y];
                        if (personBefore != null)
                        {
                            if (person.IsEquals(personBefore)) //如果有，则不再向前找
                            {
                                destPos = -1; //如果是相同的对象则合并，不再需要移动当前对象
                                person.Abolition();
                                PersonList[x, y] = null;
                                personBefore.UpdateLevel(sprites.Length, sprites[personBefore.ID]);  //personBefore对象更新等级（ID）
                            }
                            break;
                        }
                        else
                        {
                            destPos = x1; // 如果没有，则继续向前找
                        }
                    }
                    if (destPos > -1) //完成移动
                    {
                        PersonList[x, y] = null; //1、更新位置矩阵信息
                        PersonList[destPos, y] = person;
                        Vector3 pos = new Vector3((x - destPos) * ItCastHelper.XOffset, 0, 0);
                        person.SetPosition(pos); //2、更改当前游戏对象的位置
                    }
                }
            }
            CreatePerson(); //新生成游戏对象
            #endregion
        }
    }

    void OnGUI()
    {
        if (ItCastHelper.IsWin)
        {
            GUI.DrawTexture(new Rect(
                (Screen.width - Win.width/3f)/2f,
                (Screen.height - Win.height/3f)/2f,
                Win.width/3f,
                Win.height/3f
                ), Win);
        }
    }
}

[System.Serializable]
public class GameObjectManager //游戏对象管理器
{
    private static List<Person> pool = new List<Person>(); //对象池

    public static Person GetPerson() //获取卡片
    {
        return GameObjectManager.NeedPerson();
    }

    public static Person NeedPerson() //需要卡片
    {
        Person person = pool.Find(o => { return o.GetActivation().Equals(false); });
        if (person == null) //没有空闲，需要创建
        {
            person = new Person();
            pool.Add(person);
        }
        else
        {
            person.Activation();
        }
        return person;
    }
}

public class Person //
{
    public Person() //
    {
        gameObj = new GameObject();
        spriteRenderer = gameObj.AddComponent<SpriteRenderer>();
        isActivation = true;
    }

    private GameObject gameObj = null;
    private bool isActivation = false; //是激活的吗
    private SpriteRenderer spriteRenderer = null;

    public int ID;
//  {
//      get { return ID; }
//      private set { ID = value; }
//  }

    public bool GetActivation() //获取激活状态
    {
        return isActivation;
    }

    public void Abolition() //废除
    {
        gameObj.SetActive(false);
        isActivation = false;
    }

    public void Activation() //激活
    {
        isActivation = true;
        gameObj.SetActive(true);
    }

    public void SetPosition(Vector3 pos) //设置位置
    {
        gameObj.transform.position = pos;
    }

    public bool IsEquals(Person per) //判断是否相同
    {
        return this.ID.Equals(per.ID);
    }

    public void UpdateLevel(int maxIndex, Sprite person)
    {
        ID++;
        spriteRenderer.sprite = person; //修改显示效果，即精灵
        if (ID >= maxIndex)
        {
            ItCastHelper.IsWin = true;
        }
    }
}
