using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawber : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING};

    [System.Serializable]
  public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }
    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    public float timeBetweenWaves = 5f;
    private float waveCountDown;
    public float WaveCountdown
    {
        get { return waveCountDown; }
    }
    

    private float searchCountDown = 1f;
    public Transform[] spawnPoints;

   private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }
    private void Start()
    {
        waveCountDown = timeBetweenWaves;
    }
    private void Update()
    {
        
        if(state == SpawnState.WAITING)
        {
            if(!EnemyIsAlive())
            {
                WaveCopmleted();
            }
            else
            {
                return;
            }
        }
        if(waveCountDown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
           
        }
        else
        {
            waveCountDown -= Time.deltaTime;
        }
    }
    void WaveCopmleted()
    {
        Debug.Log("Wave Completed!");

        state = SpawnState.COUNTING;
        waveCountDown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE! Looping...");
           
        }
        else
        {
            nextWave++;
        }



    }
    bool EnemyIsAlive()
    {
        searchCountDown -= Time.deltaTime;
        if (searchCountDown <= 0f)
        {
            searchCountDown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }

         return true;
       
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        //waiting

        yield break;
    }
    
    void SpawnEnemy(Transform _enemy)
    {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
        Debug.Log("spawning enemy: " + _enemy.name);
    }
}
