using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats
    {
        public int MaxHealth = 100;
        public int damage = 20;
        

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, MaxHealth); }
        }

        public void Init()
        {
            curHealth = MaxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();
    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator status;

    private void Start()
    {
        
        if (status != null)
        {
            stats.Init();
            status.SetHealth(stats.curHealth, stats.MaxHealth);
        }
        
    }
    public void DamageEnemy(int damage)
    {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
        if (status != null)
        {
            status.SetHealth(stats.curHealth, stats.MaxHealth);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player _player = collision.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(stats.damage);
        }
    }
}
