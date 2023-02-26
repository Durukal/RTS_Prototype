using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class UIManager : MonoBehaviour {
        public GameObject BarracksUI;
        public GameObject PowerplantUI;

        private void Awake() {
            BarracksUI = GameObject.Find("BarracksUI");
            PowerplantUI = GameObject.Find("PowerPlantUI");
            BarracksUI.SetActive(false);
            PowerplantUI.SetActive(false);
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }

        protected void OnSelectedBuilding(Building building) {
            if (building.Name == "Barracks") {
                BarracksUI.SetActive(true);
            } else if (building.Name == "PowerPlant") {
                PowerplantUI.SetActive(true);
            }
        }

        protected void OnDeSelectedBuilding(Building building) {
            if (building.Name == "Barracks") {
                BarracksUI.SetActive(false);
            } else if (building.Name == "PowerPlant") {
                PowerplantUI.SetActive(false);
            }
        }

        private void OnEnable() {
            Building.OnSelected += OnSelectedBuilding;
            Building.OnDeSelected += OnDeSelectedBuilding;
        }

        private void OnDisable() {
            Building.OnSelected -= OnSelectedBuilding;
            Building.OnDeSelected -= OnDeSelectedBuilding;
        }
    }
}