using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts {
    public class Unit : Entity {
        public static Action<Unit> OnSelected;
        public static Action<Unit> OnDeSelected;

        [SerializeField]
        private float MovementSpeed;

        [SerializeField]
        internal int Damage;

        [SerializeField]
        private GameObject _AITarget;

        public GameObject AITarget {
            get => _AITarget;
            set => _AITarget = value;
        }

        public bool IsSelected = false;

        [SerializeField]
        private float AttackCoolDown = 5f;

        internal float AttackTimer = 0f;


        public void OnTriggerStay2D(Collider2D collision) {
            var enemyEntity = collision.gameObject.GetComponent<Entity>();
            if (enemyEntity != null && enemyEntity.Faction != this.Faction) {
                if (enemyEntity.gameObject.GetComponent<Entity>().IsUnit) {
                    if (this.Faction == (int)Enums.Factions.Enemies) {
                        GetComponent<Pathfinding.AIDestinationSetter>().target = enemyEntity.transform;
                    }

                    DealDamage(enemyEntity, Damage);
                }

                if (!enemyEntity.IsDead && enemyEntity.gameObject.activeSelf) {
                    if (enemyEntity.gameObject.GetComponent<Entity>().IsBuilding) {
                        if (enemyEntity.gameObject.GetComponent<Building>().IsBuilt) {
                            DealDamage(enemyEntity, Damage);
                        }
                    }
                }
            }
        }

        private void DealDamage(Entity enemy, int damage) {
            if (AttackTimer > AttackCoolDown) {
                enemy.Takedamage(damage);
                AttackTimer = 0f;
            }
        }

        protected void TargetSet() {
            AITarget = GameObject.Find("Target").transform.Find("AITarget").gameObject;
        }
    }
}