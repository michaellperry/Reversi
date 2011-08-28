using System;
using System.Collections.Generic;
using System.Linq;
using UpdateControls.Correspondence.POXClient;

namespace FacetedWorlds.Reversi.IntegrationTest
{
    public class POXConfigurationProvider : IPOXConfigurationProvider
    {
        public POXConfiguration Configuration
        {
            get
            {
                //return new POXConfiguration("http://facetedworlds.com/correspondence_server_web/pox", "FacetedWorlds.Reversi", "facetedworlds_private");
                return new POXConfiguration("http://localhost:8080/correspondence_server_web/pox", "FacetedWorlds.Reversi", "facetedworlds_private");
            }
        }

        public bool IsToastEnabled
        {
            get { return false; }
        }
    }
}
