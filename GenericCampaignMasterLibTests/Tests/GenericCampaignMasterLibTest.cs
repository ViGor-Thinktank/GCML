using System;
using NUnit.Framework;
using GenericCampaignMasterLib;


namespace GenericCampaignMasterLibTests
{
	[TestFixture()]
	public class GenericCampaignMasterLibTest
	{
		[Test()]
		public void TestBuildNewTicTacToe ()
		{
			CampaignBuilderTicTacTod builder = new CampaignBuilderTicTacTod();
			CampaignController controller = builder.buildNew();
			controller.addPlayer("Player 1");
			controller.addPlayer("Player 2");

            controller.saveCurrentGameState();




		}
	}
}

