using System;
using Impostor.Api.Net;

namespace Kalna.Player
{
    class PlayerData
    {
        public Boolean _isImposter { get; set; }
        public Boolean _isInfected { get; set; }
        private IClientPlayer _controller { get; set; }
        public PlayerData(Boolean isImposter, Boolean isInfected, IClientPlayer controller)
        {
            _isImposter = isImposter;
            _isInfected = isInfected;
            _controller = controller;
        }

        public Boolean matchPlayer(IClientPlayer match)
        {
            if (match.Equals(_controller))
            {
                return true;
            }
            return false;
        }
    }
}
