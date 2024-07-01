using Assets.Scripts.Game.Core.BoardBase;
using Assets.Scripts.Game.Core.Enums;
using Assets.Scripts.Game.Core.Managers.PoolSystem;
using Assets.Scripts.Game.Core.NumberBase;
using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Game.Core.Managers
{
    public class ParticleManager : MonoBehaviour, IProvidable
    {
        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventManager.OnCellExploded += PlayParticle;
        }

        private void PlayParticle(Cell cell)
        {
            PlayParticleEffect(cell.Number);
        }

        private void UnsubscribeEvents()
        {
            EventManager.OnCellExploded -= PlayParticle;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        public void PlayParticleEffect(Number number)
        {
            string ParticlePoolID;

            switch (number.NumberData.NumberType)
            {
                case NumberType.Number1:
                    ParticlePoolID = "ParticleNumber1";
                    break;
                case NumberType.Number2:
                    ParticlePoolID = "ParticleNumber2";
                    break;
                case NumberType.Number3:
                    ParticlePoolID = "ParticleNumber3";
                    break;
                case NumberType.Number4:
                    ParticlePoolID = "ParticleNumber4";
                    break;
                case NumberType.Number5:
                    ParticlePoolID = "ParticleNumber5";
                    break;
                case NumberType.Unique:
                    ParticlePoolID = "ParticleUnique";
                    break;
                default:
                    return;
            }

            var particle = PoolingSystem.Instance.InstantiatePoolObject(ParticlePoolID, number.transform.position).GetComponent<ParticleSystem>();
            PoolingSystem.Instance.DestroyPoolObject(particle.gameObject, 1f);
            particle.Play(true);
        }
    }
}
