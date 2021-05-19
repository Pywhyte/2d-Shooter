using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityStandardAssets._2D.Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
            
        }
        public void Init()
        {
            curHealth = maxHealth;
        }
    }
    
    public PlayerStats stats = new PlayerStats();

    public int fallBoundary = -20;

    public string deathSoundName = "DeathVoice";
    public string damageSoundName = "Grunt";

    private AudioManager audioManager;
    [SerializeField]
    private StatusIndicator statusIndicator;
    private void Start()
    {
        if (statusIndicator != null)
        {
            stats.Init();
            Debug.LogError("No statusInd");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No AudioManager");
        }
    }

    private void Update()
    {
        if(transform.position.y <= fallBoundary)
        {
            DamagePlayer(99999);
        }
    }
    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<UnityStandardAssets._2D.Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        if(stats.curHealth <= 0)
        {
            audioManager.PlaySound(deathSoundName);
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(damageSoundName);
        }
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

}
