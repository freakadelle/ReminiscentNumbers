using UnityEngine;

namespace Renumbrance
{

    public class TestingTool : MonoBehaviour
    {
        ReminiscentNumber number1;

        public void Test()
        {
            //Define a base value
            number1 = 10;

            Debug.Log(number1.ToString());

            number1 *= 2;

            Debug.Log(number1.ToString());

            number1++;
            Debug.Log(number1.ToString());

            number1 *= 3;
            Debug.Log(number1.ToString());

            number1++;
            Debug.Log(number1.ToString());

            number1++;

            //number1--;

            //Debug.Log(number1.ToString());

            //number1 *= 5;

            //Debug.Log(number1.ToString());

            //number1 /= 2;

            //Debug.Log(number1.ToString() + " " + number1.BaseValue);


        }
    }
}