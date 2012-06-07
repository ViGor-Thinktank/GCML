using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field_Schachbrett : Field
    {
        private int m_width = -1;
        private int m_height = -1;

        public class clsSektorKoordinaten_Schachbrett : clsSektorKoordinaten
        {
            public int X = -1;
            public int Y = -1;
            private clsSektorKoordinaten_Schachbrett objSektorKoordSchach;

            public clsSektorKoordinaten_Schachbrett(int newX, int newY)
            {
                this.X = newX;
                this.Y = newY;
            }

            public clsSektorKoordinaten_Schachbrett(clsSektorKoordinaten_Schachbrett objSektorKoordSchach)
            {
                this.X = objSektorKoordSchach.X;
                this.Y = objSektorKoordSchach.Y;
            }

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schachbrett otherCast = (clsSektorKoordinaten_Schachbrett)other;
                return (this.X == otherCast.X && this.Y == otherCast.Y);
            }

            public static clsSektorKoordinaten_Schachbrett operator +(clsSektorKoordinaten_Schachbrett c1, clsSektorKoordinaten_Schachbrett c2)
            {
                return new clsSektorKoordinaten_Schachbrett(c1.X + c2.X, c1.Y + c2.Y);
            }

            public override string uniqueIDstr()
            {
                return X.ToString() + "|" + Y.ToString();
            }


        }

        public Field_Schachbrett(int width, int height)
        {
            m_width = width;
            m_height = height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    
                    Sektor newSek;
                    if (i != j) { newSek = new Sektor(this.ListSektors.Count.ToString()); }
                    else { newSek = new Sektor(this.ListSektors.Count.ToString(), new Code.clsSektorType_heavyTerrain() ); }
                    
                    newSek.objSektorKoord = new clsSektorKoordinaten_Schachbrett(i, j);
                    newSek.onUnitEnteredSektor += new EventHandler(newSek_onUnitEnteredSektor);
                    newSek.onUnitLeftSektor += new EventHandler(newSek_onUnitLeftSektor);

                    this.ListSektors.Add(newSek.strUniqueID, newSek);
                }
            }
        }


        public override Sektor get(string strSektorID)
        {
            return this.ListSektors[strSektorID];
        }

        public override Sektor get(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord.uniqueIDstr()];
        }

        public override bool checkKoordsValid(Field.clsSektorKoordinaten objSektorKoord)
        {
            clsSektorKoordinaten_Schachbrett objSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)objSektorKoord;
            return objSektorKoordSchach.X < this.m_height && objSektorKoordSchach.Y < m_width;
        }

        public override List<Field.clsSektorKoordinaten> getMoveVektors()
        {
            List<Field.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    lisVektors.Add(new clsSektorKoordinaten_Schachbrett(i, j));
                }
            }
            return lisVektors;
        }



        public override Sektor move(Sektor aktSek, Field.clsSektorKoordinaten Vektor)
        {
            clsSektorKoordinaten_Schachbrett objZielSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)aktSek.objSektorKoord + (clsSektorKoordinaten_Schachbrett)Vektor;

            if (ListSektors.ContainsKey(objZielSektorKoordSchach.uniqueIDstr()))
                return ListSektors[objZielSektorKoordSchach.uniqueIDstr()];
            else
                return null;
        }

        protected override void setNullSektor()
        {
            this.nullSektorKoord = new clsSektorKoordinaten_Schachbrett(0, 0);
        }
    }
}
