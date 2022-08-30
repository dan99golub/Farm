using UnityEngine;

namespace PowerUps
{
    [CreateAssetMenu(fileName = "PowerUp Id", menuName = "Game/Power Up", order = 51)]
    public class PowerUp : ScriptableObject
    {
        [System.Serializable]
        public class Data
        {
            public PowerUp TypePowerUp;
            [SerializeReference] public Life LifePowerUp;
            public ReplaceMod ReaplcaeMethod;

            public void Update()
            {
                LifePowerUp.Update(Time.deltaTime);
            }
            
            [System.Serializable]
            public class ReplaceMod
            {
                public Mod PowerUpMod;

                public bool Replace(Data data, PowerUpContainer powerUpContainer)
                {
                    switch (PowerUpMod)
                    {
                        case Mod.Replace:
                            powerUpContainer.Replace(data);
                            return true;
                        case Mod.AddIfFree:
                            if (powerUpContainer.GetData(data.TypePowerUp) == null)
                            {
                                powerUpContainer.TryAdd(data);
                                return true;
                            }
                            return false;
                        case Mod.Zero:
                            var dataInContainer = powerUpContainer.GetData(data.TypePowerUp);
                            if (dataInContainer != null)
                            {
                                var t = dataInContainer.LifePowerUp as TimerLife;
                                if(t != null) t.Zero();
                                else powerUpContainer.Replace(data);
                            }
                            else
                            {
                                powerUpContainer.Replace(data);
                            }

                            return true;
                            break;
                        default:
                            return false;
                    }
                }
                
                public enum Mod
                {
                    Replace, Zero, AddIfFree
                }
            }
            
            [System.Serializable]
            public abstract class Life
            {
                public abstract void Update(float deltaTime);

                public abstract bool IsEnd();
            }
            
            [System.Serializable]
            public class TimerLife : Life
            {
                [SerializeField] private float _fullTime;
                [SerializeField] private float _current;

                public float Current => _current;
                public float FullTime => _fullTime;
                public float Normal => Mathf.Clamp(_current / _fullTime, 0, 1);
                
                public event System.Action Updated;
                
                public override void Update(float deltaTime)
                {
                    _current += deltaTime;
                    Updated?.Invoke();
                }

                public override bool IsEnd() => _current > _fullTime;

                public void Zero()
                {
                    _current = 0;
                    Updated?.Invoke();
                }
            }
        }
    }
}