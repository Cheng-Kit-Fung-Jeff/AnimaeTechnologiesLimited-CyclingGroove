using UnityEngine;
using UnityEngine.Events;

public class CKF_Math : MonoBehaviour
{
    public float value = 0;

    public UnityEvent<float> getValue = new();

    public void Set(float value) { this.value = value; }
    public void SetInverse(float value) { this.value = 1/value; }
    public void SetSin(float value) { this.value = Mathf.Sin(value); }
    public void SetCos(float value) { this.value = Mathf.Cos(value); }
    public void SetTan(float value) { this.value = Mathf.Tan(value); }
    public void SetSign(float value) { this.value = Mathf.Sign(value); }
    public void SetSqrt(float value) { this.value = Mathf.Sqrt(value); }
    public void SetRound(float value) { this.value = Mathf.Round(value); }
    public void SetRad2Deg(float value) { this.value = value * Mathf.Rad2Deg; }
    public void SetDeg2Rad(float value) { this.value = value * Mathf.Deg2Rad; }
    public void SetPerlinNoise1D(float value) { this.value = Mathf.PerlinNoise1D(value); }
    public void SetNextPowerOf2(float value) { this.value = Mathf.NextPowerOfTwo((int)value); }
    public void SetLog10(float value) { this.value = Mathf.Log10(value); }
    public void SetExp(float value) { this.value = Mathf.Exp(value); }
    public void SetGammaToLinearSpace(float value) { this.value = Mathf.GammaToLinearSpace(value); }
    public void SetClamp01(float value) { this.value = Mathf.Clamp01(value); }
    public void SetCeil(float value) { this.value = Mathf.Ceil(value); }
    public void SetFloor(float value) { this.value = Mathf.Floor(value); }
    public void SetAsin(float value) { this.value = Mathf.Asin(value); }
    public void SetAcos(float value) { this.value = Mathf.Acos(value); }
    public void SetAtan(float value) { this.value = Mathf.Atan(value); }
    public void SetAbs(float value) { this.value = Mathf.Abs(value); }

    public void ApplyInverse() { value = 1 / value; }
    public void ApplySin() { value = Mathf.Sin(value); }
    public void ApplyCos() { value = Mathf.Cos(value); }
    public void ApplyTan() { value = Mathf.Tan(value); }
    public void ApplySign() { value = Mathf.Sign(value); }
    public void ApplySqrt() { value = Mathf.Sqrt(value); }
    public void ApplyRound() { value = Mathf.Round(value); }
    public void ApplyRad2Deg() { value *= Mathf.Rad2Deg; }
    public void ApplyDeg2Rad() { value *= Mathf.Deg2Rad; }
    public void ApplyPerlinNoise1D() { value = Mathf.PerlinNoise1D(value); }
    public void ApplyNextPowerOf2() { value = Mathf.NextPowerOfTwo((int)value);}
    public void ApplyLog10() { value = Mathf.Log10(value); }
    public void ApplyExp() { value = Mathf.Exp(value); }
    public void ApplyGammaToLinearSpace() { value = Mathf.GammaToLinearSpace(value); }
    public void ApplyClamp01() { value = Mathf.Clamp01(value); }
    public void ApplyCeil() { value = Mathf.Ceil(value); }
    public void ApplyFloor() { value = Mathf.Floor(value); }
    public void ApplyAsin() { value = Mathf.Asin(value); }
    public void ApplyAcos() { value = Mathf.Acos(value); }
    public void ApplyAtan() { value = Mathf.Atan(value); }
    public void ApplyAbs() { value = Mathf.Abs(value); }

    public void Get() { getValue?.Invoke(value); }
    public void GetInverse(float value) { getValue?.Invoke(1/value);}
    public void GetSin(float value) { getValue?.Invoke(Mathf.Sin(value)); }
    public void GetCos(float value) { getValue?.Invoke(Mathf.Cos(value)); }
    public void GetTan(float value) { getValue?.Invoke(Mathf.Tan(value)); }
    public void GetSign(float value) { getValue?.Invoke(Mathf.Sign(value)); }
    public void GetSqrt(float value) { getValue?.Invoke(Mathf.Sqrt(value)); }
    public void GetRound(float value) { getValue?.Invoke(Mathf.Round(value)); }
    public void GetRad2Deg(float value) { getValue?.Invoke(Mathf.Rad2Deg*value); }
    public void GetDeg2Rad(float value) { getValue?.Invoke(Mathf.Deg2Rad*value); }
    public void GetPerlinNoise1D(float value) { getValue?.Invoke(Mathf.PerlinNoise1D(value)); }
    public void GetNextPowerOf2(float value) { getValue?.Invoke(Mathf.NextPowerOfTwo((int)value)); }
    public void GetLog10(float value) { getValue?.Invoke(Mathf.Log10(value)); }
    public void GetExp(float value) { getValue?.Invoke(Mathf.Exp(value)); }
    public void GetGammaToLinearSpace(float value) { getValue?.Invoke(Mathf.GammaToLinearSpace(value)); }
    public void GetClamp01(float value) { getValue?.Invoke(Mathf.Clamp01(value)); }
    public void GetCeil(float value) { getValue?.Invoke(Mathf.Ceil(value)); }
    public void GetFloor(float value) { getValue?.Invoke(Mathf.Floor(value)); }
    public void GetAsin(float value) { getValue?.Invoke(Mathf.Asin(value)); }
    public void GetAcos(float value) { getValue?.Invoke(Mathf.Acos(value)); }
    public void GetAtan(float value) { getValue?.Invoke(Mathf.Atan(value)); }
    public void GetAbs(float value) { getValue?.Invoke(Mathf.Abs(value)); }


    public void Mul(float value) { this.value *= value; }
    public void Div(float value) { this.value /= value; }
    public void Add(float value) { this.value += value; }
    public void Sub(float value) { this.value -= value; }
    public void Mod(float value) { this.value %= value; }

}

