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

        public Field_Schlauch(int p)
        {
            for (int i = 0; i < p; i++)
            {
                Sektor newSek = new Sektor(this.ListSektors.Count.ToString());
                newSek.objSektorKoord = new clsSektorKoordinaten_Schlauch(i);
                newSek.onUnitEnteredSektor += new EventHandler(newSek_onUnitEnteredSektor);
                newSek.onUnitLeftSektor += new EventHandler(newSek_onUnitLeftSektor);

                this.ListSektors.Add(newSek.objSektorKoord.uniqueIDstr(), newSek);
            }
        }

        protected override void setNullSektor()
        {
            this.nullSektor = new clsSektorKoordinaten_Schlauch(1);
        }


        public class clsSektorKoordinaten_Schlauch : clsSektorKoordinaten
        {
            public int X = -1;

            public clsSektorKoordinaten_Schlauch(string newX)
            {
                this.X = Convert.ToInt32(newX);
            }

            public clsSektorKoordinaten_Schlauch(int newX)
            {
                this.X = newX;
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

        public override Sektor get(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ListSektors[objSektorKoord.uniqueIDstr()];
        }

        public override bool checkKoordsValid(Field.clsSektorKoordinaten objSektorKoord)
        {
            return ((clsSektorKoordinaten_Schlauch)objSektorKoord).X < this.ListSektors.Count;
        }

        public override List<Field.clsSektorKoordinaten> getMoveVektors()
        {
            List<Field.clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                lisVektors.Add(new clsSektorKoordinaten_Schlauch(i));
            }
            return lisVektors;
        }

        public override Sektor move(Sektor aktSek, Field.clsSektorKoordinaten Vektor)
        {
            int newID = ((clsSektorKoordinaten_Schlauch)aktSek.objSektorKoord).X + ((clsSektorKoordinaten_Schlauch)Vektor).X;
            foreach (Sektor aktSektor in ListSektors.Values)
            {
                if (aktSektor.Id == newID.ToString())
                    return aktSektor;
            }

            return null;
        }


    }
}
