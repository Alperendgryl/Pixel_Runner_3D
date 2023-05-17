using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private void Start()
    {
        BgConstant = BgTimeDiff;
        CoinConstant = coinTimeDiff;
        ObstaclesConstant = obstaclesTimeDiff;

        RandomCar();
        InitiliazePath();
        InitiliazeCoins();
        InitiliazeObstacles();
        InitiliazeBackgroundObj();
    }
    private void Update()
    {
        if (GameManager._canMove)
        {
            SpawnObstacles();
            SpawnBackgroundObj();
            SpawnCoins();
        }
    }

    #region Car
    [Header("Car Section")]
    [Space]
    [SerializeField] private Transform[] Transforms;
    [SerializeField] private GameObject[] cars;
    private void RandomCar()
    {
        for (int i = 0; i < 3; i++)
        {
            int randomObject = Random.Range(0, cars.Length);

            Instantiate(cars[randomObject], Transforms[i]);
        }
    }
    #endregion

    #region Path
    [Header("Path Section")]
    [Space]
    [SerializeField] private GameObject[] pathObjects;
    [SerializeField] private Transform generationThreshold;
    [SerializeField] private float pathObjectDiff;
    [SerializeField] private int pathObjCount;
    private void InitiliazePath()
    {
        Vector3 temp = Vector3.zero; //initial pos
        int counter = 0;

        while (counter <= pathObjCount) // 3.2f between 2 paths = 20 paths in total
        {
            int random = Random.Range(0, pathObjects.Length);
            Instantiate(pathObjects[random], temp, transform.rotation); //to prevent the changings on this.transform.pos
            temp.z += pathObjectDiff;
            counter++;
        }
    }
    #endregion

    #region Coins
    [Header("Coin Section")]
    [Space]
    [SerializeField] private GameObject[] coins;
    [SerializeField] private float coinTimeDiff;
    [SerializeField] private Transform topPos;
    private float CoinConstant;
    private GameObject[] CoinPrefab = new GameObject[6]; // coins.length
    private void InitiliazeCoins()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            Vector3 temp;

            bool upOrDown = Random.value > 0.5f;

            if (upOrDown)
            {
                temp = topPos.position;
            }
            else
            {
                temp = generationThreshold.position;
            }

            CoinPrefab[i] = Instantiate(coins[i], temp, Quaternion.identity);
            CoinPrefab[i].SetActive(false);
        }
    }
    private void SpawnCoins()
    {
        CoinConstant -= Time.deltaTime;

        if (CoinConstant <= 0)
        {
            int randomObject = Random.Range(0, coins.Length);

            if (!CoinPrefab[randomObject].activeInHierarchy)
            {
                CoinPrefab[randomObject].SetActive(true);
            }
            else
            {
                randomObject = Random.Range(0, coins.Length); // create random again.
            }
            CoinConstant = Random.Range(coinTimeDiff * 0.75f, coinTimeDiff * 1.25f);
        }
    }
    #endregion

    #region Obstacles
    [Header("Obstacles Section")]
    [Space]
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private float obstaclesTimeDiff;
    private float ObstaclesConstant;
    private GameObject[] ObstaclesPrefab = new GameObject[16]; // coins.length
    private void InitiliazeObstacles()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            int randomObject = Random.Range(0, obstacles.Length);

            ObstaclesPrefab[i] = Instantiate(obstacles[randomObject], Vector3.forward * generationThreshold.transform.position.z, Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f));
            ObstaclesPrefab[i].SetActive(false);
        }
    }
    private void SpawnObstacles()
    {
        ObstaclesConstant -= Time.deltaTime;

        if (ObstaclesConstant <= 0)
        {
            int randomObject = Random.Range(0, obstacles.Length);

            if (!ObstaclesPrefab[randomObject].activeInHierarchy)
            {
                ObstaclesPrefab[randomObject].SetActive(true);
            }
            else
            {
                randomObject = Random.Range(0, obstacles.Length);
            }

            ObstaclesConstant = Random.Range(obstaclesTimeDiff / 4, obstaclesTimeDiff);
        }
    }
    #endregion

    #region Background Objects
    [Header("Background Objects Section")]
    [Space]
    [SerializeField] private GameObject[] backgroundObj;
    [SerializeField] private float BgTimeDiff;
    [SerializeField] private Transform minPos, maxPos;
    private float BgConstant;
    private GameObject[] BgObjPrefab = new GameObject[17]; // BgObj.length
    private void InitiliazeBackgroundObj()
    {
        for (int i = 0; i < backgroundObj.Length; i++)
        {
            int randomSide = Random.Range(0, 2) < 0.5 ? 1 : -1;

            float randomPos = Random.Range(minPos.transform.position.x * randomSide, maxPos.transform.position.x * randomSide);

            BgObjPrefab[i] = Instantiate(backgroundObj[i], new Vector3(randomPos, 0f, generationThreshold.position.z), Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f));
            BgObjPrefab[i].SetActive(false);
        }
    }
    private void SpawnBackgroundObj()
    {
        BgConstant -= Time.deltaTime;

        if (BgConstant <= 0)
        {
            int randomObject = Random.Range(0, backgroundObj.Length);

            if (!BgObjPrefab[randomObject].activeInHierarchy)
            {
                BgObjPrefab[randomObject].SetActive(true);
            }
            else
            {
                randomObject = Random.Range(0, backgroundObj.Length);
            }

            BgConstant = Random.Range(BgTimeDiff / 2, BgTimeDiff);
        }
    }
    #endregion
}
