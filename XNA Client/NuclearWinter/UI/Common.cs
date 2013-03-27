using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

namespace NuclearWinter.UI
{
    /*
     * Various common enum, values & small classes / structs
     */

    //--------------------------------------------------------------------------
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up
    
    }

    public enum Orientation
    {
        Horizontal,
        Vertical
    }

    //--------------------------------------------------------------------------
    public enum Anchor
    {
        Center,
        Start,
        End,
        Fill
    }

    public static class TextManipulation
    {
        // Used for detecting word boundaries
        public static string WordBoundaries = @",?.;:/\!$(){}[]@=+-*%^`""'~#";
        //public static char[] WordBoundaries = { ',', '?', '.', ';', ':', '/', '\\', '!', '$', '(', ')', '{', '}', '[', ']', '@', '=', '+', '-', '*', '%', '^', '`', '"', '\'', '~', '#' };
    }

    //--------------------------------------------------------------------------
    // SpriteFont decorator with support for custom vertical offset / line spacing
    // and implicit casting
    public class UIFont
    {
        //----------------------------------------------------------------------
        SpriteFont      mSpriteFont;
        public int      YOffset;

        public ReadOnlyCollection<char> Characters                          { get { return mSpriteFont.Characters; } }
        public char?                    DefaultCharacter                    { get { return mSpriteFont.DefaultCharacter; } set { mSpriteFont.DefaultCharacter = value; } }
        public int                      LineSpacing                         { get { return mSpriteFont.LineSpacing; } set { mSpriteFont.LineSpacing = value; } }
        public float                    Spacing                             { get { return mSpriteFont.Spacing; } set { mSpriteFont.Spacing = value; } }

        public Vector2 MeasureString( string _text )
        {
            return mSpriteFont.MeasureString( _text );
        }

        public Vector2 MeasureString( StringBuilder _text )
        {
            return mSpriteFont.MeasureString( _text );
        }

        //----------------------------------------------------------------------
        public static implicit operator SpriteFont( UIFont _instance )
        {
            return _instance.mSpriteFont;
        }

        //----------------------------------------------------------------------
        public UIFont( SpriteFont _font, int _iLineSpacing, int _iYOffset )
        {
            mSpriteFont = _font;
            mSpriteFont.LineSpacing = _iLineSpacing;
            YOffset     = _iYOffset;
        }

        //----------------------------------------------------------------------
        public UIFont( SpriteFont _font )
        : this( _font, _font.LineSpacing, 0 )
        {
        }
    }
}
