using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField]
    WaveSpawber spawner;

    [SerializeField]
    Animator waveAnimator;

    [SerializeField]
    Text WaveCountdownText;

    [SerializeField]
    Text waveCountText;

    private WaveSpawber.SpawnState previousState;
   
    void Start()
    {
        if(spawner == null)
        {
            Debug.LogError("no spawner");
            this.enabled=false;
        }
        if (waveAnimator == null)
        {
            Debug.LogError("no waveAnimator");
            this.enabled = false;
        }
        if (WaveCountdownText == null)
        {
            Debug.LogError("no WaveCountdownText");
            this.enabled = false;
        }
        if (waveCountText == null)
        {
            Debug.LogError("no waveCountText");
            this.enabled = false;
        }
    }

    
    void Update()
    {
         switch(spawner.State)
        {
            case WaveSpawber.SpawnState.COUNTING:
                UpdateCountdownUI();
                break;
            case WaveSpawber.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
            
        }

        previousState = spawner.State;
    }
    void UpdateCountdownUI()
    {
        if (previousState != WaveSpawber.SpawnState.COUNTING)
        {
            waveAnimator.SetBool("WaveIncoming", false);
            waveAnimator.SetBool("WaveCountDown", true);
            //Debug.Log("Counting");
        }
        WaveCountdownText.text = ((int)spawner.WaveCountdown).ToString();
    }
    void UpdateSpawningUI()
    {
        if(previousState != WaveSpawber.SpawnState.SPAWNING)
        {
            waveAnimator.SetBool("WaveIncoming", true);
            waveAnimator.SetBool("WaveCountDown", false);

            waveCountText.text = ((int)spawner.NextWave).ToString(); 
            //Debug.Log("Spawning");
        }
        
    }
}
