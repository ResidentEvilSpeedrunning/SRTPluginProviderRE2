﻿using SRTPluginBase.Interfaces;
using System;

namespace SRTPluginProviderRE2
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Resident Evil 2 (2019))";

        public string Description => "A game memory provider plugin for Resident Evil 2 (2019).";

        public string Author => "Squirrelies, CursedToast, VideoGameRoulette, DeathHound6";

        public Uri MoreInfoURL => new Uri("https://github.com/SpeedrunTooling/SRTPluginProducerRE2");

        public int VersionMajor => assemblyVersion?.Major ?? 0;

        public int VersionMinor => assemblyVersion?.Minor ?? 0;

        public int VersionBuild => assemblyVersion?.Build ?? 0;

        public int VersionRevision => assemblyVersion?.Revision ?? 0;

        public Version Version => assemblyVersion ?? new();

        private readonly Version? assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        public bool Equals(IPluginInfo? other) => Equals(this, other);
    }
}
