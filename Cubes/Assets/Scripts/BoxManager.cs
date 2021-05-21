using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxManager : MonoBehaviour
{
    public static BoxManager instance;

    [SerializeField]
    private int cols = 9;

    [SerializeField]
    private float boxGap = 0.6f;

    [SerializeField]
    private int boxYLimit = 15;

    private Box box;

    public Box tile;
    
    private Box[,] tiles;
    public List<Sprite> characters = new List<Sprite>();


    [SerializeField]
    private List<Box> BoxesToClear = new List<Box>();

    public GameObject emptyPrefab;
    private Sprite emptySprite;

    public GameObject BoxSpawner;

    [SerializeField]
    private List<Box> BoxesToSpawn = new List<Box>();

    [SerializeField]
    private float boxSpawnTimeGap = 0.8f;

    [SerializeField]
    private float nextBoxSetSpawnTimeGap = 1.5f;

    public GameObject boxesParent;

    public class ColContainer
    {
        public List<Box> colList;
    }

    List<ColContainer> containerList = new List<ColContainer>();

    public bool gameOver;

    public delegate void ScoreAddedAction();
    public static event ScoreAddedAction onScoreAdded;

    private void Start()
    {
        instance = GetComponent<BoxManager>();
        emptySprite = emptyPrefab.GetComponent<SpriteRenderer>().sprite;

        SetInitBoxes();
        StartCoroutine(SetNewBoxes());
    }

    private int noOfLevels;

    private int[][][] level = new int[][][]
                    {
                     new int[][]
                            {
                                  new int[]{ 0, 1, 1, 1, 1},
                                  new int[]{ 2, 2, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3,0, 3, 1, 1, 1, 2},
                                  new int[]{ 0, 4, 1, 1, 1},
                                  new int[]{ 0, 1, 1, 1, 1,1, 2, 0, 3},
                                  new int[]{ 0, 3, 2, 1, 3, 2},
                                  new int[]{ 0, 1, 4, 1, 1,3, 0, 4, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3, 0, 2, 3, 2},
                                  new int[]{ 1, 3, 0, 1, 3, 2}
                            },

                     new int[][]
                            {
                                  new int[]{ 0, 1, 1, 1, 1},
                                  new int[] { 2, 2, 1, 3 },
                                  new int[] { 4, 0, 3, 1, 3, 0, 3, 1},
                                  new int[] { 0, 1, 2, 1 },
                                  new int[] { 0, 4, 1, 3},
                                  new int[] { 0, 3, 1, 1, 3, 2 },
                                  new int[] { 0, 1, 1, 2, 1, 3, 0, 0, 1, 3 },
                                  new int[] { 3, 0, 0, 1, 2 },
                                  new int[] { 1, 3, 2, 1, 3, 2 }
                            },

                     new int[][]
                            {
                                  new int[] { 0, 1, 1, 3, 3 },
                                  new int[] { 2, 2, 1, 3 },
                                  new int[] { 3, 0, 0, 1, 2, 1, 1, 2 },
                                  new int[] { 0, 1, 4, 0, 1 },
                                  new int[] { 0, 1, 2, 1, 1, 1, 2, 0, 3 },
                                  new int[] { 0, 3, 2, 1, 3, 2 },
                                  new int[] { 0, 4, 3,2 },
                                  new int[] { 3, 0, 0, 1, 1, 0, 2},
                                  new int[] { 1, 3, 0, 1, 4, 2}
                            }

                    };


private void SetInitBoxes()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;
       
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        noOfLevels = level.Length;
        int levelIndex = Random.Range(0, noOfLevels);

        for (int i = 0; i < cols; i++)
        {
            ColContainer c = new ColContainer();
            c.colList = new List<Box>();

            for (int j = 0; j < level[levelIndex][i].Length; j++)
            {
                Box newTile = Instantiate(tile, new Vector3(startX + (boxGap * i), startY + (boxGap * j), 0), tile.transform.rotation);
                newTile.GetComponent<BoxCollider2D>().enabled = true;

                newTile.transform.parent = boxesParent.transform; // Add this line
        
                Sprite newSprite = possibleCharacters[level[levelIndex][i][j]];
                newTile.GetComponent<SpriteRenderer>().sprite = newSprite;

                c.colList.Add(newTile);
            }

            containerList.Add(c);

        }
    }

    public void UpdateBoxesToClear(Box box)
    {
        if (!BoxesToClear.Contains(box))
        {
            UpdateScore();
            BoxesToClear.Add(box);
        }
    }

    public void ClearBoxes()
    {
        foreach (Box box in BoxesToClear)
        {
            box.GetComponent<SpriteRenderer>().sprite = null;
            box.boxState = 0;
        }

        BoxesToClear.Clear();

        //StopCoroutine(FindNullTiles()); //Add this line
        StartCoroutine(FindNullTiles1()); //Add this line
    }

    public int points;
    public void UpdateScore()
    {
        points++;

        if (onScoreAdded != null)
            onScoreAdded();
    }


    //public IEnumerator FindNullTiles()
    //{
    //    for(int x = 0; x < xSize; x++)
    //    {
    //        for(int y = ySize - 1; y >= 0; y--)
    //        {
    //            if(tiles[x, y].GetComponent<SpriteRenderer>().sprite == null)
    //            {
    //                yield return StartCoroutine(ShiftTilesDown(x, y));
    //                break;
    //            }
    //        }
    //    }

    //}

    //private IEnumerator ShiftTilesDown(int x, int yStart, float shiftDelay = 0.05f)
    //{
    //    //IsShifting = true;

    //    List<SpriteRenderer> renders = new List<SpriteRenderer>();

    //    int nullCount = 0;

    //    for (int y = yStart; y >= 0; y--) //ysize
    //    {
    //        SpriteRenderer render = tiles[x, y].GetComponent<SpriteRenderer>();

    //        if (render.sprite == null)
    //        {
    //            nullCount++;
    //        }

    //        renders.Add(render);
    //    }

    //    for (int i = 0; i < nullCount; i++)
    //    {
    //        //GUIManager.instance.Score += 50; // Add this line here

    //        yield return new WaitForSeconds(shiftDelay);

    //        for (int k = 0; k < renders.Count - 1; k++)
    //        {
    //            renders[k].sprite = renders[k + 1].sprite;
    //            renders[k + 1].sprite = emptySprite; //GetNewSprite(x, ySize - 1);
    //        }
    //    }

    //    //IsShifting = false;
    //}


    public IEnumerator FindNullTiles1()    //containerList[j].colList
    {
        for (int x = 0; x < cols; x++)
        {
            for(int y = 0; y < containerList[x].colList.Count; y++)
            {
                if(containerList[x].colList[y].GetComponent<SpriteRenderer>().sprite == null)
                {
                    yield return StartCoroutine(ShiftTilesDown1(x, y));
                    break;
                }
            }
        }

    }

    private IEnumerator ShiftTilesDown1(int x, int yStart, float shiftDelay = 0.05f)
    {
        //IsShifting = true;

        List<SpriteRenderer> renders = new List<SpriteRenderer>();

        int nullCount = 0;

        for(int y = yStart; y < containerList[x].colList.Count ; y++) //ysize //
        {
            SpriteRenderer render = containerList[x].colList[y].GetComponent<SpriteRenderer>();

            if (render.sprite == null)
            {
                nullCount++;
            }

            //renders.Add(render);
        }

        for(int y = yStart; y < containerList[x].colList.Count; y++)
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
                if(renders[k]!= null && renders[k].GetComponent<SpriteRenderer>()!=null && renders[k+1] != null && renders[k+1].GetComponent<SpriteRenderer>() != null)
                {
                    renders[k].sprite = renders[k + 1].sprite;
                    renders[k + 1].sprite = emptySprite; //GetNewSprite(x, ySize - 1);
                }
            }
        }

        //IsShifting = false;
    }


    public IEnumerator SetNewBoxes()
    {
        float startX = BoxSpawner.transform.position.x;
        float startY = BoxSpawner.transform.position.y;

        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        for(int x = 0; x < cols; x++)
        {
            Box newBox = Instantiate(tile, new Vector3(startX + (boxGap * x), startY, 0), tile.transform.rotation);

            Sprite newBoxprite = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
            newBox.GetComponent<SpriteRenderer>().sprite = newBoxprite;

            BoxesToSpawn.Add(newBox);
            newBox.transform.parent = BoxSpawner.transform; // Add this line

            yield return new WaitForSeconds(boxSpawnTimeGap);
        }

        AddNewBoxes(BoxesToSpawn);

        for(int i = 0;i< BoxSpawner.transform.childCount;i++)
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

        for(int j = 0; j < BoxesToSpawn.Count; j++)
        {
            Box newTile = Instantiate(BoxesToSpawn[j], new Vector3(startX + (boxGap * j), startY, 0), tile.transform.rotation);
            newTile.GetComponent<BoxCollider2D>().enabled = true;

            newTile.transform.parent = boxesParent.transform; // Add this line

            containerList[j].colList.Insert(0,newTile);

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

    public void GameFinished(int gamefinish)
    {
        gameOver = true;
        StopAllCoroutines();

        GameObject gameOverObj = GameObject.Find("GameComplete");

        for(int i = 0; i < gameOverObj.transform.childCount; i++)
        {
            gameOverObj.transform.GetChild(i).gameObject.SetActive(true);
        }

        gameOverObj.GetComponent<GameOver>().InitGameOver(gamefinish);
    }

    private void Reset()
    {
        gameOver = false;
    }

}
