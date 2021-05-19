using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float fireRate = 0f;
    public int Damage = 10;
    public LayerMask whatToHit;
    public Transform muzzleFlash;
    public Transform hitPrefab;

    public Transform BulletTrailPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float camShakeAmt = 0.01f;
    public float camShakeLenght = 0.1f;
    CameraShake camShake;
    AudioManager audioManager;
    
    public string weaponShootSound = "DefoultShoot";


    Vector3 hitNormal;

    float timeToFire = 0f;
    Transform firePoint;

    [System.Obsolete]
    private void Awake()
    {
        firePoint = transform.FindChild("FirePoint");
        if(firePoint == null)
        {
            Debug.LogError("We DONT HAVE FIRE POINT!!!!");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        camShake= GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null)
        {
            Debug.LogError("NO camera shake script found on GM obj");
        }
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audio Mnager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();

            }
        }
    }
    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
       

        if (hit.collider != null)
        {
            
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(Damage);
                Debug.Log("We hit " + hit.collider.name + " and did " + Damage + " damage.");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            Vector3 hitNormal;
            Vector3 hitPos;
            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
                
            Effect(hitPos, hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;

        }


    }
    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
        LineRenderer lr = trail.GetComponent<LineRenderer>();
        if(lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
       
        Destroy(trail.gameObject, 0.2f);
        if(hitNormal != new Vector3(9999,9999,9999))
        {
            Transform hitParticle =  Instantiate(hitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal)) as Transform;
            Destroy(hitParticle.gameObject, 1f);
        }
       Transform clone = Instantiate(muzzleFlash, firePoint.position, firePoint.rotation) as Transform;
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        
        Destroy(clone.gameObject, 0.05f);

        //shake the camera
        camShake.Shake(camShakeAmt, camShakeLenght);

        //Play shoot sound
        audioManager.PlaySound(weaponShootSound);
    }

}
