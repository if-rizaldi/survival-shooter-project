using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public float weaponDamage;
    [SerializeField] private GameObject muzzleEffect;
    [SerializeField] private Transform muzzlePosition;
    [SerializeField] private GameObject impactEffect;
    [SerializeField] private TrailRenderer bulletTrail;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private float gunDistance;
    public float gunDelay;
    private EnemyStats damagedEnemy;
    private Vector3 impactPosition;
    private float destroyDelay = 0.5f;
    bool isGunFiring;
    float timeUntilFiring;
    Vector3 initialPosition = new Vector3(0.6f, 0, -0.918F);
    Quaternion initialRotation = new Quaternion();
    [SerializeField] private Vector3 recoilForce = new Vector3(1f, 1f, 1f);
    [SerializeField] private GameObject gunPivotPoint;


    void Start()
    {
        timeUntilFiring = gunDelay;
        isGunFiring = true;
        if (!gunPivotPoint)
            gunPivotPoint = this.gameObject;

        gunSound = this.GetComponent<AudioSource>();
        gunSound.Stop();

    }


    void Update()
    {
        Ray ray = new Ray(muzzlePosition.transform.position, transform.forward);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, gunDistance))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (isGunFiring)
                {
                    //munculkan muzzle efek dari senapan ini
                    if (muzzleEffect)
                        InstantiateMuzzleEffect();


                    TrailRenderer trail = Instantiate(bulletTrail, muzzlePosition.position, Quaternion.identity);
                    StartCoroutine(SpawnTrail(trail, hit));




                    //munculkan efek damage pada musuh yang terkena hit
                    if (impactEffect)
                        InstantiateImpactEffect(hit.point);

                    //recoil pada senapan
                    DoRecoil(gunDelay);


                    //berikan efek kerusakan pada musuh
                    if (hit.collider.GetComponent<EnemyStats>())
                    {
                        damagedEnemy = hit.collider.GetComponent<EnemyStats>();
                        damagedEnemy.EnemyDamaged(weaponDamage);
                    }


                    //menjeda tembakan
                    isGunFiring = false;
                    timeUntilFiring = gunDelay;

                }
                else
                {
                    gunSound.Stop();
                    timeUntilFiring -= Time.deltaTime;
                    if (timeUntilFiring <= 0f)
                        isGunFiring = true;

                    //  ResetRecoil();



                }

            }

        }


    }


    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

       

        while (time < 1)
        {
            
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.fixedDeltaTime / Trail.time;

            if(time >= 1)
            {
                Destroy(Trail.gameObject);
            }

            yield return null;
        }

    

    }

    void InstantiateMuzzleEffect()
    {
        float scale = Random.Range(0.1f, 0.5f);
        Vector3 rotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject obj = Instantiate(muzzleEffect, muzzlePosition.transform.position, Quaternion.Euler(rotation));
        obj.transform.SetParent(muzzlePosition);
        //obj.transform.rotation =  Quaternion.Euler(rotation);
        obj.transform.localScale = new Vector3(scale, scale, scale);

        gunSound.PlayOneShot(gunSound.GetComponent<AudioClip>());
        Destroy(obj, destroyDelay);


    }

    void InstantiateImpactEffect(Vector3 impactPosition)
    {
        float scale = Random.Range(0.1f, 0.5f);
        Vector3 rotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        GameObject obj = Instantiate(impactEffect, impactPosition, Quaternion.Euler(rotation));
        obj.transform.localScale = new Vector3(scale, scale, scale);
        Destroy(obj, destroyDelay);

    }

    void ResetRecoil()
    {
        //mengembalikan recoil
        // Menghitung posisi dan rotasi baru saat recoil pulih
        //float t = Mathf.Clamp01((Time.time - (.1f + gunDelay)) / gunDelay);
        Vector3 recoveryPosition = Vector3.Lerp(gunPivotPoint.transform.localPosition, initialPosition, 0.3f);
        Quaternion recoveryRotation = Quaternion.Lerp(gunPivotPoint.transform.localRotation, initialRotation, 0.3f);


        // Mengatur posisi dan rotasi objek saat recoil pulih
        transform.localPosition = recoveryPosition;
        transform.localRotation = recoveryRotation;

    }

    IEnumerator ResetRecoilAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetRecoil();
        yield break;
    }


    void DoRecoil(float maxRecoilDuration)
    {
        Vector3 currentRecoil = new Vector3();
        float currentRecoilDuration;
        currentRecoil = new Vector3(Random.Range(-recoilForce.x, recoilForce.x),
                                       Random.Range(-recoilForce.y, recoilForce.y),
                                       Random.Range(recoilForce.z, recoilForce.z));
        currentRecoilDuration = 0f;
        // Menghitung durasi recoil
        currentRecoilDuration += Time.deltaTime;

        // Menghitung posisi dan rotasi baru setelah recoil
        Vector3 newPosition = Vector3.Lerp(gunPivotPoint.transform.localPosition, initialPosition + currentRecoil, currentRecoilDuration / maxRecoilDuration);
        Quaternion newRotation = Quaternion.Lerp(gunPivotPoint.transform.localRotation, Quaternion.Euler(gunPivotPoint.transform.localEulerAngles + currentRecoil), currentRecoilDuration / maxRecoilDuration);

        // Mengatur posisi dan rotasi objek
        transform.localPosition = newPosition;
        transform.localRotation = newRotation;

        StartCoroutine(ResetRecoilAfterDelay(gunDelay));

    }



}
