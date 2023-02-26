using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

/*
 * In this game you can select a building from the construction menu with left click and then in the suitable location you can build it with your right click.
 * If you left click with your mouse to any building you will open the information menu of that building. with right click to any position you can close it.
 * If you open Barracks menu and then click soldiers to spawn it. It will spawn in a suitable location near to the barrack.
 * You can drag select or single select the units with the left click of your mouse. Then right click to enemy unit or building or a position to give attack or move order to selected units.
 * If you click with your left mouse button to empty space while units selected, they will unselected and do not follow your orders until you reselect them again.
 * You can scroll on the menu infinitely and also you can spawn buildings and soldiers as many as you want.
 * I couldn't have time for reducing the drawcall, plus I didnt have installed photoshop to prepare a single asset that has all the textures I used in the game.
 */
namespace Assets.Scripts {
    public class GameController : MonoBehaviour {
        [Header("Spawning Entities")]
        [Space(10)]
        [SerializeField]
        private Tilemap map = null;

        [SerializeField]
        private GameObject objectToMove;

        private GameObject entityToSpawn;

        #region Mouse Tracking

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

        private Vector3Int mouseMapPosition;

        #endregion

        #region Entity Selection

        [Header("Entity Selection")]
        [SerializeField]
        private LayerMask clickablesLayer;

        [SerializeField]
        private List<GameObject> selectedUnits;

        [SerializeField]
        private GameObject target;

        [SerializeField]
        private Transform selectionAreaTransform;

        private Vector3 dragSelectionStartPosition;

        [SerializeField]
        private GameObject selectedItem;

        #endregion

        private void Awake() {
            selectionAreaTransform.gameObject.SetActive(false);
        }

