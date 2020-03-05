using UnityEngine;
using System.Collections;

namespace BaseSystems.EventSystem
{
    public struct DamageTakenEvent
    {
        public int DamageAmount;
        public int PreviousHealth;
        public int CurrentHealth;

        public DamageTakenEvent(int damageAmount, int previousHealth, int currentHealth)
        {
            DamageAmount = damageAmount;
            PreviousHealth = previousHealth;
            CurrentHealth = currentHealth;
        }

        static DamageTakenEvent e;

        public static void Trigger(int damageAmount, int previousHealth, int currentHealth)
        {
            e.DamageAmount = damageAmount;
            e.PreviousHealth = previousHealth;
            e.CurrentHealth = currentHealth;
            EventManager.Trigger(e);
        }
    }
}