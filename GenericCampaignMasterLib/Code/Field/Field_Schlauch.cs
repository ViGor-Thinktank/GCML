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

        public override Sektor get(string strSektorID)
        {
            foreach (Sektor aktSektor in dicSektors.Values)
            {
                if (aktSektor.Id == strSektorID)
                    return aktSektor;
            }

            return null;
        }

        public Sektor get(clsSektorKoordinaten objSektorKoord)
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
