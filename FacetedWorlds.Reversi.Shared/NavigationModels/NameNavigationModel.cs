﻿using UpdateControls;

namespace FacetedWorlds.Reversi.NavigationModels
{
    public class NameNavigationModel
    {
        private string _name;

        #region Independent properties
        // Generated by Update Controls --------------------------------
        private Independent _indName = new Independent();

        public string Name
        {
            get { _indName.OnGet(); return _name; }
            set { _indName.OnSet(); _name = value; }
        }
        // End generated code --------------------------------
        #endregion
    }
}
