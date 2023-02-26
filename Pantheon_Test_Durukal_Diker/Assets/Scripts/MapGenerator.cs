using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts {
    public class MapGenerator : MonoBehaviour {
        [SerializeField]
        private Tilemap map = null;

        [SerializeField]
        private Tile ground = null;

        [SerializeField]
        private int mapWidth = 0;

        [SerializeField]
        private int mapHeight = 0;

        private void Start() {
            for (int i = -(mapWidth / 2); i <= (mapWidth / 2); i++) {
                for (int j = -(mapHeight / 2); j <= (mapHeight / 2); j++) {
                    map.SetTile(new Vector3Int(i, j, 0), ground);
                }
            }
        }
    }
}