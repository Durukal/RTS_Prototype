using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts {
    public class MouseFollower : MonoBehaviour {
        [SerializeField]
        private GameObject powerPlant;

        [SerializeField]
        private GameObject barrack;

        [SerializeField]
        private Tilemap map = null;

        [SerializeField]
        private Tile ground = null;

        private GameObject objectToMove;
        private Vector3Int mouseMapPosition;

        private Vector3 mousePosition {
            get {
                var tempPosition = Input.mousePosition;
                return Camera.main.ScreenToWorldPoint(tempPosition);
            }
        }

        private Vector3 worldPosition {
            get {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                return ray.GetPoint(-ray.origin.z / ray.direction.z);
            }
        }

        [SerializeField]
        private float moveSpeed = 0.1f;

        private void Update() {
            mouseMapPosition = map.WorldToCell(mousePosition);
            if (Input.GetKey(KeyCode.Escape)) {
                GameObject.Destroy(objectToMove);
            }

            if (objectToMove != null) {
                //objectToMove.transform.position = Vector2.Lerp(objectToMove.transform.position, mousePosition, moveSpeed);
                objectToMove.transform.position = map.GetCellCenterLocal(mouseMapPosition);
            }

            if (objectToMove != null && Input.GetMouseButtonDown(1)) {
                BuildOnClick(worldPosition);
                objectToMove = null;
            }
        }

        public void ObjectCreate(int buildCode) {
            if (objectToMove == null && buildCode == (int)Enums.BuildCodes.PowerPlant) {
                objectToMove = GameObject.Instantiate(powerPlant, mousePosition, powerPlant.transform.rotation);
            } else if (objectToMove == null && buildCode == (int)Enums.BuildCodes.Barracks) {
                objectToMove = GameObject.Instantiate(barrack, mousePosition, powerPlant.transform.rotation);
            }
        }

        private void BuildOnClick(Vector3 point) {
            Vector3Int cellIndex = map.WorldToCell(point);
            BuildPositionSetter(cellIndex);
        }

        private void BuildPositionSetter(Vector3Int dest) {
            objectToMove.transform.position = map.GetCellCenterLocal(dest);
        }
    }
}