        private void Update() {
            mouseMapPosition = map.WorldToCell(mousePosition);

            if (Input.GetKey(KeyCode.Escape) && objectToMove != null) {
                objectToMove.SetActive(false);
                objectToMove = null;
            }

            if (objectToMove != null) {
                objectToMove.transform.position = map.GetCellCenterLocal(mouseMapPosition);
            }

            if (Input.GetMouseButtonDown(1)) {
                // If its for building
                if (objectToMove != null) {
                    if (CanSpawnBuildings(objectToMove, mousePosition)) {
                        BuildOnClick(worldPosition);
                        objectToMove = null;
                        AstarPath.active.Scan();
                    } else
                        Debug.Log("Cant Build!");
                }

                //if its for unit selection
                else {
                    if (!target.activeSelf && selectedUnits.Count > 0) {
                        target.SetActive(true);
                        target.transform.position = map.GetCellCenterLocal(mouseMapPosition);
                    } else if (selectedUnits.Count > 0) {
                        target.transform.position = map.GetCellCenterLocal(mouseMapPosition);
                    }

                    if (selectedItem != null && selectedItem.GetComponent<Entity>().IsBuilding) {
                        selectedItem.GetComponent<Building>().Deselected();
                    }
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                selectionAreaTransform.gameObject.SetActive(false);
                Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(dragSelectionStartPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                foreach (var item in collider2DArray) {
                    if (item.gameObject.GetComponent<Entity>().Faction == (int)Enums.Factions.Allies) {
                        if (item.gameObject.GetComponent<Entity>().IsUnit) {
                            SelectUnits(selectedUnits, item.gameObject);
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0)) {
                Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 lowerLeftCorner = new Vector3(Mathf.Min(dragSelectionStartPosition.x, currentMousePosition.x), Mathf.Min(dragSelectionStartPosition.y, currentMousePosition.y));
                Vector3 upperRightCorner = new Vector3(Mathf.Max(dragSelectionStartPosition.x, currentMousePosition.x), Mathf.Max(dragSelectionStartPosition.y, currentMousePosition.y));

                selectionAreaTransform.position = lowerLeftCorner;
                selectionAreaTransform.localScale = upperRightCorner - lowerLeftCorner;
            }

            if (Input.GetMouseButtonDown(0)) {
                ///for multiple unit selection
                selectionAreaTransform.gameObject.SetActive(true);
                dragSelectionStartPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (target.activeSelf) {
                    target.SetActive(false);
                }

                if (ClickSelect() != null) {
                    selectedItem = ClickSelect();
                    Entity selectedType = selectedItem.GetComponent<Entity>();
                    if (selectedItem != null && selectedType.IsBuilding) {
                        selectedItem.GetComponent<Building>().Selected();
                    }

                    if (selectedItem != null && selectedType.IsUnit) {
                        if (!selectedItem.GetComponent<Unit>().IsAI) {
                            SelectUnits(selectedUnits, selectedItem);
                        }
                    }
                } else if (selectedUnits.Count > 0) {
                    DeselectUnits(selectedUnits);
                }
            }
        }

        private void DeselectUnits(List<GameObject> units) {
            foreach (var item in units) {
                item.GetComponent<Unit>().IsSelected = false;
                item.transform.GetChild(0).gameObject.SetActive(false);
                item.GetComponent<Pathfinding.AIDestinationSetter>().target = null;
            }

            units.Clear();
        }

        private void SelectUnits(List<GameObject> units, GameObject unit) {
            unit.GetComponent<Unit>().IsSelected = true;
            unit.transform.GetChild(0).gameObject.SetActive(true);
            units.Add(unit);
        }

        public void SpawnBuilding(int spawnCode) {
            if (objectToMove == null && spawnCode == (int)Enums.BuildCodes.PowerPlant) {
                objectToMove = ObjectPooler.SharedInstance.GetPooledObject("PowerPlant");
                objectToMove.transform.position = mousePosition;
                objectToMove.SetActive(true);
            } else if (objectToMove == null && spawnCode == (int)Enums.BuildCodes.Barracks) {
                objectToMove = ObjectPooler.SharedInstance.GetPooledObject("Barracks");
                objectToMove.transform.position = mousePosition;
                objectToMove.SetActive(true);
            }
        }

        public void SpawnUnits(int spawnCode) {
            if (spawnCode == (int)Enums.Soldiers.Infantry) {
                entityToSpawn = ObjectPooler.SharedInstance.GetPooledObject("Infantry");
            } else if (spawnCode == (int)Enums.Soldiers.Peasant) {
                entityToSpawn = ObjectPooler.SharedInstance.GetPooledObject("Peasant");
            } else if (spawnCode == (int)Enums.Soldiers.Knight) {
                entityToSpawn = ObjectPooler.SharedInstance.GetPooledObject("Knight");
            }

            entityToSpawn.transform.position = selectedItem.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
            entityToSpawn.SetActive(true);
        }

        private void BuildOnClick(Vector3 point) {
            Vector3Int cellIndex = map.WorldToCell(point);
            BuildPositionSetter(cellIndex);
        }

        private void BuildPositionSetter(Vector3Int dest) {
            objectToMove.transform.position = map.GetCellCenterLocal(dest);
            objectToMove.GetComponent<Building>().IsBuilt = true;
        }

        GameObject ClickSelect() {
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit) {
                return hit.transform.gameObject;
            } else return null;
        }

        private bool CanSpawnBuildings(GameObject building, Vector3 position) {
            // //Menuden seçilen objeyi taşırken direkt olarak o obje ile etkileşime girdiği için ayrım yaptırıp altındaki bina ile çakışıp çakışmadığını hesaplattıramadım, PowerPlantler ile çalışıyor ancak Barracklar ile çalışmıyor.
            // BoxCollider2D buildingBoxCollider = building.gameObject.GetComponent<BoxCollider2D>();
            // var result = Physics2D.OverlapBox(position + (Vector3)buildingBoxCollider.offset, buildingBoxCollider.size, 0, clickablesLayer);
            // if (result != null && result.gameObject.GetComponent<Building>().IsBuilt) {
            //     return false;
            // } else {
            //     return true;
            // }
            if (building.GetComponent<Entity>().IsColliding) {
                return false;
            } else {
                return true;
            }
        }
    }
}