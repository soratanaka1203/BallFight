using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LightningDashEffect : MonoBehaviour
{
    private float knockbackForce;
    private bool isActive = false;

    public UnityEngine.GameObject hitEffect;

    public void Activate(float force)
    {
        knockbackForce = force;
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return; // スキルが発動中のみ処理

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Shadow"))
        {
            Vector3 knockbackDirection = collision.transform.position - transform.position;
            knockbackDirection.y = 0; // 水平方向のみノックバック
            collision.rigidbody.AddForce(knockbackDirection.normalized * knockbackForce, ForceMode.Impulse);
        }

        // エフェクト再生
        StartCoroutine(PlayEffect(collision.contacts[0].point));
    }

    private IEnumerator PlayEffect(Vector3 pos)
    {
        if (hitEffect != null)
        {
            UnityEngine.GameObject effect = Instantiate(hitEffect, pos, Quaternion.identity);
            yield return new WaitForSeconds(1f); // エフェクトの持続時間
            Destroy(effect);
        }
    }
}
