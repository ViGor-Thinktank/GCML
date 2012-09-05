using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public class Field : IEquatable<Field>
    {
        public Field()
        {

        }


        public Field(int intX, int intY = 0, int intZ = 0)
        {
            this.m_FieldDimension = new clsSektorKoordinaten(intX, intY, intZ);
            this.initFieldDimension();
        }

        public Field(clsSektorKoordinaten fieldDimension)
        {
            this.m_FieldDimension = fieldDimension;
            this.initFieldDimension();
        }

        private void initFieldDimension()
        {
            for (int x = 0; x <= m_FieldDimension.X; x++)
            {
                for (int y = 0; y <= m_FieldDimension.Y; y++)
                {
                    for (int z = 0; z <= m_FieldDimension.Z; z++)
                    {
                        clsSektorKoordinaten coord = new clsSektorKoordinaten(x, y, z);
                        coord.Position = new List<int> { x, y, z };
                        
                        Sektor newSek = new Sektor(this.dicSektors.Count.ToString());
                        newSek.objSektorKoord = coord;
                        newSek.onUnitEnteredSektor += new Sektor.UnitEnteringHandler(newSek_onUnitEnteredSektor);
                        newSek.onUnitLeftSektor += new Sektor.UnitLeavingHandler(newSek_onUnitLeftSektor);
                        this.dicSektors.Add(newSek.objSektorKoord.uniqueIDstr(), newSek);
                    }
                }
            }
        }

        
        // Ausdehnung des Feldes in 3 Dimensionen. 
        private clsSektorKoordinaten m_FieldDimension;
        public clsSektorKoordinaten FieldDimension 
        {
            get
            {
                return m_FieldDimension;
            }

            set
            {
                m_FieldDimension = value;
            }
        }


        public clsSektorKoordinaten nullSektorKoord = new clsSektorKoordinaten(0,0,0);
        
        public Sektor nullSektor { get { return this.dicSektors[this.nullSektorKoord.uniqueIDstr()]; } }


        public Sektor get(clsSektorKoordinaten objSektorKoord)
        {
            var sektorquery = from s in m_dicSektors.Values
                              where s.objSektorKoord.X == objSektorKoord.X
                              && s.objSektorKoord.Y == objSektorKoord.Y
                              && s.objSektorKoord.Z == objSektorKoord.Z
                              select s;

            if (sektorquery.Count() > 1)
                throw new Exception("mehr als ein Sektor für SektorKoord " + objSektorKoord.ToString());
            else if (sektorquery.Count() == 0)
                throw new Exception("kein Sektor für SektorKoord " + objSektorKoord.ToString());
            else
                return sektorquery.First();

            
        }

        public bool checkKoordsValid(clsSektorKoordinaten objSektorKoord)
        {
            return this.dicSektors.ContainsKey(objSektorKoord.uniqueIDstr());
        }

        private Dictionary<string, Sektor> m_dicSektors = new Dictionary<string, Sektor>();

        public Dictionary<string, Sektor> dicSektors
        {
            get 
            { 
                return m_dicSektors; 
            }
            set
            {
                m_dicSektors = value;
            }
        }
        
        
        public Sektor getSektorForUnit(clsUnit u)
        {
            foreach (Sektor aktSek in dicSektors.Values)
            {
                foreach (clsUnit aktUnit in aktSek.ListUnits)
                {
                    if (aktUnit.Id == u.Id)
                        return aktSek;

                    /*var lokUnit = (from aktUnitinList in aktSek.ListUnits
                                   where aktUnitinList.Id == u.Id
                                   select aktUnitinList);
                    if (lokUnit != null) { return aktSek; }
                */
                }   
            }
            return null;
            /*
            var aktSek = (from s in dicSektors.Values
                         where s.ListUnits.Contains(u)
                         select s).First();

            return aktSek as Sektor;*/
            
        }

        
        public List<Sektor> getSektorList()
        {
            return dicSektors.Values.ToList<Sektor>();
        }


        public void setSektorList(List<Sektor> sektorList)
        {
            m_dicSektors.Clear();
            foreach (Sektor s in sektorList)
            {
                string unid = s.objSektorKoord.uniqueIDstr();
                m_dicSektors.Add(unid, s);
            }
        }

        public int Id { get; set; }
        
        public bool Equals(Field other)
        {
            return (this.Id == other.Id);
        }

        public delegate void delStatus(string strText);
        
        public event delStatus onFieldStatus;


        protected void newSek_onUnitEnteredSektor(object sender, SektorEventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitEnter " + SekSender.strUniqueID);
        }

        protected void newSek_onUnitLeftSektor(object sender, SektorEventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitLeft " + SekSender.strUniqueID);
        }

        public List<clsSektorKoordinaten> getDirectionVectors()
        {
            List<clsSektorKoordinaten> lisVektors = new List<clsSektorKoordinaten>();
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        lisVektors.Add(new clsSektorKoordinaten(i, j, k));
                    }
                }
            }
            return lisVektors;
        }

        public Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor)
        {
            clsSektorKoordinaten objZielSektorKoord = aktSek.objSektorKoord + Vektor;

            if (dicSektors.ContainsKey(objZielSektorKoord.uniqueIDstr()))
                return dicSektors[objZielSektorKoord.uniqueIDstr()];
            else
                return null;
        }

    }
    
 
}
