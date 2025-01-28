using UnityEngine;

public class NPC : MonoBehaviour
{
    [Tooltip("The amount of life recovered when the player interacts with this NPC.")]
    [SerializeField] private int lifeRecoveryAmount = 1; // 回復量
    [Tooltip("List of sprite images for NPC appearance.")]
    public Material[] npcMaterials; // NPC外見用のマテリアルリスト
    private Renderer npcRenderer; // NPCのレンダラー

    private void Start()
    {
        npcRenderer = GetComponent<Renderer>();
        if (npcMaterials.Length > 0 && npcRenderer != null)
        {
            // ランダムなスプライトを選択
            Material randomMaterial = npcMaterials[Random.Range(0, npcMaterials.Length)];
            npcRenderer.material = randomMaterial;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Playerタグかどうか確認
        {
            PlayerControl player = other.gameObject.GetComponentInParent<PlayerControl>();
            if (player != null)
            {
                Debug.Log($"Player detected: {other.name}, Tag: {other.tag}");
                player.RecoverLife(lifeRecoveryAmount); // プレイヤーのライフを回復
                Destroy(gameObject); // NPCを消す
            }
            else
            {
                Debug.Log($"Triggered by {other.name}, but PlayerControl component was not found.");
            }
        }
        else // Playerタグではない場合
        {
            Debug.Log($"Object {other.name} entered the trigger but does not have the Player tag. Its tag is: {other.tag}");
        }
    }

}

