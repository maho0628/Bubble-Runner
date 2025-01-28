using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // NPCのプレハブ
    public Transform[] spawnAreas; // スポーンエリアの中心
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // スポーンエリアのサイズ
    public int npcCount = 5; // スポーンするNPCの数
    private PlayerControl playerControl; // PlayerControlを参照

    public Material[] npcMaterials; // NPCのマテリアルリスト

    private void Start()
    {
        playerControl = FindAnyObjectByType<PlayerControl>();
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();
        }
    }

    private void SpawnNPC()
    {
        // ランダムな位置を計算
        // 複数のスポーンエリアからランダムに1つ選択
        int randomAreaIndex = Random.Range(0, spawnAreas.Length);
        Transform selectedSpawnArea = spawnAreas[randomAreaIndex];
        // プレイヤーのレーンからランダムにレーンを選択
        int randomLaneIndex = Random.Range(0, playerControl.lanes.Length);
        Transform spawnLane = playerControl.lanes[randomLaneIndex];

        // 選ばれたスポーンエリア内でランダムなZ軸位置を選択
        Vector3 randomPosition = new Vector3(
            spawnLane.position.x, // レーンのX位置
            selectedSpawnArea.position.y, // スポーンエリアの高さ
            Random.Range(selectedSpawnArea.position.z - spawnAreaSize.z / 2, selectedSpawnArea.position.z + spawnAreaSize.z / 2) // Z軸方向のランダム位置
        );

        // NPCを生成
        GameObject npc = Instantiate(npcPrefab, randomPosition, Quaternion.identity);

        // NPCにランダムなマテリアルを設定
        if (npcMaterials.Length > 0)
        {
            Material randomMaterial = npcMaterials[Random.Range(0, npcMaterials.Length)];
            Renderer npcRenderer = npc.GetComponent<Renderer>();
            if (npcRenderer != null)
            {
                npcRenderer.material = randomMaterial; // マテリアルを設定
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 複数のスポーンエリアをシーンビューに可視化
        Gizmos.color = Color.green;
        foreach (var area in spawnAreas)
        {
            Gizmos.DrawWireCube(area.position, spawnAreaSize);
        }
    }
}
