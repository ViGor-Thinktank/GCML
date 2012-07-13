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
        public Sektor nullSektor { get { return this.ListSektors[this.nullSektorKoord.uniqueIDstr()]; } }
        

        public abstract Sektor get(string strSektorID);
        public abstract Sektor get(clsSektorKoordinaten objSektorKoord);

        public bool checkKoordsValid(clsSektorKoordinaten objSektorKoord)
        {
            return this.ListSektors.ContainsKey(objSektorKoord.uniqueIDstr());

        }

        private Dictionary<string, Sektor> m_ListSektors = new Dictionary<string, Sektor>();

        public Dictionary<string, Sektor> ListSektors
        {
            get 
            { 
                return m_ListSektors; 
            }
            set
            {
                m_ListSektors = value;
            }
        }
        
        
        public Sektor getSektorForUnit(IUnit u)
        {
            var aktSek = (from s in ListSektors.Values
                         where s.ListUnits.Contains(u)
                         select s).First();

            return aktSek as Sektor;
            
        }

        
        public List<Sektor> getSektorList()
        {
            return ListSektors.Values.ToList<Sektor>();
        }


        public void setSektorList(List<Sektor> sektorList)
        {
            m_ListSektors.Clear();
            foreach (Sektor s in sektorList)
            {
                string unid = s.objSektorKoord.uniqueIDstr();
                m_ListSektors.Add(unid, s);
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
