using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounce : MonoBehaviour
{
    float baseBounceForce = 0.3f; // 基本的な跳ね返りの強さ

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shadow"))
        {
            Vector3 normal = collision.contacts[0].normal; // 衝突した面の法線
            Vector3 relativeVelocity = collision.relativeVelocity; // 衝突時の相対速度

            // 水平方向の相対速度を考慮する
            Vector3 bounceDirection = new Vector3(-normal.x, 0f, -normal.z).normalized;

            // 速度に応じたバウンド力を計算（速度が速いほど強く跳ね返る）
            float bounceForce = baseBounceForce + relativeVelocity.magnitude * 0.5f;

            collision.rigidbody.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);
        }
    }
}
