using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEffect : MonoBehaviour
{
    public UnityEngine.GameObject explosionEffect; // 爆発エフェクトのプレハブ
    public float explosionRadius = 3f; // 爆発の範囲
    public float explosionForce = 10f; // 吹き飛ばしの強さ
    public float upwardsModifier = 1f; // 垂直方向の持ち上げ強さ

    private float delTime = 3.5f;
    private float time;
    private void Start()
    {
        time = Time.time;
    }

    private void Update()
    {
        if (Time.time - time >= delTime)Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shadow"))
        {
            // 爆発を発生
            Explode();

            // ファイアーボールを削除
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        // エフェクトを生成
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 爆発の影響を与える
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // ノックバックを適用（爆発の中心から外向きに力を加える）
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier, ForceMode.Impulse);
            }
        }
    }
}
