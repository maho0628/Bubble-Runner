using UnityEngine;

public class NPC : MonoBehaviour
{
    [Tooltip("The amount of life recovered when the player interacts with this NPC.")]
    [SerializeField] private int lifeRecoveryAmount = 1; // �񕜗�
    [Tooltip("List of sprite images for NPC appearance.")]
    public Material[] npcMaterials; // NPC�O���p�̃}�e���A�����X�g
    private Renderer npcRenderer; // NPC�̃����_���[

    private void Start()
    {
        npcRenderer = GetComponent<Renderer>();
        if (npcMaterials.Length > 0 && npcRenderer != null)
        {
            // �����_���ȃX�v���C�g��I��
            Material randomMaterial = npcMaterials[Random.Range(0, npcMaterials.Length)];
            npcRenderer.material = randomMaterial;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Player�^�O���ǂ����m�F
        {
            PlayerControl player = other.gameObject.GetComponentInParent<PlayerControl>();
            if (player != null)
            {
                Debug.Log($"Player detected: {other.name}, Tag: {other.tag}");
                player.RecoverLife(lifeRecoveryAmount); // �v���C���[�̃��C�t����
                Destroy(gameObject); // NPC������
            }
            else
            {
                Debug.Log($"Triggered by {other.name}, but PlayerControl component was not found.");
            }
        }
        else // Player�^�O�ł͂Ȃ��ꍇ
        {
            Debug.Log($"Object {other.name} entered the trigger but does not have the Player tag. Its tag is: {other.tag}");
        }
    }

}

