using Impostor.Api.Events;
using Impostor.Api.Events.Player;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections;
using Kalna.Player;
using Kalna.Plugin;
using Impostor.Api.Net.Inner.Objects.Components;
using Impostor.Api.Net;
using System.Collections.Generic;
using System;

namespace Kalna.Handlers
{
    /// <summary>
    ///     A class that listens for two events.
    ///     It may be more but this is just an example.
    ///
    ///     Make sure your class implements <see cref="IEventListener"/>.
    /// </summary>
    public class GameEventListener : IEventListener
    {
        private readonly ILogger<KalnaPlug> _logger;
        private List<PlayerData> gameInstancePlayers;

        public GameEventListener(ILogger<KalnaPlug> logger)
        {
            _logger = logger;
        }

        [EventListener]
        public async ValueTask OnGameCreated(IGameCreatedEvent e)
        {
            var options = e.Game.Options;
            options.MaxPlayers = 30;
            options.NumImpostors = 9;
            await e.Game.SyncSettingsAsync();
        }


        [EventListener]
        public void OnGameStarted(IGameStartedEvent e)
        {
            _logger.LogInformation($"Game is starting.");

            gameInstancePlayers = new List<PlayerData>();
            
            int players = e.Game.PlayerCount;
            var host = e.Game.Host.Character;

            // Store each player in the plugin PlayerData class
            foreach (var player in e.Game.Players)
            {
                var playerC = player.Character;
                var info = player.Character.PlayerInfo;
                var isImpostor = info.IsImpostor;

                gameInstancePlayers.Add(new PlayerData(isImpostor, false, player));

            }

        }

        [EventListener]
        public void OnGameEnded(IGameEndedEvent e)
        {
            _logger.LogInformation($"Game has ended.");
        }

        [EventListener]
        public void OnPlayerChat(IPlayerChatEvent e)
        {
            _logger.LogInformation($"{e.PlayerControl.PlayerInfo.PlayerName} said {e.Message}");

            var host = e.Game.Host.Character;
            foreach (var player in e.Game.Players)
            {
                var playerC = player.Character;
                var info = player.Character.PlayerInfo;
                var isImpostor = info.IsImpostor;
                if (isImpostor)
                {
                    _logger.LogInformation($"- {info.PlayerName} is an impostor.");
                }
                else
                {
                    _logger.LogInformation($"- {info.PlayerName} is a crewmate.");
                }

            }
        }

        [EventListener]
        public void OnPlayerMurder(IPlayerMurderEvent e)
        {
            _logger.LogInformation(e.Victim.PlayerInfo.PlayerName + " was murdered");
        }

        [EventListener]
        public void OnPlayerExile(IPlayerExileEvent e)
        {
            var exiled = e.PlayerControl.PlayerInfo;
            e.PlayerControl.SetMurderedAsync();
        }

        [EventListener]
        public void OnPlayerEvent(IPlayerEvent e)
        {

        }

        public Boolean CheckCollision(IClientPlayer a, IClientPlayer b)
        {
            var characterA = a.Character;
            var characterB = b.Character;
            if (a.Equals(b))
            {
                return false;
            }

            if (characterA.NetworkTransform.Equals(characterB.NetworkTransform))
            {
                PlayerData playerA = null;
                PlayerData playerB = null;

                foreach(PlayerData p in gameInstancePlayers) {
                    if (p.matchPlayer(a))
                    {
                        playerA = p;
                    } else if (p.matchPlayer(b))
                    {
                        playerB = p;
                    }
                }

                if ((playerA == null || playerB == null) || (playerA.Equals(playerB)))
                {
                    return false;
                }

                return true;
            }
            return false;
        }
            
    }
}