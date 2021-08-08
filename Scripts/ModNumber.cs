using System;
using System.Collections.Generic;
using UnityEngine;
using MyEditorTools;
using System.Collections.Specialized;
using System.Linq;

namespace Renumbrance
{

    [Serializable]
    public class ModNumber : IEquatable<ModNumber>
    {
        #region Value Fields

        [SerializeField]
        private double baseValue;
        [SerializeField]
        private double maxValue;
        [SerializeField]
        private double currentValue;
        [SerializeField]
        private double delta;
        [SerializeField]
        [Tooltip("Updates inspector fields on every operator calculation but will be disabled in build due to performance.")]
        private bool debugMode;
        #endregion

        #region getter setter

        //Why accessing the last of the dictionary when it could be stored separately
        public double Value
        {
            get
            {
                if (gotModified)
                    ApplyModifications();

                ApplyDeltas();

                return currentValue;
            }
        }

        #endregion

        /// <summary>
        /// Keeps track of modified status, if the ModNumber has been manipulated through any of its stages.
        /// </summary>
        private bool gotModified;

        /// <summary>
        /// Keeps track of modified status, if the ModNumber has been manipulated through any of its stages.
        /// </summary>
        private bool isDeltaModified;

        /// <summary>
        /// Stores the values of each given stage to prevent recalculation everytime when accessing a stage.
        /// </summary>
        private SortedDictionary<uint, double> stagedValues;

        /// <summary>
        /// Keeps track of any applied modification. These values are essential for recalculating the true value of a modNumber.
        /// </summary>
        private SortedDictionary<uint, Dictionary<ModifierOperator, List<double>>> modifications;

        /// <summary>
        /// Defines in which order the operators of any stage should be applied.
        /// </summary>
        private ModifierOperator[] operatorOrder = new ModifierOperator[] {
            ModifierOperator.ADD,
            ModifierOperator.SUBSTRACT,
            ModifierOperator.MULTIPLY,
            ModifierOperator.DIVIDE,
            ModifierOperator.SET
        };

        #region Functionality

        /// <summary>
        /// Initialize ModNumber with base value
        /// </summary>
        /// <param name="baseValue">The initial base value of this number. This is the origin point of each stage modification.</param>
        public ModNumber(double baseValue)
        {
            this.baseValue = baseValue;
            gotModified = false;

            ClearModifications();
            ClearValues();
            ApplyModifications();
            ApplyDeltas();
        }

        /// <summary>
        /// Performs a basic operator modification on any given stage.
        /// </summary>
        /// <param name="phase">The stage which will be modified</param>
        /// <param name="modOperator">The applied operator to modify the given stage</param>
        /// <param name="modValue">The applied value to modify the given stage</param>
        public void Modify(uint stage, ModifierOperator modOperator, double modValue)
        {
            if (!modifications.ContainsKey(stage))
                modifications.Add(stage, CreateEmptyStage());

            gotModified = true;
            modifications[stage][modOperator].Add(modValue);

            if (debugMode)
            {
                ApplyModifications();
                ApplyDeltas();
            }
        }

        /// <summary>
        /// Removes a modification from any given stage if there is any.
        /// </summary>
        /// <param name="stage">The stage on which a modification should be removed</param>
        /// <param name="modOperator">The type of operator modification which should be removed</param>
        /// <param name="modValue">The modification value which should be removed</param>
        public void RemoveModification(uint stage, ModifierOperator modOperator, double modValue)
        {
            if (!modifications.ContainsKey(stage))
                return;

            gotModified = true;
            modifications[stage][modOperator].Remove(modValue);
        }

        private Dictionary<ModifierOperator, List<double>> CreateEmptyStage()
        {
            Dictionary<ModifierOperator, List<double>> newStage = new Dictionary<ModifierOperator, List<double>>();

            foreach (ModifierOperator modOperator in operatorOrder)
                newStage.Add(modOperator, new List<double>());

            return newStage;
        }

        /// <summary>
        /// Clears the values of all stages and reinitialize the base value on stage zero.
        /// </summary>
        public void ClearValues()
        {
            stagedValues = new SortedDictionary<uint, double>();
            stagedValues.Add(0, baseValue);
        }

