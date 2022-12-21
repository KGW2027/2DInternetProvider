using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IP
{
    
    public static class StaticFunctions
    {
        public struct Bytes
        {
            private long InternalValue { get; set; }

            public static readonly ulong MB = 1L << 0;
            public static readonly ulong GB = 1L << 10;
            public static readonly ulong TB = 1L << 20;
            public static readonly ulong PB = 1L << 30;
            public static readonly ulong EB = 1L << 40;
            public static readonly ulong ZB = 1L << 50;
            public static readonly ulong YB = 1L << 60;

            public static long operator *(Bytes b, long n)
            {
                return b.InternalValue * n;
            }

            public static implicit operator Bytes(long value)
            {
                return new Bytes {InternalValue = value};
            }

            public static string ToByteString(ulong data)
            {
                if (data / YB > 0) return $"{(double) data / YB:F} YB";
                if (data / ZB > 0) return $"{(double) data / ZB:F} ZB";
                if (data / EB > 0) return $"{(double) data / EB:F} EB";
                if (data / PB > 0) return $"{(double) data / PB:F} PB";
                if (data / TB > 0) return $"{(double) data / TB:F} TB";
                if (data / GB > 0) return $"{(double) data / GB:F} GB";
                return $"{(double) data / MB:F} MB";
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