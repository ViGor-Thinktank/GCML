using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace NuclearWinter.UI
{
    //--------------------------------------------------------------------------
    public class ProgressBar: Widget
    {
        //----------------------------------------------------------------------
        public int Value
        {
            get { return miValue; }
            set { miValue = value; mfLerpValue = miValue; }
        }

        public int Max;

        //----------------------------------------------------------------------
        int         miValue;
        float       mfLerpValue;

        //----------------------------------------------------------------------
        public void SetProgress( int _iValue )
        {
            miValue = _iValue;
        }

        //----------------------------------------------------------------------
        public ProgressBar( Screen _screen, int _iValue=0, int _iMax=100 )
        : base( _screen )
        {
            Value   = _iValue;
            Max     = _iMax;
        }

        //----------------------------------------------------------------------
        public override void Update( float _fElapsedTime )
        {
            float fLerpAmount = Math.Min( 1f, _fElapsedTime * NuclearGame.LerpMultiplier );

            mfLerpValue = MathHelper.Lerp( mfLerpValue, Value, fLerpAmount );
        }

        //----------------------------------------------------------------------
        public override void Draw()
        {
            Screen.DrawBox( Screen.Style.ProgressBarFrame, LayoutRect, Screen.Style.ProgressBarFrameCornerSize, Color.White );

            if( Value > 0 )
            {
                Rectangle progressRect = new Rectangle( LayoutRect.X, LayoutRect.Y, (int)( LayoutRect.Width * mfLerpValue / Max ), LayoutRect.Height );
                Screen.DrawBox( Screen.Style.ProgressBar, progressRect, Screen.Style.ProgressBarCornerSize, Color.White );
            }
        }
    }
}
