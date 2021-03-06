﻿using System;

namespace Microsoft.DwayneNeed.Geometry
{
    /// <summary>
    ///     A generic interface for a 3-dimensional size.
    /// </summary>
    public interface ISize3D<T>
    {
        T Width { get; }
        T Height { get; }
        T Depth { get; }
    }

    /// <summary>
    ///     A simple implementation of a 3-dimensional size.
    /// </summary>
    public struct Size3D<T> : ISize3D<T>
    {
        public Size3D(T width, T height, T depth) : this()
        {
            T nZero = default;
            dynamic nWidth = width;
            dynamic nHeight = height;
            dynamic nDepth = depth;
            if (nWidth < nZero || nHeight < nZero || nDepth < nZero)
                throw new InvalidOperationException("Size extents may not be negative.");

            Width = width;
            Height = height;
            Depth = depth;
        }

        public T Width { get; }
        public T Height { get; }
        public T Depth { get; }
    }
}