using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field_Schachbrett : Field
    {
       public class clsSektorKoordinaten_Schachbrett : clsSektorKoordinaten
        {
            public int X = -1;
            public int Y = -1;
            
            public clsSektorKoordinaten_Schachbrett(int newX, int newY)
            {
                this.X = newX;
                this.Y = newY;
                this.Position = new List<int>() { newX, newY };
            }

            public clsSektorKoordinaten_Schachbrett(clsSektorKoordinaten_Schachbrett objSektorKoordSchach)
            {
                this.X = objSektorKoordSchach.X;
                this.Y = objSektorKoordSchach.Y;
                this.Position = new List<int>() { objSektorKoordSchach.X, objSektorKoordSchach.Y };
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

        public Field_Schachbrett(List<int> lstDimension) : base(lstDimension)
        {
            int width = lstDimension [0];
            int height = lstDimension [1];
            init(width, height);
            
        }

        public Field_Schachbrett(int width, int height): base(new List<int> (){ width, height})
        {
            init(width, height);
        }

        private void init(int width, int height)
        {
            setNullSektor();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    
                    /*Sektor newSek;
                    if (i != j) { newSek = new Sektor(this.dicSektors.Count.ToString()); }
                    else { newSek = new Sektor(this.dicSektors.Count.ToString(), new clsSektorType_heavyTerrain() ); }
                    */

                    Sektor newSek = new Sektor(this.dicSektors.Count.ToString()); 
                    newSek.objSektorKoord = new clsSektorKoordinaten_Schachbrett(i, j);
                    newSek.onUnitEnteredSektor += new Sektor.UnitEnteringHandler(newSek_onUnitEnteredSektor);
                    newSek.onUnitLeftSektor += new Sektor.UnitLeavingHandler(newSek_onUnitLeftSektor);

                    this.dicSektors.Add(newSek.strUniqueID, newSek);
                }
            }
        }
        public override Sektor get(string strSektorID)
        {
            return this.dicSektors[strSektorID];
        }
        public override Sektor get(clsSektorKoordinaten objSektorKoord)
        {
            return dicSektors[objSektorKoord.uniqueIDstr()];
        }
      
        public override List<clsSektorKoordinaten> getDirectionVectors()
        {
            List<clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    lisVektors.Add(new clsSektorKoordinaten_Schachbrett(i, j));
                }
            }
            return lisVektors;
        }



        public override Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor)
        {
            clsSektorKoordinaten_Schachbrett objZielSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)aktSek.objSektorKoord + (clsSektorKoordinaten_Schachbrett)Vektor;

            if (dicSektors.ContainsKey(objZielSektorKoordSchach.uniqueIDstr()))
                return dicSektors[objZielSektorKoordSchach.uniqueIDstr()];
            else
                return null;
        }

        protected override void setNullSektor()
        {
            this.nullSektorKoord = new clsSektorKoordinaten_Schachbrett(0, 0);
        }
    }
}
