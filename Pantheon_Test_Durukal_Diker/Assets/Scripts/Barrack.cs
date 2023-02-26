using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class Barrack : Building {
        private void Awake() {
            Health = (int)Enums.BuildingHealth.Barracks;
            CurrentHealth = Health;
            FactionCheck();
            IsBuilding = true;
        }

        // Start is called before the first frame update
        void Start() {
            //base.Start();
        }

        // Update is called once per frame
        void Update() { }

        public void OnTriggerEnter2D(Collider2D collision) {
            IsColliding = true;
        }

        public void OnTriggerExit2D(Collider2D collision) {
            IsColliding = false;
        }
    }
}