using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProtectedBool
{
    private DataEncryption.CustomFloat value;

    public ProtectedBool(bool flag)
    {
        SetValue(flag);
    }

    public bool GetValue()
    {
        float val = value.GetValue();

        if (Mathf.Abs(val - 1) < Mathf.Epsilon)
        {
            return true;
        }

        return false;
    }

    public void SetValue(bool flag)
    {
        int kVal = flag ? 1 : 0;

        value = new DataEncryption.CustomFloat(kVal);
    }
}

public class DataEncryption
{
   

    public struct CustomFloat
    {
        private float offset;
        private float value;

        public CustomFloat(float value = 0)
        {
            System.Random randomDirection = new System.Random();

            offset = randomDirection.Next(-5000, +5000);
            this.value = value + offset;
        }

        public float GetValue()
        {
            return value - offset;
        }

        public void Dispose()
        {
            offset = 0;
            value = 0;
        }

        public override string ToString()
        {
            return GetValue().ToString();
        }

        public static CustomFloat operator +(CustomFloat f1, CustomFloat f2)
        {
            return new CustomFloat(f1.GetValue() + f2.GetValue());
        }

        public static CustomFloat operator -(CustomFloat f1, CustomFloat f2)
        {
            return new CustomFloat(f1.GetValue() - f2.GetValue());
        }

        public static CustomFloat operator *(CustomFloat f1, CustomFloat f2)
        {
            return new CustomFloat(f1.GetValue() * f2.GetValue());
        }
    }
}