        /// <summary>
        /// Remove all performed modifications.
        /// </summary>
        public void ClearModifications()
        {
            modifications = new SortedDictionary<uint, Dictionary<ModifierOperator, List<double>>>();
            gotModified = true;
        }

        /// <summary>
        /// Whenever a ModNumber gets modified by deltas through default operators
        /// it will get recalculated here. This is separated from the usual modifications
        /// to prevent often heavy calculations.
        /// </summary>
        private void ApplyDeltas()
        {
            currentValue = maxValue + delta;
        }


        /// <summary>
        /// Whenever a ModNumber gets accessed and has been marked as "isModified",
        /// the Modnumber recalculates itself by applying all modifications on the
        /// proper stages with the proper operators.
        /// </summary>
        private void ApplyModifications()
        {
            gotModified = false;

            ClearValues();

            double stagedValue = baseValue;

            for (uint stage = 0; stage < modifications.Keys.Count; stage++)
            {
                double tempStageValue = stagedValue;

                foreach (ModifierOperator modOperator in operatorOrder)
                {
                    foreach (double modValue in modifications[stage][modOperator])
                    {

                        switch (modOperator)
                        {
                            case ModifierOperator.ADD:
                                stagedValue += modValue;
                                break;
                            case ModifierOperator.SUBSTRACT:
                                stagedValue -= modValue;
                                break;
                            case ModifierOperator.MULTIPLY:
                                stagedValue += ((tempStageValue * modValue) - tempStageValue);
                                break;
                            case ModifierOperator.DIVIDE:
                                stagedValue += ((tempStageValue / modValue) - tempStageValue);
                                break;
                            case ModifierOperator.SET:
                                stagedValue = modValue;
                                break;
                        }
                    }
                }

                stagedValues[stage] = stagedValue;
            }

            maxValue = stagedValue;
        }

        #endregion

        #region Operators

        //Check what this is doin?!
        public static implicit operator ModNumber(double a)
        {
            return new ModNumber(a);
        }

        public static ModNumber operator +(ModNumber a, double b)
        {
            a.delta += b;
            a.ApplyDeltas();
            return a;
        }

        public static ModNumber operator ++(ModNumber a)
        {
            a.delta++;
            a.ApplyDeltas();
            return a;
        }

        public static ModNumber operator -(ModNumber a, double b)
        {
            a.delta -= b;
            a.ApplyDeltas();
            return a;
        }

        public static ModNumber operator --(ModNumber a)
        {
            a.delta--;
            a.ApplyDeltas();
            return a;
        }

        public static ModNumber operator *(ModNumber a, double b)
        {
            double currentValue = a.Value;

            a.delta += ((currentValue * b) - currentValue);
            a.ApplyDeltas();

            return a;
        }

        public static ModNumber operator /(ModNumber a, double b)
        {
            double currentValue = a.Value;

            a.delta += ((currentValue / b) - currentValue);
            a.ApplyDeltas();

            return a;
        }

        #endregion

        #region Comparables & Equitables

        /// <summary>
        /// Equal comparison will compare on the current value, not the object
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ModNumber other)
        {
            return other.Value == Value;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Operator equal will check on current value not on object reference
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator ==(ModNumber obj1, ModNumber obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Operator equal will check on current value not on object reference
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public static bool operator !=(ModNumber obj1, ModNumber obj2)
        {
            return !(obj1 == obj2);
        }

        #endregion

        #region Basic Conversion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            //return modifications.Sum(el => el.Value.Sum(el2 => el2.Value.Count)).ToString() + " modifications found. \n Staged Values: " + String.Join(", ", stagedValues);
            return Value.ToString();
        }

        //Check waht this is doin exactly
        //Let the 
        public static implicit operator float(ModNumber n) => (float)n.Value;
        public static explicit operator int(ModNumber n) => (int)n.Value;
        public static implicit operator byte(ModNumber n) => (byte)n.Value;

        #endregion
    }

    public enum ModifierOperator
    {
        ADD,
        SUBSTRACT,
        MULTIPLY,
        DIVIDE,
        SET
    }
}