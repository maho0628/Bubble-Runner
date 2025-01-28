using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject npcPrefab; // NPC�̃v���n�u
    public Transform[] spawnAreas; // �X�|�[���G���A�̒��S
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10); // �X�|�[���G���A�̃T�C�Y
    public int npcCount = 5; // �X�|�[������NPC�̐�
    private PlayerControl playerControl; // PlayerControl���Q��

    public Material[] npcMaterials; // NPC�̃}�e���A�����X�g

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
        // �����_���Ȉʒu���v�Z
        // �����̃X�|�[���G���A���烉���_����1�I��
        int randomAreaIndex = Random.Range(0, spawnAreas.Length);
        Transform selectedSpawnArea = spawnAreas[randomAreaIndex];
        // �v���C���[�̃��[�����烉���_���Ƀ��[����I��
        int randomLaneIndex = Random.Range(0, playerControl.lanes.Length);
        Transform spawnLane = playerControl.lanes[randomLaneIndex];

        // �I�΂ꂽ�X�|�[���G���A���Ń����_����Z���ʒu��I��
        Vector3 randomPosition = new Vector3(
            spawnLane.position.x, // ���[����X�ʒu
            selectedSpawnArea.position.y, // �X�|�[���G���A�̍���
            Random.Range(selectedSpawnArea.position.z - spawnAreaSize.z / 2, selectedSpawnArea.position.z + spawnAreaSize.z / 2) // Z�������̃����_���ʒu
        );

        // NPC�𐶐�
        GameObject npc = Instantiate(npcPrefab, randomPosition, Quaternion.identity);

        // NPC�Ƀ����_���ȃ}�e���A����ݒ�
        if (npcMaterials.Length > 0)
        {
            Material randomMaterial = npcMaterials[Random.Range(0, npcMaterials.Length)];
            Renderer npcRenderer = npc.GetComponent<Renderer>();
            if (npcRenderer != null)
            {
                npcRenderer.material = randomMaterial; // �}�e���A����ݒ�
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // �����̃X�|�[���G���A���V�[���r���[�ɉ���
        Gizmos.color = Color.green;
        foreach (var area in spawnAreas)
        {
            Gizmos.DrawWireCube(area.position, spawnAreaSize);
        }
    }
}
