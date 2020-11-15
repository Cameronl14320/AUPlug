using System;
using System.Threading.Tasks;
using Impostor.Api.Events.Managers;
using Impostor.Api.Plugins;
using Microsoft.Extensions.Logging;
using Kalna.Handlers;
using Kalna;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;

namespace Kalna.Config
{
    class Settings
    {
        public int minDetective { get; set; }

        public override string ToString()
        {
            return "minDetective: " + minDetective;
        }

        
    }

}
