using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    [SerializeField]
    private int cols = 9;

    [SerializeField]
    private float boxGap = 0.6f;

    [SerializeField]
    private int boxYLimit = 15;

    [SerializeField]
    private Box box;

    [SerializeField]
    private List<Sprite> characters = new List<Sprite>();

    [SerializeField]
    private List<Box> BoxesToClear = new List<Box>();

    [SerializeField]
    private GameObject emptyPrefab;

    public Sprite emptySprite { get; private set; }

    [SerializeField]
    private GameObject BoxSpawner;

    private List<Box> BoxesToSpawn = new List<Box>();

    [SerializeField]
    private float boxSpawnTimeGap = 0.8f;

    [SerializeField]
    private float nextBoxSetSpawnTimeGap = 1.5f;

    [SerializeField]
    private GameObject boxesParent;

    [SerializeField]
    private ColContainer columnPrefab;

    private List<ColContainer> containerList = new List<ColContainer>();

    public bool gameOver;
    private bool moreThan2Cluster = false;

    public event Action<int> addScore;

    public int points { get; private set; }

    [Header("Floating Score")]
    [SerializeField]
    private FloatingScore floatingScorePrefab;
    [SerializeField]
    private int decreasingOnWrongMove = 2;

    [Header("Good Text")]
    [SerializeField]
    private int goodTextShowCount = 5;
    public event Action showGoodText;

    private void Awake()
    {
        instance = GetComponent<BoxManager>();
    }

    private void Start()
    {
        emptySprite = emptyPrefab.GetComponent<SpriteRenderer>().sprite;

        SetInitBoxes();

        SoundManager.PlayLoopSound(SoundManager.Sound.GameBG);

        StartCoroutine(SetNewBoxes());
    }

    private int noOfLevels;

    private int[][][] level = new int[][][]
                    {
                     new int[][]
                            {
                                  new int[]{ 0, 1, 1, 1, 1},
                                  new int[]{ 2, 2, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3,0, 3, 1, 1, 1, 2},    //0,1,2,3,4,5
                                  new int[]{ 0, 4, 5, 1, 1},
                                  new int[]{ 0, 1, 1, 1, 1,1, 2, 0, 3},
                                  new int[]{ 0, 3, 2, 1, 3, 2},
                                  new int[]{ 0, 5, 4, 5, 1,3, 0, 4, 1, 3},
                                  new int[]{ 3, 0, 0, 5, 3, 0, 2, 3, 2},
                                  new int[]{ 1, 3, 0, 1, 3, 2}
                            },

                     new int[][]
                            {
                                  new int[]{ 5, 3, 3, 3},
                                  new int[] { 4, 4, 3, 3,5 },
                                  new int[] { 4, 3, 3, 6,},          ////3,4,5,6,7,8
                                  new int[] { 6, 5, 6, 7, 5},
                                  new int[] { 6, 4, 8, 3},
                                  new int[] { 7, 3, 8, 6, 6, 7, 5,4 },
                                  new int[] { 8, 5, 8, 4, 5},
                                  new int[] { 3, 6, 5, 3, 6 },
                                  new int[] { 5, 3, 5}
                            },

                     new int[][]
                            {
                                  new int[] { 5, 5, 6, 7, 7 }, 
                                  new int[] { 10, 6, 5, 4, 6, 7 },  ////5,6,7,8,9,10
                                  new int[] { 10, 9, 9, 5, 6, 6 },
                                  new int[] { 9, 7, 5, 10, 7, 8, 6, 5 },
                                  new int[] { 9, 5, 5, 10, 10, 5},
                                  new int[] { 9, 6, 8, 7, 8, 7 },
                                  new int[] { 10, 5, 8, 6},
                                  new int[] { 3, 5, 5, 6, 5, 7, 10},
                                  new int[] { 7, 6, 5, 6, 7, 7}
                            },

                     new int[][]
                            {
                                  new int[] { 11, 11, 6, 7, 7 },
                                  new int[] { 10, 6, 10, 6, 7 },  ////6,7,8,9,10,11
                                  new int[] { 10, 9, 9, 7, 6, 6 },
                                  new int[] { 9, 7},
                                  new int[] { 9, 11, 11, 10},
                                  new int[] { 9, 6, 8, 7, 8, 7 },
                                  new int[] { 10, 6, 8, 6},
                                  new int[] { 6, 6, 8, 6, 11,10},
                                  new int[] { 9, 6, 7, 7}
                            }

                    };


    private int[][] chooseFrom = new int[][]
                                    {
                                        new int[] { 0, 1, 2, 3, 4, 5 },
                                        new int[] { 3, 4, 5, 6, 7, 8 },
                                        new int[] { 5, 6, 7, 8, 9, 10 },
                                        new int[] { 6, 7, 8, 9, 10, 11}
                                    };

    private int levelIndex;

    private void SetInitBoxes()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;
       
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        noOfLevels = level.Length;
        levelIndex = UnityEngine.Random.Range(0, noOfLevels);

        for (int i = 0; i < cols; i++)
        {
            ColContainer colObj = Instantiate(columnPrefab); //objs
            colObj.transform.parent = boxesParent.transform;

            colObj.transform.position = new Vector3(startX + (boxGap * i), boxesParent.transform.position.y);

            //ColContainer c = new ColContainer();
            colObj.colList = new List<Box>();

            for (int j = 0; j < level[levelIndex][i].Length; j++)
            {
                Box newTile = Instantiate(box, new Vector3(colObj.transform.position.x, startY + (boxGap * j), 0), box.transform.rotation);
                newTile.GetComponent<BoxCollider2D>().enabled = true;

                newTile.transform.parent = colObj.transform;

                Sprite newSprite = possibleCharacters[level[levelIndex][i][j]];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                colObj.colList.Add(newTile);
            }

            containerList.Add(colObj);
        }
    }

    public void UpdateBoxesToClear(Box box)
    {
        if (!BoxesToClear.Contains(box))
        {
            //UpdateScore(true);
            BoxesToClear.Add(box);
        }
    }

    public IEnumerator ClearBoxes()
    {
        int boxesBursted = BoxesToClear.Count;
        Box b = BoxesToClear[0];

        if (moreThan2Cluster)
        {
            if(BoxesToClear.Count > 2)
            {
                foreach (Box box in BoxesToClear)
                {
                    box.GetComponent<SpriteRenderer>().sprite = null;
                    box.boxState = 0;
                }

                SoundManager.PlaySound(SoundManager.Sound.Burst);
                BoxesToClear.Clear();

                StartCoroutine(FindNullBoxes()); //Add this line
            }
            else
            {
                foreach (Box box in BoxesToClear)
                    box.boxState = 0;

                SoundManager.PlaySound(SoundManager.Sound.WrongMove);
                BoxesToClear.Clear();

                //Debug.Log("Double click negative");
            }
        }
        else
        {
            foreach (Box box in BoxesToClear)
            {
                box.GetComponent<SpriteRenderer>().sprite = null;
                box.boxState = 0;

                BreakBox(box);
                
                yield return new WaitForSeconds(0.02f);
            }

            SoundManager.PlaySound(SoundManager.Sound.Burst);
            BoxesToClear.Clear();

            //Debug.Log("1");
            StartCoroutine(FindNullBoxes()); //Add this line
            
        }

        UpdateScore(true, boxesBursted,b);

        //Debug.Log("2");
        //checkIfAnyColEmpty();
    }

    private void BreakBox(Box box)
    {
        ParticleSystem particle = Instantiate(box.particles);

        particle.transform.parent = box.transform;
        particle.transform.localPosition = new Vector3(0, 0, -1);
        particle.transform.localScale = new Vector3(1, 1, 1);

        particle.Play();
    }

    public IEnumerator FindNullBoxes()    //containerList[j].colList
    {
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < containerList[x].colList.Count; y++)
            {
                if (containerList[x].colList[y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftBoxesDown(x, y));

                    break;
                }
            }
        }

    }

    private IEnumerator ShiftBoxesDown(int x, int yStart, float shiftDelay = 0.05f)
    {
        //IsShifting = true;

        List<SpriteRenderer> renders = new List<SpriteRenderer>();

        int nullCount = 0;

        for (int y = yStart; y < containerList[x].colList.Count; y++) //ysize //
        {
            SpriteRenderer render = containerList[x].colList[y].GetComponent<SpriteRenderer>();

            if (render.sprite == null)
            {
                nullCount++;
            }

            //renders.Add(render);
        }

        for (int y = yStart; y < containerList[x].colList.Count; y++)
        {
            SpriteRenderer render = containerList[x].colList[y].GetComponent<SpriteRenderer>();
            renders.Add(render);
        }

        //Debug.Log("nullCount" + nullCount + " r " + renders.Count);

        for (int i = 0; i < nullCount; i++)
        {
            //GUIManager.instance.Score += 50; // Add this line here

            yield return new WaitForSeconds(shiftDelay);

            for (int k = 0; k < renders.Count - 1; k++)
            {
                if (renders[k] != null && renders[k].GetComponent<SpriteRenderer>() != null && renders[k + 1] != null && renders[k + 1].GetComponent<SpriteRenderer>() != null)
                {
                    renders[k].sprite = renders[k + 1].sprite;
                    renders[k + 1].sprite = emptySprite; //GetNewSprite(x, ySize - 1);
                }
            }

            if(i == nullCount - 1)  //for last
            {
                checkIfAnyColEmpty();
            }
        }

        //Debug.Log("3");
        //IsShifting = false;
    }


    public IEnumerator SetNewBoxes()
    {
        float startX = BoxSpawner.transform.position.x;
        float startY = BoxSpawner.transform.position.y;

        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        for (int x = 0; x < cols; x++)
        {
            Box newBox = Instantiate(box, new Vector3(startX + (boxGap * x), startY, 0), box.transform.rotation);

            //Sprite newBoxprite = possibleCharacters[UnityEngine.Random.Range(0, /*possibleCharacters.Count*/(3+1))]; //cheat
            
            int[] nextSet = chooseFrom[levelIndex];
            int nextBox = nextSet[UnityEngine.Random.Range(0, nextSet.Length)];

            Sprite newBoxprite = possibleCharacters[nextBox];
            newBox.GetComponent<SpriteRenderer>().sprite = newBoxprite;

            BoxesToSpawn.Add(newBox);
            newBox.transform.parent = BoxSpawner.transform; // Add this line

            yield return new WaitForSeconds(boxSpawnTimeGap);
        }

        AddNewBoxes(BoxesToSpawn);

        for (int i = 0; i < BoxSpawner.transform.childCount; i++)
        {
            GameObject.Destroy(BoxSpawner.transform.GetChild(i).gameObject);
        }

        BoxesToSpawn.Clear();

        yield return new WaitForSeconds(nextBoxSetSpawnTimeGap);

        StopCoroutine(SetNewBoxes()); ///

        StartCoroutine(SetNewBoxes());

    }

    public void AddNewBoxes(List<Box> BoxesToSpawn)
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        boxesParent.transform.position = boxesParent.transform.position + new Vector3(0, boxGap, 0);

        for (int j = 0; j < BoxesToSpawn.Count; j++)
        {
            Box newTile = Instantiate(BoxesToSpawn[j], new Vector3(startX + (boxGap * j), startY, 0), box.transform.rotation);
            newTile.GetComponent<BoxCollider2D>().enabled = true;

            newTile.transform.parent = containerList[j].transform; // Add this line

            containerList[j].colList.Insert(0, newTile);

            if(containerList[j].colList.Count > boxYLimit)
            {
                if (!containerList[j].colList[containerList[j].colList.Count - 1].GetComponent<SpriteRenderer>().sprite.name.Equals("emptyPix"))
                {
                    GameFinished(2);
                    return;
                }

                GameObject.Destroy(containerList[j].colList[containerList[j].colList.Count - 1].gameObject);
                containerList[j].colList.RemoveAt(containerList[j].colList.Count - 1);

            }

        }

    }


    public void checkIfAnyColEmpty()
    {
        float startX = transform.position.x;
        List<int> emptyCols = new List<int>();

        for (int i=0; i<containerList.Count; i++)
        {
            int count = 0;
            for(int j = 0; j< containerList[i].colList.Count; j++)
            {
                Sprite sprite = containerList[i].colList[j].GetComponent<SpriteRenderer>().sprite;

                if (sprite != null && !sprite.name.Equals("emptyPix"))
                    break;

                count++;

                if(count == containerList[i].colList.Count)
                    emptyCols.Add(i);
            }
        }

        List<ColContainer> TempContainerList = new List<ColContainer>();
        TempContainerList = containerList;

        for (int i = 0; i< emptyCols.Count ;i++)
        {
            ColContainer temp = containerList[emptyCols[i]];
            containerList.RemoveAt(emptyCols[i]);

            if(emptyCols[i] > cols/2)
            {
                containerList.Insert(cols-1, temp);
            }
            else
                containerList.Insert(0, temp);
        }

        for(int i = 0; i < cols; i++)  //update position
            containerList[i].transform.position = new Vector3(startX + (boxGap * i), boxesParent.transform.position.y);

    }

    //2  = 2
    //3 = 3 * 2 = 6
    //4 = 4 * 3 = 12
    //5 = 5 * 4 = 20
    //6 = 6 * 5 = 30

    public void UpdateScore(bool increaseScore, int boxCount,Box box = null)
    {
        int newpoints = 0;
        if(increaseScore)  //increasing
        {
            newpoints = (boxCount * (boxCount - 1));
            points = points + newpoints;
        }
        else
            points = points - decreasingOnWrongMove; //decreasing

        FloatingScore floatingScore = null;

        if (box != null)  //increase
        {
            floatingScore = Instantiate(floatingScorePrefab);
            floatingScore.transform.parent = box.transform;
            floatingScore.transform.localPosition = new Vector3(0, 0, 0);

            if(increaseScore)
                floatingScore.newPoints = newpoints;
            else
                floatingScore.newPoints = decreasingOnWrongMove;
        }

        floatingScore.UpdateFloatingScore(floatingScore.newPoints, increaseScore);

        if (points < 0) points = 0;

        addScore?.Invoke(points);

        if(boxCount >= goodTextShowCount)
            showGoodText?.Invoke();
    }

    public void GameFinished(int gamefinish)
    {
        SoundManager.StopLoopSound();

        if (gamefinish == 2)
            SoundManager.PlaySound(SoundManager.Sound.Die);

        gameOver = true;
        StopAllCoroutines();

        GameObject gameOverObj = GameObject.Find("GameComplete");

        for(int i = 0; i < gameOverObj.transform.childCount; i++)
        {
            gameOverObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        gameOverObj.GetComponent<GameOver>().InitGameOver(gamefinish,points);
    }

    private void Reset()
    {
        gameOver = false;
    }

}
