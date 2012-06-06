using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericCampaignMasterLib.Code;
using GenericCampaignMasterLib.Code.Unit;

namespace GenericCampaignMasterLib
{
    public class CampaignController
    {
        private CampaignEngine m_campaignEngine;
        private List<Sektor> unitCollisionStack = new List<Sektor>();
        
        public CampaignController(CampaignEngine engine)
        {
            m_campaignEngine = engine;
            foreach (Sektor sektor in engine.FieldField.getSektorList())
            {
                sektor.onUnitEnteredSektor += onUnitMove;
                sektor.onUnitLeftSektor += onUnitMove;
            }
        }

        public List<Sektor> getUnitCollisions()
        {
            return unitCollisionStack;
        }


        public void resolveUnitCollision(Sektor sektor)
        {
            if (!checkSektorForUnitCollision(sektor))
                unitCollisionStack.Remove(sektor);
        }
        
        private void onUnitMove(object sender, EventArgs args)
        {
            Sektor sektor = sender as Sektor;
            if (checkSektorForUnitCollision(sektor))
                unitCollisionStack.Add(sektor);
        }

        private bool checkSektorForUnitCollision(Sektor sektor)
        {
            bool resultCollision = false;
            if (sektor.ListUnits.Count > 1)
            {
                List<Player> unitOwnersInSektor = new List<Player>();
                foreach (IUnit unit in sektor.ListUnits)
                {
                    Player owner = m_campaignEngine.getUnitOwner(unit);
                    if (!unitOwnersInSektor.Contains(owner))
                        unitOwnersInSektor.Add(owner);
                }

                if (unitOwnersInSektor.Count() > 1)
                    resultCollision = true;
            }
            else
            {
                ;
            }

            return resultCollision;
        }


    }

}
