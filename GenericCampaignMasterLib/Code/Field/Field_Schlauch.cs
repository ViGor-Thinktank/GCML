using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field_Schlauch : Field
    {

        public Field_Schlauch(List<Sektor> lisNewSektors)
        {
            foreach (Sektor sekNew in lisNewSektors)
            {
                sekNew.objSektorKoord = new clsSektorKoordinaten_Schlauch(sekNew.Id);
                this.ListSektors.Add(sekNew.objSektorKoord.uniqueIDstr(), sekNew);
            }
        }

        public Field_Schlauch(List<int> lstDimension) : base(lstDimension)
        {
            int p = lstDimension [0];
            init(p);
        }

        public Field_Schlauch(int p) : base(new List<int> (){ p })
        {
            init(p);
        }

        private void init(int p)
        {
            for (int i = 0; i < p; i++)
            {
                Sektor newSek = new Sektor(this.ListSektors.Count.ToString());
                newSek.objSektorKoord = new clsSektorKoordinaten_Schlauch(i);
                newSek.onUnitEnteredSektor += new Sektor.UnitEnteringHandler(newSek_onUnitEnteredSektor);
                newSek.onUnitLeftSektor += new Sektor.UnitLeavingHandler(newSek_onUnitLeftSektor);

                this.ListSektors.Add(newSek.objSektorKoord.uniqueIDstr(), newSek);
            }
        }

        protected override void setNullSektor()
        {
            this.nullSektorKoord = new clsSektorKoordinaten_Schlauch(0);
        }


        public class clsSektorKoordinaten_Schlauch : clsSektorKoordinaten
        {
            public int X = -1;

            public clsSektorKoordinaten_Schlauch(string newX)
            {
                this.X = Convert.ToInt32(newX);
                this.Position = new List<int>() { this.X };
            }

            public clsSektorKoordinaten_Schlauch(int newX)
            {
                this.X = newX;
                this.Position = new List<int>() { this.X };
            }

            public override bool Equals(clsSektorKoordinaten other)
            {
                clsSektorKoordinaten_Schlauch otherCast = (clsSektorKoordinaten_Schlauch)other;
                return (this.X == otherCast.X);
            }

            public override string uniqueIDstr()
            {
                return this.X.ToString();
            }
        }

        public override Sektor get(string strSektorID)
        {
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == strSektorID)
                    return aktSektor;
            }

            return null;
        }

        public override Sektor get(clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord.uniqueIDstr()];
        }

        
        public override List<clsSektorKoordinaten> getDirectionVectors()
        {
            List<clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                lisVektors.Add(new clsSektorKoordinaten_Schlauch(i));
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor)
        {
            int newID = ((clsSektorKoordinaten_Schlauch)aktSek.objSektorKoord).X + ((clsSektorKoordinaten_Schlauch)Vektor).X;
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == newID.ToString())
                    return aktSektor;
            }

            return nullSektor;
        }


    }
}
