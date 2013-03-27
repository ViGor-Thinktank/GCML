using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Xna.Framework;

namespace NuclearWinter
{
    public class Utils
    {
        //----------------------------------------------------------------------
        // There is no Enum.GetValues() on the Xbox 360
        // See http://forums.xna.com/forums/p/1610/157478.aspx
        public static List<T> GetValues<T>()
        {
          Type currentEnum = typeof(T);
          List<T> resultSet = new List<T>();
          if (currentEnum.IsEnum)
          {
            FieldInfo[] fields = currentEnum.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
              resultSet.Add((T)field.GetValue(null));
          }

          return resultSet;
        }

        //----------------------------------------------------------------------
        public static bool SegmentIntersect( ref Vector2 _vA, ref Vector2 _vB, ref Vector2 _vC, ref Vector2 _vD, out Vector2 _vIntersectionPoint )
        {
            float fCoeff1;
            float fCoeff2;
            return SegmentIntersect( ref _vA, ref _vB, ref _vC, ref _vD, out _vIntersectionPoint, out fCoeff1, out fCoeff2 );
        }

        //----------------------------------------------------------------------
        // Based on implementation found in Farseer Physics 2
        public static bool SegmentIntersect( ref Vector2 _vA, ref Vector2 _vB, ref Vector2 _vC, ref Vector2 _vD, out Vector2 _vIntersectionPoint, out float _fCoeff1, out float _fCoeff2 )
        {
            _vIntersectionPoint = new Vector2();
            _fCoeff1 = 0f;
            _fCoeff2 = 0f;

            float fA = _vD.Y - _vC.Y;
            float fB = _vB.X - _vA.X;
            float fC = _vD.X - _vC.X;
            float fD = _vB.Y - _vA.Y;

            // Denominator to solution of linear system
            float fDenom = (fA * fB) - (fC * fD);

            // If denominator is 0, then lines are parallel
            if( ! ( fDenom >= -sfEpsilon && fDenom <= sfEpsilon ) )
            {
                float fE = _vA.Y - _vC.Y;
                float fF = _vA.X - _vC.X;
                float fOneOverDenom = 1f / fDenom;

                // Numerator of first equation
                float fUA = (fC * fE) - (fA * fF);
                fUA *= fOneOverDenom;

                // Check if intersection point of the two lines is on line segment 1
                if( fUA >= 0f && fUA <= 1f )
                {
                    // Numerator of second equation
                    float fUB = (fB * fE) - (fD * fF);
                    fUB *= fOneOverDenom;

                    // Check if intersection point of the two lines is on line segment 2
                    // means the line segments intersect, since we know it is on
                    // segment 1 as well.
                    if( fUB >= 0f && fUB <= 1f )
                    {
                        // Check if they are coincident (no collision in this case)
                        if( fUA != 0f && fUB != 0f )
                        {
                            // There is an intersection
                            _vIntersectionPoint.X = _vA.X + fUA * fB;
                            _vIntersectionPoint.Y = _vA.Y + fUA * fD;

                            _fCoeff1 = fUA;
                            _fCoeff2 = fUB;

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //----------------------------------------------------------------------
        private const float sfEpsilon = .00001f;
    }
}