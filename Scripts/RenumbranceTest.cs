using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using MyEditorTools;

namespace Renumbrance.Editor
{

    public class RenumbranceTest : MonoBehaviour, IComponentTest
    {
        public ModNumber health = new ModNumber(100);

        public void Start()
        {
            Test();
        }

        public void Test()
        {
            health = new ModNumber(100);

            SomeDoubleFunction(health);

            Debug.Log("Compare against numbers: " + (health == 100));

            Debug.Log("Initial base value: " + health);

            health.Modify((uint)HealthPointStages.BASE, ModifierOperator.ADD, 10);

            Debug.Log("Modify base value stage 0 [Add 10]: " + health);

            health.Modify((uint)HealthPointStages.ATTRIBUTES, ModifierOperator.MULTIPLY, 2);

            Debug.Log("Modify stage 1 [Mult 2]: " + health);

            health.Modify((uint)HealthPointStages.GEAR, ModifierOperator.ADD, 150);

            Debug.Log("Modify stage 2 [ADD 150]: " + health);

            health.RemoveModification((uint)HealthPointStages.GEAR, ModifierOperator.ADD, 150);

            Debug.Log("REMOVE stage 2 Mod: " + health);

            health.Modify((uint)HealthPointStages.BUFFS, ModifierOperator.DIVIDE, 2);

            Debug.Log("Modify stage 3 [DIV 2]: " + health);

            health.Modify((uint)HealthPointStages.MAXHP, ModifierOperator.MULTIPLY, 1.1f);

            Debug.Log("Modify final stage [MULT 1.1f]: " + health);

            Debug.Log("Use common operators to change the current Value (current Value = after all modifications happened)");

            health -= 35;

            Debug.Log("Substract 35 on current value: " + health);
        }

        private void SomeDoubleFunction(double value)
        {
            Debug.Log("Use as common number.");
        }

        public enum HealthPointStages
        {
            BASE = 0,
            ATTRIBUTES = 1,
            GEAR = 2,
            BUFFS = 3,
            MAXHP = 4
        }
    }
}