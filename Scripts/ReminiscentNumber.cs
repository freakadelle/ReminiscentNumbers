using System;
using System.Collections.Generic;

namespace Renumbrance
{

    public struct ReminiscentNumber : IEquatable<ReminiscentNumber>
    {

        private long value;
        public long Value
        {
            get
            {
                if (isModified)
                    ApplyModifications();

                return value;
            }
        }

        private readonly long baseValue;
        public long BaseValue => baseValue;

        private bool isModified;

        private Dictionary<OperatorModification, List<long>> modifications;

        private readonly OperatorModification[] operatorOrder;

        public ReminiscentNumber(long baseValue)
        {
            value = baseValue;

            this.baseValue = baseValue;

            modifications = new Dictionary<OperatorModification, List<long>>()
            {
                { OperatorModification.ADD, new List<long>() },
                { OperatorModification.SUBTRACT, new List<long>() },
                { OperatorModification.MULTIPLY, new List<long>() },
                { OperatorModification.DEVIDE, new List<long>() },
                { OperatorModification.SET, new List<long>() }
            };

            operatorOrder = new OperatorModification[]
            {
                OperatorModification.ADD,
                OperatorModification.SUBTRACT,
                OperatorModification.MULTIPLY,
                OperatorModification.DEVIDE,
                OperatorModification.SET,
            };

            isModified = false;
        }

        public void AddModification(OperatorModification modtype, long modificator)
        {
            isModified = true;
            modifications[modtype].Add(modificator);
        }

        public void RemoveModification(OperatorModification modtype, long modificator)
        {
            isModified = true;
            modifications[modtype].Remove(modificator);
        }

        public void RemoveAllModifications()
        {
            modifications = new Dictionary<OperatorModification, List<long>>()
            {
                { OperatorModification.ADD, new List<long>() },
                { OperatorModification.SUBTRACT, new List<long>() },
                { OperatorModification.MULTIPLY, new List<long>() },
                { OperatorModification.DEVIDE, new List<long>() },
                { OperatorModification.SET, new List<long>() }
            };
        }

        private void ApplyModifications()
        {
            isModified = false;

            value = baseValue;

            for (int oi = 0; oi < operatorOrder.Length; oi++)
            {
                for(int mi = 0; mi < modifications[(OperatorModification)oi].Count; mi++)
                {
                    switch ((OperatorModification)oi)
                    {
                        case OperatorModification.ADD:
                            value += modifications[(OperatorModification)oi][mi];
                            break;
                        case OperatorModification.SUBTRACT:
                            value -= modifications[(OperatorModification)oi][mi];
                            break;
                        case OperatorModification.MULTIPLY:
                            value *= modifications[(OperatorModification)oi][mi];
                            break;
                        case OperatorModification.DEVIDE:
                            value /= modifications[(OperatorModification)oi][mi];
                            break;
                        case OperatorModification.SET:
                            value = modifications[(OperatorModification)oi][mi];
                            break;
                    }
                }
            }
        }

        #region Operators

        public static implicit operator ReminiscentNumber(long a)
        {
            return new ReminiscentNumber(a);
        }

        public static ReminiscentNumber operator +(ReminiscentNumber a, long b)
        {
            a.AddModification(OperatorModification.ADD, b);
            return a;
        }

        public static ReminiscentNumber operator ++(ReminiscentNumber a)
        {
            a.AddModification(OperatorModification.ADD, 1);
            return a;
        }

        public static ReminiscentNumber operator -(ReminiscentNumber a, long b)
        {
            a.AddModification(OperatorModification.SUBTRACT, b);
            return a;
        }

        public static ReminiscentNumber operator --(ReminiscentNumber a)
        {
            a.AddModification(OperatorModification.SUBTRACT, 1);
            return a;
        }

        public static ReminiscentNumber operator *(ReminiscentNumber a, long b)
        {
            a.AddModification(OperatorModification.MULTIPLY, b);
            return a;
        }

        public static ReminiscentNumber operator /(ReminiscentNumber a, long b)
        {
            a.AddModification(OperatorModification.DEVIDE, b);
            return a;
        }

        #endregion

        #region Comparables & Equitables

        public static bool operator ==(ReminiscentNumber obj1, ReminiscentNumber obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(ReminiscentNumber obj1, ReminiscentNumber obj2)
        {
            return !(obj1 == obj2);
        }

        public bool Equals(ReminiscentNumber other)
        {
            return other.value == value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        #endregion

        #region Basic overrides

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"Base: {baseValue}  current: {Value}";
        }

        #endregion
    }
    public struct NumberModification
    {
        public OperatorModification operatorType;
        public long modificator;

        public NumberModification(OperatorModification operatorType, long modificator)
        {
            this.operatorType = operatorType;
            this.modificator = modificator;
        }
    }

    public enum OperatorModification
    {
        ADD,
        SUBTRACT,
        MULTIPLY,
        DEVIDE,
        SET
    }
}