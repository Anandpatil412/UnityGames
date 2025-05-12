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
    private int cols = 8;

    public float boxGap = 0.6f;

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

    public float boxSpawnTimeGap { get; set; } // = 0.8f;
    public float nextBoxSetSpawnTimeGap { get; set; } // = 1.5f;
    public int decreaseScoreOnWrongMove { get; set; } // = 2;

    [SerializeField]
    private GameObject boxesParent;

    [SerializeField]
    private ColContainer columnPrefab;

    private List<ColContainer> containerList = new List<ColContainer>();

    public bool gameOver;
    private bool moreThan2Cluster = false;

    public event Action<int> addScore;

    public int points { get; private set; }

    public event Action updateTimer;

    [Header("Floating Score")]
    [SerializeField]
    private FloatingScore floatingScorePrefab;
  
    [Header("Good Text")]
    [SerializeField]
    private int goodTextShowCount = 4;
    public event Action showGoodText;

    private int no_of_levels;
    private int NO_OF_SETS = 10;

    [Header("UI ref")]
    [SerializeField]
    private GameObject CanvasRef;

    [SerializeField]
    private GameObject gameCompletePrefab;

    //40 levels
    //4 X 10
    //4 spawning set

    public int[][][] level = new int[][][]   
                    {
                     /////////////////////////////////////////////////Fruits 0,1,2,3,4,5
                     new int[][] //0
                            {
                                  new int[]{ 5, 1, 1,},
                                  new int[]{ 2, 2, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3,},    
                                  new int[]{ 0, 4},
                                  new int[]{ 0, 1, 5, 1, 1, 1, 2, 0, 3},
                                  new int[]{ 0, 3, 2, 1, 3, 2},
                                  new int[]{ 0, 5, 4, 5, 1, 3, 0, 4, 1, 3},
                                  new int[]{ 3, 0, 0, 5, 3, 0, 2, 3, 2},
                            },

                    new int[][] //1
                            {
                                  new int[]{ 0, 5, 1, 1},
                                  new int[]{ 2 },
                                  new int[]{ 3, 0, 0, 5, 3, 3, 2, 4, 2},
                                  new int[]{ 3, 0, 0, 1, 3},
                                  new int[]{ 0, 5, 5, 4, 1},
                                  new int[]{ 0, 1, 1, 5, 1, 1, 2},
                                  new int[]{ 0, 0, 2, 1, 3, 2},
                                  new int[]{ 0, 5, 4, 3, 3, 1, 2, 2},
                                  
                            },

                    new int[][] //2
                            {
                                  new int[]{ 0, 1},
                                  new int[]{ 2, 1, 1, 3},
                                  new int[]{ 3, 0, 1, 1, 3, 0},
                                  new int[]{ 5, 4, 5, 1, 1},
                                  new int[]{ 5, 1, 1, 4, 1, 4, 2, 0, 3},
                                  new int[]{ 0, 3},
                                  new int[]{ 3 },
                                  new int[]{ 3, 0, 0, 5, 3, 0, 5},
                            },

                    new int[][] //3
                            {
                      
                                  new int[]{ 3, 0, 0, 5, 1, 1, 2},
                                  new int[]{ 0, 4, 5, 1, 1},
                                  new int[]{ 0, 1, 1, 5, 1, 2, 4},
                                  new int[]{ 0},
                                  new int[]{ 2, 4},
                                  new int[]{ 0, 3, 2, 4, 3, 2},
                                  new int[]{ 0, 5, 4, 4, 1, 3},
                                  new int[]{ 3, 0, 0, 5, 5, 5, 2, 3, 2},
                            },

                    new int[][] //4
                            {
                                  new int[]{ 0, 2},
                                  new int[]{ 2, 2, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3, 1, 2},
                                  new int[]{ 0, 4, 5, 1, 1},
                                  new int[]{ 0, 1, 1, 2, 5, 3},
                                  new int[]{ 0, 3, 2, 5, 5, 2},
                                  new int[]{ 0, 0, 5, 3},
                                  new int[]{ 3, 0},
                            },

                     new int[][] //5
                            {
                                  new int[]{ 2, 1, 1,},
                                  new int[]{ 2, 3},
                                  new int[]{ 3, 0, 0, 1, 3,},
                                  new int[]{ 0, 4},
                                  new int[]{ 0, 3, 2, 1, 3, 2},
                                  new int[]{ 0, 5, 5, 5, 1, 3, 3, 1, 3},
                                  new int[]{ 3, 0, 0, 5, 3, 0, 2, 3, 2},
                                  new int[]{ 0, 1, 5, 5, 1}
                            },

                    new int[][] //6
                            {
                                  new int[]{ 0, 5, 1, 1},
                                  new int[]{ 2, 0, 0 },
                                  new int[]{ 3, 5, 3, 3, 2, 4, 2},
                                  new int[]{ 3, 0, 0, 0, 3},
                                  new int[]{ 0, 5, 5, 1, 1, 1},
                                  new int[]{ 0, 1, 1, 5, 1, 2},
                                  new int[]{ 0, 0, 2, 1, 3, 2},
                                  new int[]{ 0, 5, 4, 3, 3, 2, 2, 2},

                            },

                    new int[][] //7
                            {
                                  new int[]{ 0, 1},
                                  new int[]{ 2, 1, 1, 3},
                                  new int[]{ 3, 2, 1, 3, 0},
                                  new int[]{ 5, 4, 5, 1, 1},
                                  new int[]{ 5, 1, 1, 4, 2, 5, 3},
                                  new int[]{ 0, 3},
                                  new int[]{ 3 ,3},
                                  new int[]{ 3, 0, 0, 5, 5, 0, 5},
                            },

                    new int[][] //8
                            {

                                  new int[]{ 3, 0, 0, 5, 5, 5, 2, 3, 2},
                                  new int[]{ 3, 0, 0, 5, 1, 1, 2},
                                  new int[]{ 0, 4, 5, 1, 1},
                                  new int[]{ 0, 1, 1, 5, 1, 2, 4},
                                  new int[]{ 0 },
                                  new int[]{ 2, 4},
                                  new int[]{ 0, 2},
                                  new int[]{ 0, 4, 4, 4, 3},
                                  
                            },

                    new int[][] //9
                            {
                                  new int[]{ 0, 2},
                                  new int[]{ 2, 2, 1, 3},
                                  new int[]{ 3, 0, 0, 1, 3, 1, 2},
                                  new int[]{ 0, 4, 5, 3, 3},
                                  new int[]{ 0 },
                                  new int[]{ 0, 0, 5, 5, 2, 1, 1},
                                  new int[]{ 0, 0, 5, 3},
                                  new int[]{ 3, 0},
                            },

                    ////////////////////////////////////////////////////////Fruits 3,4,5,6,7,8

                    new int[][] //0
                            {
                                  new int[]{ 3, 5, 7, 5, 4, 6, 7},
                                  new int[]{ 5, 6, 8},
                                  new int[]{ 6, 6, 7, 7, 8},
                                  new int[]{ 3, 4, 8, 8, 8, 8, 3, 3},
                                  new int[]{ 7, 5, 7, 8, 7, 9, 4, 5, 3},
                                  new int[]{ 8, 4, 8},
                                  new int[]{ 7, 5, 9, 9, 8, 3},
                                  new int[]{ 8, 7, 8, 5, 3, 4, 4, 3, 5},
                            },

                    new int[][] //1
                            {
                                  new int[]{ 5, 6, 7, 6, 8, 6, 6, 3, 3},
                                  new int[]{ 6, 7, 8, 3,},
                                  new int[]{ 7, 8, 9, 3},
                                  new int[]{ 8, 8, 8, 3, 7, 6},
                                  new int[]{ 7, 6, 6, 4, 4},
                                  new int[]{ 5, 7, 3, 3, 3, 4},
                                  new int[]{ 8, 9, 9 },
                                  new int[]{ 7, 8, 6, 5, 5, 5, 7, 8, 7},
                            },

                    new int[][] //2
                            {
                                  new int[]{ 4, 4, 5, 5},
                                  new int[]{ 5 ,7},
                                  new int[]{ 6, 4, 4, 4, 9, 3},
                                  new int[]{ 7, 4, 5, 4, 9},
                                  new int[]{ 8, 4, 6, 5, 5, 3, 3},
                                  new int[]{ 3, 3, 6, 6, 5, 3, 3, 4},
                                  new int[]{ 4, 5, 8, 6, 4, 4},
                                  new int[]{ 4 },
                            },

                    new int[][] //3
                            {
                                  new int[]{ 6, 5, 7, 3, 3},
                                  new int[]{ 8, 7, 3, 5},
                                  new int[]{ 3, 7, 4, 4, 3, 5},
                                  new int[]{ 7, 8, 8, 8, 5},
                                  new int[]{ 8, 6, 8, 4, 6, 4,3},
                                  new int[]{ 9 },
                                  new int[]{ 6 },
                                  new int[]{ 3, 8, 6, 7, 3, 3},
                            },

                    new int[][] //4
                            {
                                  new int[]{ 3, 7, 6, 8},
                                  new int[]{ 3 },
                                  new int[]{ 4, 4, 3, 3, 7, 5},
                                  new int[]{ 5 },
                                  new int[]{ 5, 7, 4, 9, 9, 6},
                                  new int[]{ 6 },
                                  new int[]{ 6, 7, 7, 6, 6, 6, 3},
                                  new int[]{ 3, 7, 3, 5, 5, 3, 3},
                            },

                    new int[][] //5
                            {
                                  new int[]{ 3 },
                                  new int[]{ 5, 3, 3},
                                  new int[]{ 5, 6, 7, 7, 8 ,9 ,8},
                                  new int[]{ 3, 5, 8, 8, 8, 8, 3, 3, 5, 4},
                                  new int[]{ 7, 5, 7, 8, 7, 9, 8, 8},
                                  new int[]{ 8, 4, 5},
                                  new int[]{ 7, 5, 9, 9, 8, 8},
                                  new int[]{ 8, 7, 8, 5, 3, 4, 4, 3, 3},
                            },

                    new int[][] //6
                            {
                                  new int[]{ 5, 6, 7, 6, 8, 6, 3, 7, 8, 8, 3},
                                  new int[]{ 6, 7, 8, 3,},
                                  new int[]{ 7 },
                                  new int[]{ 8, 8, 8, 6, 7, 4, 7},
                                  new int[]{ 7, 6, 6, 4, 8, 4},
                                  new int[]{ 5 },
                                  new int[]{ 8, 9, 9, 5, 3, 3, 7},
                                  new int[]{ 7, 8, 6, 5, 3, 5, 7, 8, 7},
                            },

                    new int[][] //7
                            {
                                  new int[]{ 4, 6, 5, 5},
                                  new int[]{ 5 ,6},
                                  new int[]{ 6, 8, 3, 4, 9, 3},
                                  new int[]{ 7, 4, 5, 4, 9},
                                  new int[]{ 8 },
                                  new int[]{ 3, 3, 6, 6, 5, 3, 3, 4},
                                  new int[]{ 4, 3, 8, 8, 4, 4},
                                  new int[]{ 4, 5, 8, 8},
                            },

                    new int[][] //8
                            {
                                  new int[]{ 6, 5, 7, 6, 5},
                                  new int[]{ 8, 7, 8, 5},
                                  new int[]{ 3, 7, 4, 4, 3, 5},
                                  new int[]{ 7},
                                  new int[]{ 8},
                                  new int[]{ 9, 8, 8, 9, 3, 3},
                                  new int[]{ 6, 6, 4, 7, 3, 3, 3},
                                  new int[]{ 3, 6, 6, 7, 3, 7},
                            },

                    new int[][] //9
                            {
                                  new int[]{ 3, 7, 6, 8},
                                  new int[]{ 3, 7, 6},
                                  new int[]{ 4, 8, 5, 9, 7},
                                  new int[]{ 5, 4, 5, 9, 8 ,5, 5},
                                  new int[]{ 5, 7, 9, 9 },
                                  new int[]{ 6, 3, 3, 8, 7, 7},
                                  new int[]{ 6, 7, 3, 7, 5, 8, 3},
                                  new int[]{ 3, 7, 6, 5, 5, 9, 3},
                            },

                    ///////////////////////////////////////////////////////////Fruits 5,6,7,8,9,10
                    
                    new int[][] //0
                            {
                                  new int[]{ 6, 5, 7, 7},
                                  new int[]{ 5, 6, 10},
                                  new int[]{ 6, 6, 7, 7, 9},
                                  new int[]{ 10, 5, 8, 8, 8, 8, 10},
                                  new int[]{ 7, 5, 7, 8, 7, 9, 5, 5, 9},
                                  new int[]{ 8, 8},
                                  new int[]{ 7, 8, 9, 9, 9, 6},
                                  new int[]{ 8, 7, 10, 10, 9, 6, 7, 7, 6},
                            },

                    new int[][] //1
                            {
                                  new int[]{ 5, 6, 7, 6, 8, 6},
                                  new int[]{ 6, 7, 8, 6, 7, 6, 8, 6},
                                  new int[]{ 7, 8, 9, 6},
                                  new int[]{ 8, 8, 8, 6, 10},
                                  new int[]{ 8},
                                  new int[]{ 5, 10, 10, 6, 5, 6},
                                  new int[]{ 9, 9, 7, 8, 8, 6},
                                  new int[]{ 7 },
                            },

                    new int[][] //2
                            {
                                  new int[]{ 6, 6, 5, 5},
                                  new int[]{ 5 ,7},
                                  new int[]{ 6, 8, 5, 8, 9, 5},
                                  new int[]{ 7, 9, 5, 8, 9, 9, 10},
                                  new int[]{ 8, 9, 6, 5, 5, 6, 6},
                                  new int[]{ 10, 10, 6, 6, 5, 7, 7, 5},
                                  new int[]{ 7, 5, 8, 7, 10, 10},
                                  new int[]{ 7, 5, 8, 8},
                            },

                    new int[][] //3
                            {
                                  new int[]{ 6, 5, 7, 6, 6},
                                  new int[]{ 8, 7, 10, 5},
                                  new int[]{ 8, 7, 9, 5, 7, 5},
                                  new int[]{ 7, 8, 5, 8, 5},
                                  new int[]{ 9 },
                                  new int[]{ 9, 8, 10, 9, 8, 8},
                                  new int[]{ 6, 6, 10, 7, 5, 9, 9},
                                  new int[]{ 10, 8, 6, 7, 7, 9},
                            },

                    new int[][] //4
                            {
                                  new int[]{ 7, 7, 6, 8},
                                  new int[]{ 8, 7, 6},
                                  new int[]{ 9, 8, 9, 9, 7, 6},
                                  new int[]{ 10, 5, 9, 9},
                                  new int[]{ 10, 7, 7, 9, 9, 6},
                                  new int[]{ 6, 3, 8, 8, 10, 10},
                                  new int[]{ 6, 7, 7, 10, 5, 8, 9},
                                  new int[]{ 7, 7, 6, 10, 6, 9, 9},
                            },


                    new int[][] //5
                            {
                                  new int[]{ 6, 7, 7, 10},
                                  new int[]{ 5, 6, 10},
                                  new int[]{ 6, 6, 7, 7, 9},
                                  new int[]{ 10 },
                                  new int[]{ 7, 5, 7, 8, 7, 9, 5, 5, 9},
                                  new int[]{ 8, 6, 8},
                                  new int[]{ 7, 9, 9, 9, 6},
                                  new int[]{ 8, 7, 7, 10, 9, 10, 7, 7, 10},
                            },

                    new int[][] //6
                            {
                                  new int[]{ 5 },
                                  new int[]{ 6, 7, 8, 6, 7, 6, 8, 6},
                                  new int[]{ 7, 8, 9, 6},
                                  new int[]{ 8, 8, 8, 6, 10},
                                  new int[]{ 7, 8},
                                  new int[]{ 5, 10, 10, 6, 6, 6},
                                  new int[]{ 8, 9, 9, 7, 8, 8, 6},
                                  new int[]{ 7 },
                            },

                    new int[][] //7
                            {
                                  new int[]{ 6, 6, 5, 5},
                                  new int[]{ 5 ,7},
                                  new int[]{ 6, 8, 5, 8, 9, 5},
                                  new int[]{ 7, 9, 5, 9, 9, 9, 10},
                                  new int[]{ 8, 9, 5, 5, 6, 6},
                                  new int[]{ 10, 10, 6, 6, 5, 7, 7, 5},
                                  new int[]{ 7, 5, 8, 6, 10, 10},
                                  new int[]{ 7, 5, 8, 8},
                            },

                    new int[][] //8
                            {
                                  new int[]{ 6, 5, 7, 6, 6},
                                  new int[]{ 8, 7, 10, 5},
                                  new int[]{ 8, 7, 10, 5, 7, 5},
                                  new int[]{ 7, 7, 5, 8, 5},
                                  new int[]{ 8 ,8 ,8},
                                  new int[]{ 9, 8, 10, 9, 8, 8},
                                  new int[]{ 6, 6, 10, 7, 5, 9, 9},
                                  new int[]{ 10, 8,10 },
                            },

                    new int[][] //9
                            {
                                  new int[]{ 7, 7, 6, 8},
                                  new int[]{ 8, 6},
                                  new int[]{ 9, 8, 9, 9, 7, 6},
                                  new int[]{ 10, 8, 9, 9, 9},
                                  new int[]{ 10, 7, 7, 9, 9, 6},
                                  new int[]{ 10, 5},
                                  new int[]{ 6, 7, 7, 10, 5, 8, 9},
                                  new int[]{ 9, 7, 6, 10, 6, 9, 9},
                            },

                    /////////////////////////////////////////////////////////Fruits 6,7,8,9,10,11
                    ///
                    new int[][] //0
                            {
                                  new int[]{ 6, 7, 10},
                                  new int[]{ 7, 6, 10},
                                  new int[]{ 6, 6, 7, 7},
                                  new int[]{ 10, 11, 8, 8, 9, 8, 10},
                                  new int[]{ 7, 11, 11, 11, 7},
                                  new int[]{ 8, 7, 8},
                                  new int[]{ 7, 9, 9, 10, 9, 6},
                                  new int[]{ 8, 7, 8, 10, 9, 6, 7, 7, 10},
                            },

                    new int[][] //1
                            {
                                  new int[]{ 7, 6, 7, 6, 8, 6},
                                  new int[]{ 6, 7, 8, 6, 7, 10, 10, 6},
                                  new int[]{ 7, 8, 9, 6},
                                  new int[]{ 8, 8, 8, 6, 11},
                                  new int[]{ 7},
                                  new int[]{ 11, 10, 10, 6, 11, 6},
                                  new int[]{ 8, 9, 9, 7, 8, 8, 6},
                                  new int[]{ 11, 11, 11, 10, 9, 10, 7, 7, 7},
                            },

                    new int[][] //2
                            {
                                  new int[]{ 6, 6, 7, 8},
                                  new int[]{ 6 ,7},
                                  new int[]{ 6 },
                                  new int[]{ 7, 11, 9 },
                                  new int[]{ 8, 11, 6, 7, 7, 6, 6},
                                  new int[]{ 10, 10, 6, 6, 7, 7, 7, 5},
                                  new int[]{ 10, 6, 8, 7, 9, 9},
                                  new int[]{ 7, 8, 8, 8},
                            },

                    new int[][] //3
                            {
                                  new int[]{ 6, 6, 7, 6, 6},
                                  new int[]{ 8, 7, 10, 9},
                                  new int[]{ 8, 8, 7, 5},
                                  new int[]{ 11},
                                  new int[]{ 8 },
                                  new int[]{ 9, 8, 10, 9, 8, 8},
                                  new int[]{ 6, 6, 10, 10, 6, 9, 9},
                                  new int[]{ 10, 10, 6, 11, 11, 9},
                            },

                    new int[][] //4
                            {
                                  new int[]{ 7, 7, 8},
                                  new int[]{ 8, 7, 6 , 7},
                                  new int[]{ 9, 8, 7, 11, 7},
                                  new int[]{ 10, 8, 7, 9, 9, 8},
                                  new int[]{ 10, 7, 7, 9, 9, 10, 10,8},
                                  new int[]{ 11, 8, 9, 8, 6, 10},
                                  new int[]{ 11, 7, 9, 10, 9, 8, 9},
                                  new int[]{ 8 },
                            },

                    new int[][] //5
                            {
                                  new int[]{ 6, 7, 10},
                                  new int[]{ 6, 6, 10},
                                  new int[]{ 6, 6, 7, 10},
                                  new int[]{ 10, 11, 8, 8, 9, 8, 10},
                                  new int[]{ 7, 11, 11, 11, 7},
                                  new int[]{ 8 },
                                  new int[]{ 7, 9, 9, 10, 9, 6},
                                  new int[]{ 8, 10, 6, 6, 7, 7, 10},
                            },

                    new int[][] //6
                            {
                                  new int[]{ 7, 6, 7, 6, 8, 6},
                                  new int[]{ 6, 7, 8, 6, 7, 10, 10, 6},
                                  new int[]{ 7 },
                                  new int[]{ 8, 8, 8, 7, 7},
                                  new int[]{ 7},
                                  new int[]{ 11, 10, 10, 6, 11, 7},
                                  new int[]{ 8, 9, 9, 7, 8, 8, 11},
                                  new int[]{ 11, 11, 11, 10, 9, 10, 7, 7, 7},
                            },

                    new int[][] //7
                            {
                                  new int[]{ 6, 6, 7, 8},
                                  new int[]{ 6 ,7},
                                  new int[]{ 6 },
                                  new int[]{ 7, 11, 9 },
                                  new int[]{ 8, 11, 6, 7, 7, 6, 6},
                                  new int[]{ 10 ,6, 6, 7, 7, 7, 5},
                                  new int[]{ 10, 6, 8, 7, 7, 9},
                                  new int[]{ 7, 8, 8, 8},
                            },

                    new int[][] //8
                            {
                                  new int[]{ 6, 6, 7, 6, 6},
                                  new int[]{ 8, 7, 10, 6},
                                  new int[]{ 8, 8, 5, 5},
                                  new int[]{ 11 },
                                  new int[]{ 8  },
                                  new int[]{ 9, 8, 8, 9, 8, 8},
                                  new int[]{ 6, 6, 10, 10, 9, 9, 9},
                                  new int[]{ 10, 10, 10, 11, 11, 9},
                            },

                    new int[][] //9
                            {
                                  new int[]{ 7, 7, 8},
                                  new int[]{ 8, 7, 6 , 7},
                                  new int[]{ 9, 8, 7, 11, 7},
                                  new int[]{ 10, 8, 7, 9, 9, 9},
                                  new int[]{ 10 },
                                  new int[]{ 11, 8, 9, 8, 6, 7},
                                  new int[]{ 11, 7, 9, 10, 9, 7, 9},
                                  new int[]{ 8 },
                            },

                    };


    private int[][][] spawningSet = new int[][][]
                    {
                     
                     new int[][] //0
                            {
                                  new int[]{ 0, 1, 2, 1, 4, 3, 1, 3},//0      //Fruits 0,1,2,3,4,5
                                  new int[]{ 1, 0, 2, 2, 2, 5, 3, 1},//1
                                  new int[]{ 2, 4, 3, 4, 0, 3, 2, 4},//2
                                  new int[]{ 4, 5, 4, 5, 1, 2, 3, 5},//3
                                  new int[]{ 5, 3, 2, 1, 3, 4, 4, 0},//4
                                  new int[]{ 6, 3, 1, 2, 4, 1, 5, 5},//5
                                  new int[]{ 1, 2, 0, 3, 2, 2, 2, 1},//6
                                  new int[]{ 2, 1, 4, 4, 1, 3, 1, 3},//7
                                  new int[]{ 3, 0, 5, 5, 1, 4, 0, 4},//8
                                  new int[]{ 4, 2, 1, 1, 3, 1, 1, 5},//9

                                  new int[]{ 0, 2, 2, 3, 4, 1, 1, 3},//10      
                                  new int[]{ 1, 4, 2, 2, 2, 5, 3, 1},//11
                                  new int[]{ 2, 1, 3, 4, 3, 3, 2, 4},//12
                                  new int[]{ 4, 5, 1, 5, 1, 1, 3, 5},//13
                                  new int[]{ 5, 3, 2, 2, 5, 1, 5, 0},//14
                                  new int[]{ 6, 3, 1, 2, 4, 1, 5, 0},//15
                                  new int[]{ 1, 2, 2, 4, 4, 0, 4, 1},//16
                                  new int[]{ 2, 1, 1, 4, 1, 3, 1, 3},//17
                                  new int[]{ 3, 0, 0, 3, 0, 2, 3, 4},//18
                                  new int[]{ 4, 2, 1, 1, 3, 5, 3, 5},//19

                            },

                     new int[][] //1
                            {
                                  new int[]{ 3, 4, 5, 4, 4, 7, 8, 5},//0      //Fruits 3,4,5,6,7,8
                                  new int[]{ 5, 5, 4, 3, 5, 5, 3, 6},//1
                                  new int[]{ 6, 6, 5, 6, 3, 4, 4, 4},//2
                                  new int[]{ 8, 4, 6, 5, 5, 3, 5, 7},//3
                                  new int[]{ 4, 7, 3, 7, 7, 8, 6, 8},//4
                                  new int[]{ 5, 8, 5, 7, 7, 6, 7, 3},//5
                                  new int[]{ 7, 3, 7, 8, 7, 5, 3, 4},//6
                                  new int[]{ 8, 5, 5, 4, 8, 4, 8, 7},//7
                                  new int[]{ 3, 3, 4, 5, 3, 8, 4, 8},//8
                                  new int[]{ 4, 3, 8, 7, 4, 6, 6, 4},//9

                                  new int[]{ 6, 5, 4, 4, 5, 4, 5, 6},//10      
                                  new int[]{ 7, 7, 5, 7, 6, 5, 6, 7},//11
                                  new int[]{ 4, 8, 6, 5, 5, 6, 7, 8},//12
                                  new int[]{ 8, 3, 7, 6, 4, 4, 8, 4},//13
                                  new int[]{ 3, 4, 8, 7, 5, 3, 3, 5},//14
                                  new int[]{ 5, 6, 4, 8, 6, 3, 5, 3},//15
                                  new int[]{ 7, 7, 6, 3, 7, 5, 6, 4},//16
                                  new int[]{ 4, 7, 7, 4, 3, 8, 7, 7},//17
                                  new int[]{ 5, 5, 8, 8, 7, 6, 8, 8},//18
                                  new int[]{ 8, 2, 2, 8, 8, 7, 4, 4},//19
                            },

                            new int[][] //2
                            {
                                  new int[]{ 5, 10, 7,  10, 7, 8, 9, 6},//0      //Fruits 5,6,7,8,9,10
                                  new int[]{ 7,  9, 5,  9,  7, 9, 7, 7},//1
                                  new int[]{ 9,  7, 8,  8,  9, 7, 8, 8},//2
                                  new int[]{ 6,  9, 9,  6,  6, 8, 10, 9},//3
                                  new int[]{ 9, 10, 10, 7,  9, 9, 7, 10},//4
                                  new int[]{ 10, 8, 9,  8,  6, 6, 8, 10},//5
                                  new int[]{ 10, 9, 10, 9,  9, 10, 9, 7},//6
                                  new int[]{ 5, 10, 8,  10, 7, 8, 8, 7},//7
                                  new int[]{ 5,  8, 9,  8,  9, 10, 10, 8},//8
                                  new int[]{ 7,  9, 10, 9,  7, 7, 10, 7},//9

                                  new int[]{ 10, 7,  8,  7,  10, 7, 10, 8},//10      
                                  new int[]{ 8,  8,  9,  8,  8, 9, 8, 9},//11
                                  new int[]{ 7,  9,  10, 9,  9, 6, 6, 10},//12
                                  new int[]{ 9,  10, 9,  10, 7, 8, 9, 6},//13
                                  new int[]{ 6,  6,  8,  7,  7, 9, 9, 7},//14
                                  new int[]{ 8,  8,  9,  8,  9, 10, 10, 8},//15
                                  new int[]{ 9,  5,  10, 9,  9, 9, 6, 9},//16
                                  new int[]{ 10, 8,  8,  9,  10, 7, 10, 10},//17
                                  new int[]{ 8,  10, 6,  10, 6, 8, 9, 7},//18
                                  new int[]{ 9,  9,  6,  8,  8, 9, 10, 8},//19
                            },

                            new int[][] //3
                            {
                                  new int[]{ 6, 11, 8, 10, 9, 6, 11, 6},//0      //Fruits 6,7,8,9,10,11
                                  new int[]{ 7, 10, 9, 7, 10, 7, 8, 8},//1
                                  new int[]{ 8, 8, 10, 9, 9, 11, 9, 9},//2
                                  new int[]{ 9, 9, 7, 6, 8, 9, 10, 10},//3
                                  new int[]{ 11,9, 9, 8, 7, 10, 7, 11},//4
                                  new int[]{ 8, 7, 10, 9, 9, 11, 9, 11},//5
                                  new int[]{ 8, 8, 9, 10, 8, 7, 10, 10},//6
                                  new int[]{ 9, 9, 8, 7, 9, 9, 8, 7},//7
                                  new int[]{ 10,10,11, 9, 10, 9, 9, 8},//8
                                  new int[]{ 11,8, 11, 11, 7, 6, 10, 9},//9

                                  new int[]{ 6, 8, 9, 7, 7, 11, 7, 11},//10      
                                  new int[]{ 8, 9, 8, 8, 9, 7, 9, 10},//11
                                  new int[]{ 9, 10, 7, 9, 10, 9, 8, 8},//12
                                  new int[]{ 10, 11, 11, 9, 11, 11, 9, 9},//13
                                  new int[]{ 11, 8, 9, 11, 11, 6, 11, 9},//14
                                  new int[]{ 6, 9, 10, 10, 10, 8, 10, 10},//15
                                  new int[]{ 8, 10, 11, 11, 10, 10, 10, 8},//16
                                  new int[]{ 9, 11, 8, 10, 11, 7, 11, 7},//17
                                  new int[]{ 9, 11, 9, 8, 10, 9, 8, 11},//18
                                  new int[]{ 11, 8, 10, 7, 7, 11, 6, 6},//19
                            }

                    };

    public int levelIndex;
    private int spawnSetIndex;

    private int[][] nextSpawnSet;

    private void Awake()
    {
        instance = GetComponent<BoxManager>();
        Debug.Log("Awake BoxManager");
    }

    void OnEnable()
    {
        Debug.Log("AAAA");
        if(MPLDirector.Instance != null)
            MPLDirector.Instance.initGG += initGame;

    }

    void OnDisable()
    {
        Debug.Log("BBBBB");

        if(MPLDirector.Instance != null)
            MPLDirector.Instance.initGG -= initGame;
    }

    private void Start()
    {

        //if(MPLDirector.Instance.roomOwner)
        {
            if (MPLDirector.Instance != null)
                MPLDirector.Instance.roomOwner = false;

            if(MPLDirector.Instance == null) //direct game
            {
                boxSpawnTimeGap = 0.8f;
                nextBoxSetSpawnTimeGap = 1.5f;
                decreaseScoreOnWrongMove = 2;
            }

            initGame();
        }
    }

    public void initGame()
    {
        //setMPLSessionInfo(); //MPL

        emptySprite = emptyPrefab.GetComponent<SpriteRenderer>().sprite;

        SoundManager.PlayLoopSound(SoundManager.Sound.GameBG);

        SetInitBoxes();

        StartCoroutine(SetNewBoxes());

        points = 0;
        addScore?.Invoke(points);

        updateTimer?.Invoke();
    }

    private void SetInitBoxes()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;
       
        List<Sprite> possibleCharacters = new List<Sprite>();
        possibleCharacters.AddRange(characters);

        //levelIndex = MPLDirector.Instance.currentSet;

        spawnSetIndex = levelIndex / NO_OF_SETS;  //levelIndex
        nextSpawnSet = spawningSet[spawnSetIndex];
        spawnIndex = 0;
        /////

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

        if(moreThan2Cluster)
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
        ParticleSystem particle = Instantiate(box.particles); //obj.GetComponent<ParticleSystem>();
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

    private int spawnIndex;
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

            int nextBox = nextSpawnSet[spawnIndex][x];

            //int[] nextSet = chooseFrom[levelIndex];
            //int nextBox = nextSet[UnityEngine.Random.Range(0, nextSet.Length)];

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

        StopCoroutine(SetNewBoxes()); 

        if(spawnIndex < nextSpawnSet.Length - 1)
            spawnIndex++;
        else
            spawnIndex = 0;

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

        //List<ColContainer> TempContainerList = new List<ColContainer>();
        //TempContainerList = containerList;

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
        int prevPoints = points;

        if (increaseScore)  //increasing
        {
            newpoints = (boxCount * (boxCount));
            points = points + newpoints;
        }
        else
            points = points - decreaseScoreOnWrongMove; //decreasing

        FloatingScore floatingScore = null;

        if(box != null)  //increase
        {
            floatingScore = ObjectPooler.instance.SpawnFromPool("FloatingScore").GetComponent<FloatingScore>(); //*/ Instantiate(floatingScorePrefab);

            floatingScore.transform.parent = box.transform;
            floatingScore.transform.localPosition = new Vector3(0, 0, 0);
            floatingScore.activate();

            if (increaseScore)
                floatingScore.newPoints = newpoints;
            else
                floatingScore.newPoints = decreaseScoreOnWrongMove;
        }

        floatingScore.UpdateFloatingScore(floatingScore.newPoints, increaseScore);

        if (points < 0) points = 0;

        //addScore?.Invoke(points);

        if (MPLDirector.Instance != null)
            MPLDirector.Instance.UpdateScore(prevPoints, points);

        if (boxCount >= goodTextShowCount)
            showGoodText?.Invoke();
    }

    public void GameFinished(int gamefinish)
    {
        SoundManager.StopLoopSound();

        if (gamefinish == 2)
            SoundManager.PlaySound(SoundManager.Sound.Die);

        gameOver = true;
        StopAllCoroutines();

        GameObject gameOverObj = Instantiate(gameCompletePrefab,new Vector3(0f,0f,0f),Quaternion.identity);
        gameOverObj.transform.parent = CanvasRef.transform;

        gameOverObj.transform.localPosition = Vector3.zero;
        gameOverObj.transform.localEulerAngles = Vector3.zero;
        gameOverObj.transform.localScale = new Vector3(2f,2f,2f);

        gameOverObj.GetComponent<GameOver>().InitGameOver(gamefinish, points);

        StartCoroutine(WaitBeforeMPLGameComplete(gamefinish));///wait for mpl screen

    }

    public IEnumerator WaitBeforeMPLGameComplete(int gamefinish)
    {
        yield return new WaitForSeconds(1f);

        if (MPLDirector.Instance != null)
        {
            if (gamefinish == 2)
                MPLDirector.Instance.SubmitScore(MPLGameEndReason.GameEndReasons.OUT_OF_TIME);

            if (gamefinish == 1)
                MPLDirector.Instance.SubmitScore(MPLGameEndReason.GameEndReasons.OUT_OF_LIVES);
        }
    }

    public void PlayAgainReset()  //for popup
    {
        resetToRestart();
        initGame();
    }

    public void resetToRestart()
    {
        StopAllCoroutines();

        containerList.Clear();

        foreach (Transform child in boxesParent.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in BoxSpawner.transform)
        {
            Destroy(child.gameObject);
        }

        gameOver = false;

    }
}
