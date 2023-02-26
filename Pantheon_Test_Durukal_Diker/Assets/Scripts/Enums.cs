using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class Enums {
        public enum BuildCodes {
            PowerPlant = 1,
            Barracks = 2
        }

        public enum Soldiers {
            Peasant = 3,
            Infantry = 4,
            Knight = 5
        }

        public enum UnitDamages {
            Peasant = 2,
            Infantry = 5,
            Knight = 10
        }

        public enum UnitHealts {
            Peasant = 10,
            Infantry = 10,
            Knight = 10
        }

        public enum BuildingHealth {
            PowerPlant = 50,
            Barracks = 100
        }

        public enum Factions {
            Allies = 1,
            Enemies = 2
        }
    }
}