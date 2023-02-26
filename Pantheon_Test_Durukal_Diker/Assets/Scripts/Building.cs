using System.Collections;
using UnityEngine;
using System;

namespace Assets.Scripts {
    public class Building : Entity {
        public static Action<Building> OnSelected;
        public static Action<Building> OnDeSelected;
        public Vector2Int Dimensions;
        public bool IsBuilt = false;
        

        
        public void Selected() {
            if (!IsAI) {
                OnSelected?.Invoke(this);
            }
        }

        public void Deselected() {
            if (!IsAI) {
                OnDeSelected?.Invoke(this);
            }
        }
    }
}