using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericCampaignMasterLib
{
    
    public class clsViewableSectorFactory : clsSektorFactoryBase
    {
        private List<Sektor> m_ListVisibleSektors = new List<Sektor>();
        public List<Sektor> ListVisibleSektors { get { return m_ListVisibleSektors; } }

        public clsViewableSectorFactory(Field FieldField)
            : base(FieldField)
        { 
            
        }


        internal void getVisibleSektorsFromUnitSektor(Sektor aktOriginSektor, int intSichtweite)
        {
            intSichtweite -= 1;

            foreach (Field.clsSektorKoordinaten aktVektor in m_DirectionVektors)
            {
                Sektor newSek = this.FieldField.move(aktOriginSektor, aktVektor);

                if (newSek != null)
                {
                    if (!this.m_ListVisibleSektors.Contains(newSek))
                    {
                        this.m_ListVisibleSektors.Add(newSek);
                    }

                    if (intSichtweite > 0)
                    {
                        getVisibleSektorsFromUnitSektor(newSek, intSichtweite);
                    }
                }
            }
        }
    }
}
