using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{

    public GameObject npcPrefab; // NPC�̃v���n�u
    public Transform spawnPoint; // �X�|�[���ʒu
    public int npcCount = 5; // �X�|�[������NPC�̐�

    public Material[] npcMaterials; // NPC�̃}�e���A�����X�g

    private void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        // NPC���X�|�[��
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

        // NPC�̃}�e���A����ݒ�
        NPC npcScript = npc.GetComponent<NPC>();
        if (npcScript != null)
        {
            npcScript.npcMaterials = npcMaterials; // �}�e���A�����X�g��n��
        }
    }
}


