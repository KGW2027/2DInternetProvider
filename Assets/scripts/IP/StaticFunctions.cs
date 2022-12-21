using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IP
{
    
    public static class StaticFunctions
    {
        public struct Bytes
        {
            private double InternalValue { get; set; }

            public static readonly double GB = 1L << 0;
            public static readonly double TB = 1L << 10;
            public static readonly double PB = 1L << 20;
            public static readonly double EB = 1L << 30;
            public static readonly double ZB = 1L << 40;
            public static readonly double YB = 1L << 50;
            public static readonly double RB = 1L << 60;

            public static double operator *(Bytes b, double n)
            {
                return b.InternalValue * n;
            }

            public static implicit operator Bytes(double value)
            {
                return new Bytes {InternalValue = value};
            }

            public static string ToByteString(double data)
            {
                if (Math.Floor(data / RB) > 0) return $"{data / RB:F} RB";
                if (Math.Floor(data / YB) > 0) return $"{data / YB:F} YB";
                if (Math.Floor(data / ZB) > 0) return $"{data / ZB:F} ZB";
                if (Math.Floor(data / EB) > 0) return $"{data / EB:F} EB";
                if (Math.Floor(data / PB) > 0) return $"{data / PB:F} PB";
                if (Math.Floor(data / TB) > 0) return $"{data / TB:F} TB";
                return $"{data / GB:F} GB";
            }
        }
    
        public static Bounds OrthographicBounds(this Camera camera, float aspect)
        {
            float cameraHeight = camera.orthographicSize * 2;
            Bounds bounds = new Bounds(
                camera.transform.position,
                new Vector3(cameraHeight * aspect, cameraHeight, 0));
            return bounds;
        }
    }
}