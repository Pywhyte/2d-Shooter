using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    [SerializeField]
    private int maxLives = 3;
    [SerializeField]
    private AudioSource audio;
    private static int _remainingLives = 3;
    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallBack(bool ative);
    public UpgradeMenuCallBack onToggleUpgradeMenu;
        

    public static int RemainingLives
    {
        get { return _remainingLives; }
    }
    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
    }


    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 2f;
    public Transform spawnPrefab;
    public string spawnSound = "spawnSound";

    [SerializeField]
    private GameObject gameOverUI;
    private AudioManager audioManager;
    public string gameOverSound = "gameOver";




    private void Start()
    {
        _remainingLives = maxLives;
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("Freak Out, where is audio");
        }

    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
    }
    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }
    public void EndGame()
    {
        audioManager.PlaySound(gameOverSound);
        gameOverUI.SetActive(true);
    }
    public IEnumerator RespawnPlayer()
    {
        audioManager.PlaySound("Respawn");
        yield return new WaitForSeconds(spawnDelay);

        audioManager.PlaySound(spawnSound);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone.gameObject, 3f);
    }
    public static void KillPlayer(Player player)
    {

        Destroy(player.gameObject);
        _remainingLives -= 1;
        if(_remainingLives <=0)
        {
            
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
        
    }
    public static void KillEnemy(Enemy enemy)
    {
        gm._killEnemy(enemy);
       
    }
    public void _killEnemy(Enemy _enemy)
    {
        Destroy(_enemy.gameObject);
    }

}
