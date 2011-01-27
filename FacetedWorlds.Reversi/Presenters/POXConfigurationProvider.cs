using System;
using UpdateControls.Correspondence.POXClient;

namespace FacetedWorlds.Reversi.Presenters
{
    public class POXConfigurationProvider : IPOXConfigurationProvider
    {
        public POXConfiguration Configuration
        {
            get
            {
                return new POXConfiguration("http://fwprod01/correspondence_server_web/pox", "FacetedWorlds.Reversi");
            }
        }
    }
}
