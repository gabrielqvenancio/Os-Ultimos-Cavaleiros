using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void Loop();

public class PerformanceHandler : MonoBehaviour
{
    internal static PerformanceHandler instance;
    [SerializeField] private ScriptableLevel[] allLevels;
    internal static Loop loopsDelegate;
    internal static Loop fixedLoopsDelegate;
    internal List<Queue<GameObject>> enemiesQueue;

    

    private void Start()
    {
        instance = this;
        enemiesQueue = new List<Queue<GameObject>>();
        currentLevel = allLevels[0];

        InputHandler.instance.changeLevelDelegate += ChangeLevelQueues;
    }

    private void Update()
    {
        loopsDelegate?.Invoke();
    }

    private void FixedUpdate()
    {
        fixedLoopsDelegate?.Invoke();
    }

    private void EnqueueEnemy(GameObject enemy)
    {
        enemiesQueue[enemy.GetComponent<Enemy>().attributes.mapId].Enqueue(enemy);
    }

    private GameObject DequeueEnemy(int enemyMapId)
    {
        return enemiesQueue[enemyMapId].Dequeue();
    }

    private void ChangeLevelQueues(ScriptableLevel levelToLoad)
    {
        currentLevel = levelToLoad;
        EnvironmentHandler.instance.
        foreach(GameObject enemyPrefab in levelToLoad.enemiesToSpawn)
        {
            enemiesQueue.Add(new Queue<GameObject>());
        }
    }
}
