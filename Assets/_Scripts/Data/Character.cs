﻿using System;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Data
{
    public class Character : MonoBehaviour
    {
        private Animator _animator;
        public string name;
        public int level;
        public Stats stats;
        public Ability[] abilities;

        public Transform attackPos;
        [HideInInspector] public Vector3 originalPos;

        private void OnValidate()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            originalPos = transform.position;
        }

        public void SetCharacter(string name, int level, Stats stats)
        {
            this.name = name;
            this.level = level;
            this.stats = stats;
        }

        public void PlayAnimation(AnimationType animationType, AttackType attackType = AttackType.HighAttack)
        {
            switch (animationType)
            {
                case AnimationType.Idle:
                    _animator.Play("Idle");
                    break;
                case AnimationType.Attack:
                    switch (attackType)
                    {
                        //Todo Attack Punch 1 2 3
                        //Todo Attack Kick 1 2


                        case AttackType.HighAttack:
                            _animator.Play("Attack Main Hand 1");
                            break;
                        case AttackType.MiddleAttack:
                            _animator.Play("Attack Main Hand 2");
                            break;
                        case AttackType.LowAttack:
                            _animator.Play("Attack Main Hand 3");
                            break;
                        case AttackType.Buff:
                            _animator.Play("Cast 1");
                            break;
                        case AttackType.DirectSkill:
                            _animator.Play("Attack Punch 1");
                            break;
                        case AttackType.SkillFromDown:
                            _animator.Play("Attack Kick 1");
                            break;
                        case AttackType.SkillFromUp:
                            _animator.Play("Cast 2");
                            break;
                        case AttackType.Debuf:
                            _animator.Play("Cast 1");
                            break;
                        default:
                            _animator.Play("Cast 2");
                            break;
                    }

                    break;
                case AnimationType.TakeDamage:
                    _animator.Play("Hit");
                    break;
                case AnimationType.Die:
                    _animator.Play("Die");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }

        public void TakeDamage(int damage)
        {
            Debug.Log(this.name + " takes " + damage + " damage");
            stats.health -= damage;
            PlayAnimation(AnimationType.TakeDamage);
        }

        public async Task Attack(Character target, Ability ability)
        {
            Debug.Log(this.name + " attacks " + target.name);
            int damage = CalculateDamage(ability); // Calculate damage based on ability
            PlayAnimation(AnimationType.Attack, ability.abilityType);

            // Pass target's original position to CreateAbility and await the execution
            await ExecuteAbility(ability, target.transform);
            target.TakeDamage(damage);
            // You can continue with other logic here without waiting for ability completion.
        }

        private int CalculateDamage(Ability ability) => 10;

        private async Task MoveSkillToTarget(GameObject skill, Vector3 targetPos, float duration)
        {
            await skill.transform.DOMove(targetPos, duration).AsyncWaitForCompletion();
            Destroy(skill);
        }

        private async Task MoveSkillFromUp(GameObject skill, Vector3 targetPos, float duration)
        {
            await skill.transform.DOMove(targetPos + (Vector3.up * 10), duration).AsyncWaitForCompletion();
            Destroy(skill);
        }

        private async Task ExecuteAbility(Ability ability, Transform targetOriginalPos)
        {
            var skillInWorldSpace = Instantiate(ability.abilityPrefab,
                transform.position + ability.startOffSet,
                Quaternion.identity);

            await ability.PlayAbilityAnimation(skillInWorldSpace.transform, targetOriginalPos);
            Destroy(skillInWorldSpace);

            // switch (ability.abilityType)
            // {
            //     case AttackType.HighAttack:
            //         break;
            //     case AttackType.MiddleAttack:
            //         break;
            //     case AttackType.LowAttack:
            //         break;
            //     case AttackType.DirectSkill:
            //         await MoveSkillToTarget(skillInWorldSpace, targetOriginalPos, ability.abilitySpeed);
            //         break;
            //     case AttackType.SkillFromUp:
            //         break;
            //     case AttackType.SkillFromDown:
            //         break;
            //     case AttackType.Buff:
            //         break;
            //     case AttackType.Debuf:
            //         break;
            //     default:
            //         throw new ArgumentOutOfRangeException();
            // }
        }


        public bool IsDead() => stats.health <= 0;
    }

    public enum AnimationType
    {
        Idle,
        Attack,
        TakeDamage,
        Die
    }
}