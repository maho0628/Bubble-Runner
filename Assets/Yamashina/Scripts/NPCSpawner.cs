using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{

    public GameObject npcPrefab; // NPCのプレハブ
    public Transform spawnPoint; // スポーン位置
    public int npcCount = 5; // スポーンするNPCの数

    public Material[] npcMaterials; // NPCのマテリアルリスト

    private void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        // NPCをスポーン
        GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

        // NPCのマテリアルを設定
        NPC npcScript = npc.GetComponent<NPC>();
        if (npcScript != null)
        {
            npcScript.npcMaterials = npcMaterials; // マテリアルリストを渡す
        }
    }
}


