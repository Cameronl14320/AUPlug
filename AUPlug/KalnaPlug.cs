using System;
using System.Threading.Tasks;
using Impostor.Api.Events.Managers;
using Impostor.Api.Plugins;
using Impostor.Api.Net.Manager;
using Microsoft.Extensions.Logging;
using Kalna.Handlers;
using Kalna.Config;
using System.IO;
using Newtonsoft.Json;

namespace Kalna.Plugin
{
    /// <summary>
    ///     The metadata information of your plugin, this is required.
    /// </summary>
    [ImpostorPlugin(
        package: "gg.impostor.kalna",
        name: "Kalna",
        author: "Kalna",
        version: "0.0.1")]
    public class KalnaPlug : PluginBase // This is also required ": PluginBase".
    {
        /// <summary>
        ///     A logger that works seamlessly with the server.
        /// </summary>
        private readonly ILogger<KalnaPlug> _logger;
        private readonly IEventManager _eventManager;
        private IDisposable _unregister;

        // Settings for Plugin
        private Settings _settings;

        /// <summary>
        ///     The constructor of the plugin. There are a few parameters you can add here and they
        ///     will be injected automatically by the server, two examples are used here.
        public KalnaPlug(ILogger<KalnaPlug> logger, IEventManager eventManager)
        {
            _logger = logger;
            _eventManager = eventManager;

            loadSettings();
        }

        public void loadSettings()
        {
            _settings = new Settings();
            string file = ".\\settings.json";
            string json = File.ReadAllText(file);
            _settings = JsonConvert.DeserializeObject<Settings>(json);
            _logger.LogInformation(_settings.ToString());
        }

        /// <summary>
        ///     This is called when your plugin is enabled by the server.
        /// </summary>
        /// <returns></returns>
        public override ValueTask EnableAsync()
        {
            _logger.LogInformation("Kalna is being enabled.");
            _unregister = _eventManager.RegisterListener(new GameEventListener(_logger));
            return default;
        }


        /// <summary>
        ///     This is called when your plugin is disabled by the server.
        ///     Most likely because it is shutting down, this is the place to clean up any managed resources.
        /// </summary>
        /// <returns></returns>
        public override ValueTask DisableAsync()
        {
            _logger.LogInformation("Kalna is being disabled.");
            _unregister.Dispose();
            return default;
        }
    }
}