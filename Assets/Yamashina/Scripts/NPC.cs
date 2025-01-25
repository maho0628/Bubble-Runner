using System.Collections;
using System.Collections.Generic;
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
        if (other.CompareTag("Player"))
        {
            PlayerControl player = other.GetComponent<PlayerControl>();
            if (player != null)
            {
                player.RecoverLife(lifeRecoveryAmount); // �v���C���[�̃��C�t����
                Destroy(gameObject); // NPC������
            }
        }
    }

}
