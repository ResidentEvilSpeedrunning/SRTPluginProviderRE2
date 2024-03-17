using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SRTPluginBase.Interfaces;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SRTPluginBase;
using System.Text.Json;

namespace SRTPluginProviderRE2
{
    public partial class SRTPluginProducerRE2 : PluginBase<SRTPluginProducerRE2>, IPluginProducer
    {
        private readonly ILogger<SRTPluginProducerRE2> logger;
        private readonly IPluginHost pluginHost;

        // Properties
        public override IPluginInfo Info => new PluginInfo();

        public override ILogger Logger => logger;

        // Fields
        private GameMemoryRE2Scanner? gameMemoryScanner;

        public SRTPluginProducerRE2(ILogger<SRTPluginProducerRE2> logger, IPluginHost pluginHost)
        {
            this.logger = logger;
            this.pluginHost = pluginHost;

            // Register pages.
            registeredPages.Add("Config", (Controller controller) =>
            {
                if (controller.Request.Query.ContainsKey("Config"))
                {
                    Configuration = JsonSerializer.Deserialize<PluginConfiguration>(controller.Request.Query["Config"]!);
                    SaveConfiguration((Configuration as PluginConfiguration)!.ModelToConfigDictionary());
                }
                return Task.FromResult<IActionResult>(controller.View("Config", Configuration as PluginConfiguration));
            });

            Refresh(); // Attempt to initialize and perform first read.
        }

        public object? Refresh()
        {
            if (gameMemoryScanner is null || !gameMemoryScanner.ProcessRunning)
            {
                Process? gameProc = Process.GetProcessesByName("re2")?.FirstOrDefault();
                uint pid = (uint)(gameProc?.Id ?? 0);
                if (pid != 0)
                    gameMemoryScanner = new GameMemoryRE2Scanner(gameProc);
            }

            if (gameMemoryScanner is not null && gameMemoryScanner.ProcessRunning)
                return gameMemoryScanner.Refresh();

            return default;
        }

        public override void Dispose()
        {
            gameMemoryScanner?.Dispose();
            gameMemoryScanner = default;
            if (Configuration is not null)
                SaveConfiguration((Configuration as PluginConfiguration)!.ModelToConfigDictionary());

        }
        public override async ValueTask DisposeAsync()
        {
            Dispose();
            await Task.CompletedTask;
        }

        public bool Equals(IPluginProducer? other) => Equals(this, other);

        //private Process process;
        //private GameMemoryRE2Scanner gameMemoryScanner;
        //private Stopwatch stopwatch;
        //public IPluginInfo Info => new PluginInfo();
        //public bool GameRunning
        //{
        //    get
        //    {
        //        if (gameMemoryScanner != null && !gameMemoryScanner.ProcessRunning)
        //        {
        //            process = GetProcess();
        //            if (process != null)
        //                gameMemoryScanner.Initialize(process); // Re-initialize and attempt to continue.
        //        }

        //        return gameMemoryScanner != null && gameMemoryScanner.ProcessRunning;
        //    }
        //}

        //public int Startup(IPluginHostDelegates hostDelegates)
        //{
        //    this.hostDelegates = hostDelegates;
        //    process = GetProcess();
        //    gameMemoryScanner = new GameMemoryRE2Scanner(process);
        //    stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    return 0;
        //}

        //public int Shutdown()
        //{
        //    gameMemoryScanner?.Dispose();
        //    gameMemoryScanner = null;
        //    stopwatch?.Stop();
        //    stopwatch = null;
        //    return 0;
        //}

        //public object PullData()
        //{
        //    try
        //    {
        //        if (!GameRunning) // Not running? Bail out!
        //            return null;

        //        if (stopwatch.ElapsedMilliseconds >= 2000L)
        //        {
        //            gameMemoryScanner.UpdatePointers();
        //            stopwatch.Restart();
        //        }
        //        return gameMemoryScanner.Refresh();
        //    }
        //    catch (Win32Exception ex)
        //    {
        //        if ((ProcessMemory.Win32Error)ex.NativeErrorCode != ProcessMemory.Win32Error.ERROR_PARTIAL_COPY)
        //            hostDelegates.ExceptionMessage(ex);// Only show the error if its not ERROR_PARTIAL_COPY. ERROR_PARTIAL_COPY is typically an issue with reading as the program exits or reading right as the pointers are changing (i.e. switching back to main menu).

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        hostDelegates.ExceptionMessage(ex);
        //        return null;
        //    }
        //}

        //private Process GetProcess() => Process.GetProcessesByName("re2")?.FirstOrDefault();
    }
}
