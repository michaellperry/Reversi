using System.Collections.Generic;
using System;
using System.Linq;

namespace FacetedWorlds.Reversi.Model
{
    public partial class Claim
    {
        public void Approve()
        {
            Community.AddFact(new ClaimResponse(this, 1));
        }

        public void Deny()
        {
            Community.AddFact(new ClaimResponse(this, 0));
        }
    }
}
