using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform target; // 追従対象
    private Vector3 offset; // 初期位置との差分

    public void SetTarget(Transform player)
    {
        target = player;
        offset = transform.position - player.position; // 初期位置のオフセットを記録
    }

    private void Update()
    {
        if (target != null)
        {
            // プレイヤーの位置に追従（オフセットを考慮）
            transform.position = target.position + offset;
        }
    }
}
