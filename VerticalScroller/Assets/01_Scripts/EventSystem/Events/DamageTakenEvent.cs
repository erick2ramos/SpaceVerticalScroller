using UnityEngine;
using System.Collections;
using BaseSystems.EventSystem;

namespace GameplayLogic.Events
{
    
    /// <summary>
    /// Event triggered every time a character suffers damage to its health component
    /// </summary>
    public struct DamageTakenEvent
    {
        public int DamageAmount;
        public int PreviousHealth;
        public int CurrentHealth;
        public Character DamagedActor;

        public DamageTakenEvent(int damageAmount, int previousHealth, int currentHealth, Character damagedActor)
        {
            DamageAmount = damageAmount;
            PreviousHealth = previousHealth;
            CurrentHealth = currentHealth;
            DamagedActor = damagedActor;
        }

        static DamageTakenEvent e;

        public static void Trigger(int damageAmount, int previousHealth, int currentHealth, Character damagedActor)
        {
            e.DamageAmount = damageAmount;
            e.PreviousHealth = previousHealth;
            e.CurrentHealth = currentHealth;
            e.DamagedActor = damagedActor;
            EventManager.Trigger(e);
        }
    }
}