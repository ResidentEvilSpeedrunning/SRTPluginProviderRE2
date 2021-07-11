using SRTPluginProviderRE2.Structs;
using SRTPluginProviderRE2.Structs.GameStructs;
using System;

namespace SRTPluginProviderRE2
{
    public interface IGameMemoryRE2
    {
        string GameName { get; }

        string VersionInfo { get; }

        GameTimer Timer { get; }

        CharacterEnumeration PlayerCharacter { get; }

        GamePlayer Player { get; }

        string PlayerName { get; }

        bool IsPoisoned { get; }

        GameRankManager RankManager { get; }

        GameInventoryEntry[] PlayerInventory { get; }

        EnemyHP[] EnemyHealth { get; }

        long IGTCalculated { get; }

        long IGTCalculatedTicks { get; }

        TimeSpan IGTTimeSpan { get; }

        string IGTFormattedString { get; }
    }
}
