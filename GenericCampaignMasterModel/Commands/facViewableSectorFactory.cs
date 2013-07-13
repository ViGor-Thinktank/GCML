using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterModel.Commands
{
    
    public class clsViewableSectorFactory : clsSektorFactoryBase
    {
        private Dictionary<string, Sektor> m_ListVisibleSektors = new Dictionary<string, Sektor>();
        public Dictionary<string, Sektor> ListVisibleSektors { get { return m_ListVisibleSektors; } }

        private Sektor m_aktOriginSektor = null;
        private int m_intSichtweite = -1;

        public clsViewableSectorFactory(Field FieldField)
            : base(null, FieldField)
        { 
            
        }

        public clsViewableSectorFactory(Field FieldField, Sektor aktOriginSektor, int intSichtweite)
            : base(null, FieldField)
        {
            m_aktOriginSektor = aktOriginSektor;
            m_intSichtweite = intSichtweite;
        }

        public override void go()
        {
            getVisibleSektorsFromUnitSektor(m_aktOriginSektor, m_intSichtweite);
        }

        public void getVisibleSektorsFromUnitSektor(Sektor aktOriginSektor, int intSichtweite)
        {
            intSichtweite -= 1;

            if (intSichtweite >= 0)
            {
                foreach (clsSektorKoordinaten aktVektor in m_DirectionVektors)
                {
                    Sektor newSek = this.FieldField.move(aktOriginSektor, aktVektor);

                    if (newSek != null)
                    {
                        if (!this.m_ListVisibleSektors.ContainsKey(newSek.strUniqueID))
                        {
                            this.m_ListVisibleSektors.Add(newSek.strUniqueID, newSek);
                        }

                        getVisibleSektorsFromUnitSektor(newSek, intSichtweite);
                    }
                }
            }
            else
            {
                //den eigenen Sektor sieht die Unit immer
                if (this.m_ListVisibleSektors.Count == 0)
                {
                    this.m_ListVisibleSektors.Add(aktOriginSektor.strUniqueID, aktOriginSektor);
                }
            }
        }
    }
}
