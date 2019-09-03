﻿using UnityEngine;

namespace MarchingCubes.Examples.DensityFunctions
{
    // Taken from http://iquilezles.org/www/articles/distfunctions/distfunctions.htm
   
    [CreateAssetMenu(fileName = "New Round Box Density Function", menuName = "Density Functions/Round Box Density Function")]
    public class RoundBoxDensity : BoxDensity
    {
        [SerializeField] private float rounding;

        public override float CalculateDensity(int x, int y, int z)
        {
            return base.CalculateDensity(x, y, z) - rounding;
        }
    }
}