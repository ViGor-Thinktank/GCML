using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using GenericCampaignMasterLib;

namespace GenericCampaignMasterLib
{
    public class CampaignController
    {
        private CampaignEngine m_campaignEngine;        
        private List<Sektor> unitCollisionStack = new List<Sektor>();
        
        public event Field.delStatus onStatus;
        
        public void Global_onStatus(string strText)
        {
            if (onStatus != null)
                onStatus(strText);
        }

        public CampaignController()
        {


        }

        public CampaignController(CampaignEngine engine)
        {
            init(engine);
        }

        private void init(CampaignEngine engine)
        {
            m_campaignEngine = engine;
            foreach (Sektor sektor in engine.FieldField.getSektorList())
            {
                sektor.onUnitEnteredSektor += onUnitMove;
                sektor.onUnitLeftSektor += onUnitMove;
            }
        }

        public void loadGameState(Guid gameid, Dictionary <Guid, CampaignState> gameStateCollection)
        {
            CampaignState loadedState = gameStateCollection[gameid];
            CampaignEngine loadedEngine = loadedState.Restore();

            init(loadedEngine);
        }

        public void loadLastGameState()
        {


        }


        public void saveGameState()
        {
            CampaignState state = m_campaignEngine.getState();
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string strState = serializer.Serialize(state);
        }


        public List<Sektor> getUnitCollisions()
        {
            return unitCollisionStack;
        }
       
        private void onUnitMove(object sender, EventArgs args)
        {
            Sektor sektor = sender as Sektor;
            if (checkSektorForUnitCollision(sektor))
            {
                unitCollisionStack_Add(sektor);
            }
            else if (unitCollisionStack.Contains(sektor))
            {
                unitCollisionStack.Remove(sektor);
            }
        }

        private void unitCollisionStack_Add(Sektor sektor)
        {
            Global_onStatus("Collision: " + sektor.strUniqueID);
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



        public Player addPlayer(string p)
        {
            return this.m_campaignEngine.addPlayer(p);
        }

        public void createNewUnit(int intPlayerID, Type type)
        {
            this.m_campaignEngine.addUnit(intPlayerID, type);
        }

        public List<ICommand> getCommandsForUnit(IUnit unit)
        {
            return this.m_campaignEngine.getCommandsForUnit(unit);
        }
    }

}
