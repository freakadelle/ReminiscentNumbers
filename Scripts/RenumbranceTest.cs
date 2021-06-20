using System;
using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEngine;
using MyEditorTools;

namespace Renumbrance.Editor
{

    public class RenumbranceTest : MonoBehaviour, IComponentTest
    {
        public Member health;

        public void Start()
        {
            Test();
        }

        public void Test()
        {
            health = 100;                           //Initialize Member with basevalue 100

            health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
            Debug.Log(health);                      //Base: 100  current: 110

            health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
            Debug.Log(health);                      //Base: 100  current: 120

            health.Modify(ModType.MULTIPLY, 1.1);   //Multiply by 10% from base
            Debug.Log(health);                      //Base: 100  current: 130

            health += 100;                          //Increase base Value by another 100
            Debug.Log(health);                      //Base: 200  current: 260

            health.Modify(ModType.ADD, 40);         //Add 40
            Debug.Log(health);                      //Base: 200  current: 300

            health /= 10;                           //Divide Base Value by 10
            Debug.Log(health);                      //Base: 20  current: 66
        }

        private IEnumerator TestRoutine()
        {
            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.MULTIPLY, 1.1); // 10
            Debug.Log(health);

            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.MULTIPLY, 1.2); //30
            Debug.Log(health);


            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.MULTIPLY, 1.3); //60
            Debug.Log(health);

            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.MULTIPLY, 1.4); //100
            Debug.Log(health);

            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.MULTIPLY_BASE, 2);
            Debug.Log(health);

            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.ADD_BASE, 2);
            Debug.Log(health);

            yield return new WaitForSecondsRealtime(1);
            health.Modify(ModType.ADD, 2);
            Debug.Log(health);
        }
    }

    public class TestToolNonInspectorClass
    {
        public Member health = 0;

        public void Print()
        {
            Debug.Log("Non editor Class " + health);

            health += 2;

            health = (health + 2);

            Debug.Log("Non editor Class " + health);
        }

        public void SetHealth(float value)
        {

        }
    }

    [Serializable]
    public struct TestToolNonInspectorStruct
    {
        public float foaty;
        public Member health;

        public double Foaty
        {
            get
            {
                return foaty;
            }
        }

        public void Print()
        {
            Debug.Log("Non editor Class " + health);

            health += 2;

            Debug.Log("Non editor Class " + health);
        }

        public void SetFloaty(float x)
        {
            foaty += x;
        }

    }
}