using System;
using System.Collections.Generic;
using UnityEngine;
using MyEditorTools;

namespace Renumbrance
{

    [Serializable]
    public struct Member : IEquatable<Member>
    {
        [ReadOnly]
        [SerializeField]
        private double value;

        public double Value
        {
            get
            {
                if (isModified)
                    ApplyModifications();

                return value;
            }
        }

        [SerializeField]
        private double baseValue;

        public readonly double BaseValue => baseValue;

        private bool isModified;

        private Dictionary<ModType, List<double>> modifications;

        private ModType[] operatorOrder;

        #region Functionality

        public Member(double baseValue)
        {
            value = baseValue;
            this.baseValue = baseValue;
            isModified = false;

            operatorOrder = (ModType[])Enum.GetValues(typeof(ModType));

            modifications = new Dictionary<ModType, List<double>>();

            foreach(ModType modType in operatorOrder)
                modifications.Add(modType, new List<double>());
        }

        public void Modify(ModType modtype, double modificator)
        {
            if (modifications == null)
                ClearModifications();

            isModified = true;
            modifications[modtype].Add(modificator);

            ApplyModifications();
        }

        public void RemoveModification(ModType modtype, double modificator)
        {
            if (modifications == null)
                ClearModifications();

            isModified = true;
            modifications[modtype].Remove(modificator);
        }

        public void ClearOperatorOrder()
        {
            operatorOrder = (ModType[])Enum.GetValues(typeof(ModType));
        }

        public void ClearModifications()
        {
            if (operatorOrder == null || operatorOrder.Length <= 0)
                ClearOperatorOrder();

            modifications = new Dictionary<ModType, List<double>>();

            foreach (ModType modType in operatorOrder)
                modifications.Add(modType, new List<double>());

            isModified = true;

            ApplyModifications();
        }

        private void ApplyModifications()
        {
            isModified = false;

            double tempBaseValue = baseValue;
            value = baseValue;

            for (int oi = 0; oi < operatorOrder.Length; oi++)
            {
                for(int mi = 0; mi < modifications[(ModType)oi].Count; mi++)
                {
                    switch ((ModType)oi)
                    {
                        case ModType.ADD_BASE:
                            tempBaseValue += modifications[(ModType)oi][mi];
                            break;
                        case ModType.MULTIPLY_BASE:
                            tempBaseValue *= modifications[(ModType)oi][mi];
                            break;
                        case ModType.ADD:
                            value += modifications[(ModType)oi][mi];
                            break;
                        case ModType.MULTIPLY:
                            value += ((tempBaseValue * modifications[(ModType)oi][mi]) - tempBaseValue);
                            break;
                        case ModType.SET:
                            value = modifications[(ModType)oi][mi];
                            break;
                    }
                }
            }
        }

        #endregion

        #region Operators

        public static implicit operator Member(double a)
        {
            return new Member(a);
        }

        public static Member operator +(Member a, double b)
        {
            //a.AddModification(OperatorModification.ADD, b);
            a.baseValue += b;
            a.isModified = true;
            return a;
        }

        public static Member operator ++(Member a)
        {
            //a.AddModification(OperatorModification.ADD, 1);
            a.baseValue++;
            a.isModified = true;
            return a;
        }

        public static Member operator -(Member a, double b)
        {
            //a.AddModification(OperatorModification.SUBTRACT, b);
            a.baseValue -= b;
            a.isModified = true;
            return a;
        }

        public static Member operator --(Member a)
        {
            //a.AddModification(OperatorModification.SUBTRACT, 1);
            a.baseValue--;
            a.isModified = true;
            return a;
        }

        public static Member operator *(Member a, double b)
        {
            //a.AddModification(OperatorModification.MULTIPLY, b);
            a.baseValue *= b;
            a.isModified = true;
            return a;
        }

        public static Member operator /(Member a, double b)
        {
            //a.AddModification(OperatorModification.DEVIDE, b);
            a.baseValue /= b;
            a.isModified = true;
            return a;
        }

        #endregion

        #region Comparables & Equitables

        public static bool operator ==(Member obj1, Member obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(Member obj1, Member obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(Member other)
        {
            return other.value == value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion

        #region Basic Conversion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Base: {baseValue}  current: {Value}";
        }

        public static implicit operator float(Member n) => (float)n.Value;
        public static explicit operator int(Member n) => (int)n.Value;
        public static implicit operator byte(Member n) => (byte)n.Value;

        #endregion
    }

    public struct NumberModification
    {
        public ModType operatorType;
        public long modificator;

        public NumberModification(ModType operatorType, long modificator)
        {
            this.operatorType = operatorType;
            this.modificator = modificator;
        }
    }

    public enum ModType
    {
        ADD_BASE,
        MULTIPLY_BASE,
        ADD,
        MULTIPLY,
        SET
    }
}