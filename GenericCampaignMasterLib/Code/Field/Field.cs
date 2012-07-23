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
            setNullSektor();
        }

        public Field(List<int> lstDimension)
        {
            setNullSektor();
            m_lstDimensions = lstDimension;
        }

        private List<int> m_lstDimensions;
        public List<int> ListDimensions { get { return m_lstDimensions;} }

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
