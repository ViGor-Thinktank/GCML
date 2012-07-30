using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field_Schachbrett : Field
    {
     
       public Field_Schachbrett(List<int> lstDimension) : base(new clsSektorKoordinaten(lstDimension[0], lstDimension [1]))
        {
            int width = lstDimension [0];
            int height = lstDimension [1];
            init(width, height);
        }

       public Field_Schachbrett(clsSektorKoordinaten fieldLength)
           : base(fieldLength)
        {
            init(fieldLength.X, fieldLength.Y );
        }

        public Field_Schachbrett(int width, int height): base(new clsSektorKoordinaten(width, height))
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
                    newSek.objSektorKoord = new clsSektorKoordinaten(i, j, 0);
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
        public Sektor get(clsSektorKoordinaten objSektorKoord)
        {
            if(dicSektors.ContainsKey(objSektorKoord.uniqueIDstr()))
                return dicSektors[objSektorKoord.uniqueIDstr()];
            else
                return base.get(objSektorKoord);
        }
      
        public override List<clsSektorKoordinaten> getDirectionVectors()
        {
            List<clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    lisVektors.Add(new clsSektorKoordinaten(i, j, 0));
                }
            }
            return lisVektors;
        }



        public override Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor)
        {
            //clsSektorKoordinaten_Schachbrett objZielSektorKoordSchach = (clsSektorKoordinaten_Schachbrett)aktSek.objSektorKoord + (clsSektorKoordinaten_Schachbrett)Vektor;

            clsSektorKoordinaten objZielSektorKoord = aktSek.objSektorKoord + Vektor;

            if (dicSektors.ContainsKey(objZielSektorKoord.uniqueIDstr()))
                return dicSektors[objZielSektorKoord.uniqueIDstr()];
            else
                return null;
        }

        protected override void setNullSektor()
        {
            this.nullSektorKoord = new clsSektorKoordinaten();
        }
    }
}
