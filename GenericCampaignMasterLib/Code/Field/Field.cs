using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class Field : IEquatable<Field>
    {
        public Field()
        {
        }

        public Field(clsSektorKoordinaten fieldDimension)
        {
            this.m_FieldDimension = fieldDimension;
        }

        public void initFieldDimension()
        {
            setNullSektor();
             
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

        private List<int> m_lstDimensions;
        public List<int> ListDimensions { get { return m_lstDimensions;} }

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


        public clsSektorKoordinaten nullSektorKoord = null;
        protected abstract void setNullSektor();
        public Sektor nullSektor { get { return this.dicSektors[this.nullSektorKoord.uniqueIDstr()]; } }
        

        public abstract Sektor get(string strSektorID);
        public abstract Sektor get(clsSektorKoordinaten objSektorKoord);

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
        
        
        public Sektor getSektorForUnit(BaseUnit u)
        {
            foreach (Sektor aktSek in dicSektors.Values)
            {


                foreach (BaseUnit aktUnit in aktSek.ListUnits)
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

        public abstract Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor);
        public abstract List<clsSektorKoordinaten> getDirectionVectors();

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


        
    }
    
 
}
