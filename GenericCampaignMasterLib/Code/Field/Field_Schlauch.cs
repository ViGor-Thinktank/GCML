using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field_Schlauch : Field
    {
        public Field_Schlauch(int length) : base(new clsSektorKoordinaten(length))
        {
            base.initFieldDimension();
        }

        public Field_Schlauch(clsSektorKoordinaten fieldLength) : base(fieldLength)
        {
            base.initFieldDimension();
        }

        protected override void setNullSektor()
        {
            this.nullSektorKoord = new clsSektorKoordinaten(0);
        }

        //public class clsSektorKoordinaten_Schlauch : clsSektorKoordinaten
        //{
        //    public int X = -1;

        //    public clsSektorKoordinaten_Schlauch(string newX)
        //    {
        //        this.X = Convert.ToInt32(newX);
        //        this.Position = new List<int>() { this.X };
        //    }

        //    public clsSektorKoordinaten_Schlauch(int newX)
        //    {
        //        this.X = newX;
        //        this.Position = new List<int>() { this.X };
        //    }

        //    public override bool Equals(clsSektorKoordinaten other)
        //    {
        //        clsSektorKoordinaten_Schlauch otherCast = (clsSektorKoordinaten_Schlauch)other;
        //        return (this.X == otherCast.X);
        //    }

        //    public override string uniqueIDstr()
        //    {
        //        return this.X.ToString();
        //    }
        //}

        public override Sektor get(string strSektorID)
        {
            foreach (Sektor aktSektor in dicSektors.Values)
            {
                if (aktSektor.Id == strSektorID)
                    return aktSektor;
            }

            return null;
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
                lisVektors.Add(new clsSektorKoordinaten(i));
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor)
        {
            int newID = aktSek.objSektorKoord.X + Vektor.X;
            foreach (Sektor aktSektor in dicSektors.Values)
            {
                if (aktSektor.Id == newID.ToString())
                    return aktSektor;
            }

            return nullSektor;
        }


    }
}
