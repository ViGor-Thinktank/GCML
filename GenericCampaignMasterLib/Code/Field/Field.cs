using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    public abstract class Field : IEquatable<Field>
    {
        public abstract class clsSektorKoordinaten
        {
           public abstract bool Equals(clsSektorKoordinaten other);

           public abstract string uniqueIDstr();
            
        }

        public Field()
        {
            setNullSektor();
        }

        public Field(List<int> lstDimension)
        {
            m_lstDimensions = lstDimension;
        }

        private List<int> m_lstDimensions;
        public List<int> ListDimensions { get { return m_lstDimensions;} }

        public clsSektorKoordinaten nullSektorKoord = null;
        protected abstract void setNullSektor();
        public Sektor nullSektor { get { return this.ListSektors[this.nullSektorKoord.uniqueIDstr()]; } }
        

        public abstract Sektor get(string strSektorID);
        public abstract Sektor get(clsSektorKoordinaten objSektorKoord);

        public bool checkKoordsValid(Field.clsSektorKoordinaten objSektorKoord)
        {
            return this.ListSektors.ContainsKey(objSektorKoord.uniqueIDstr());

        }

        private Dictionary<string, Sektor> m_ListSektors = new Dictionary<string, Sektor>();

        public Dictionary<string, Sektor> ListSektors
        {
            get { return m_ListSektors; }
        }
        
        
        public Sektor getSektorForUnit(IUnit u)
        {
            // Erstmal so gelöst. Geht besser.      
            foreach (Sektor s in ListSektors.Values)
            {
                if (s.ListUnits.Contains(u))
                    return s;
            }

            return null;
        }

        
        public List<Sektor> getSektorList()
        {
            return ListSektors.Values.ToList<Sektor>();
        }

        public abstract Sektor move(Sektor aktSek, clsSektorKoordinaten Vektor);
        public abstract List<Field.clsSektorKoordinaten> getDirectionVectors();

        public int Id { get; set; }
        
        public bool Equals(Field other)
        {
            return (this.Id == other.Id);
        }

        public delegate void delStatus(string strText);
        
        public event delStatus onFieldStatus;


        protected  void newSek_onUnitEnteredSektor(object sender, EventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitEnter " + SekSender.strUniqueID);
        }

        protected void newSek_onUnitLeftSektor(object sender, EventArgs e)
        {
            Sektor SekSender = (Sektor)sender;
            if (onFieldStatus != null)
                onFieldStatus("UnitLeft " + SekSender.strUniqueID);
        }


        
    }
    
 
}
