using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Sirenix.OdinInspector;

public class LevelManager : SerializedMonoBehaviour
{
    [BoxGroup("Player")]
    public GameObject player;

    [BoxGroup("Enemies")]
    public GameObject[] enemies;

    [BoxGroup("Enemies")]
    [SerializeField]
    private Vector2Int enemiesAmountMinMax = new Vector2Int(3, 10);

    [BoxGroup("Collectables")]
    public GameObject[] collectables;

    [BoxGroup("Collectables")]
    [SerializeField]
    private Vector2Int collectablesAmountMinMax = new Vector2Int(3, 10);

    [BoxGroup("Powerups")]
    public GameObject milk;

    [BoxGroup("Tilemaps")]
    [SerializeField]
    private Grid[] gridLayouts;

    private Vector3[] gridPositions;

    private Tilemap[] tilemaps;

    private Grid currentGrid;
    private Tilemap[] currentTilemaps;

    public Dictionary<Tilemap, List<Vector3Int>> cellPositions;

    private List<CollectableController> currentCollectables = new List<CollectableController>();

    public bool isInitialized { get; private set; }

    [Button]
    public void Initialize()
    {
        cellPositions = new Dictionary<Tilemap, List<Vector3Int>>();

        currentGrid = gridLayouts[Random.Range(0, gridLayouts.Length)];

        currentTilemaps = currentGrid.GetComponentsInChildren<Tilemap>();

        foreach (Tilemap tilemap in currentTilemaps)
        {
            List<Vector3Int> positions = new List<Vector3Int>();

            foreach (Vector3Int position in tilemap.cellBounds.allPositionsWithin)
            {
                if(tilemap.GetTile(position) != null)
                    positions.Add(position);
            }

            cellPositions.Add(tilemap, positions);
        }

        Instantiate(player, Vector3.zero, player.transform.rotation);

        LayoutObjectAtRandom(collectables,collectablesAmountMinMax.x, collectablesAmountMinMax.y);
        LayoutObjectAtRandom(enemies, enemiesAmountMinMax.x, enemiesAmountMinMax.y);
        LayoutObjectAtRandom(milk);

        isInitialized = true;
    }

    public void LayoutObjectAtRandom(GameObject objectToSpawn)
    {
        GameObject[] objects = new GameObject[1];
        objects[0] = objectToSpawn;

        LayoutObjectAtRandom(objects, 1, 1);
    }

    [Button]
    public void LayoutObjectAtRandom(GameObject[] objects, int minimum, int maximum)
    {
        if (objects.Length < 1) return;

        int amount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < amount; i++)
        {
            GameObject objectToPlace = objects[Random.Range(0, objects.Length)];
            Tilemap tilemap = currentTilemaps[0];
            List<Vector3Int> tilemapPositions = cellPositions[tilemap];
            Vector3Int randomPosition = tilemapPositions[Random.Range(0, tilemapPositions.Count)];
            Vector3 offset = new Vector3(1,1,0) * 0.5f;

            Instantiate(objectToPlace, randomPosition + offset, Quaternion.identity, transform);           
        }
    }
}
