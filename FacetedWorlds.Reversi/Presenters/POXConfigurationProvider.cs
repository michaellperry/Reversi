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
                return new POXConfiguration("http://184.106.132.210/correspondence_server_web/pox", "FacetedWorlds.Reversi");
            }
        }
    }
}
