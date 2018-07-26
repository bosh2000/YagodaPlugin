using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using Resto.Front.Api.V6;
using Resto.Front.Api.V6.Attributes;
using Resto.Front.Api.V6.Attributes.JetBrains;
using Resto.Front.Api.YagodaPlugin;
using NLog;

namespace Resto.Front.Api.YagodaPlug
{
    /// <summary>
    /// Тестовый плагин для демонстрации возможностей Api.
    /// Автоматически не публикуется, для использования скопировать Resto.Front.Api.SamplePlugin.dll в Resto.Front.Main\bin\Debug\Plugins\Resto.Front.Api.SamplePlugin\
    /// </summary>
    [UsedImplicitly]
    [PluginLicenseModuleId(21005108)]
    public sealed class YagodaPlug : IFrontPlugin
    {
        private readonly Stack<IDisposable> subscriptions = new Stack<IDisposable>();
        private static ILog logger;

        public YagodaPlug()
        {
            logger = PluginContext.Log;
            logger.Info("Initializing YagodaPlugin");

            subscriptions.Push(new PluginCore(logger));



            PluginContext.Log.Info("SamplePlugin YagodaPlugin");
        }

        public void Dispose()
        {
            while (subscriptions.Any())
            {
                var subscription = subscriptions.Pop();
                try
                {
                    subscription.Dispose();
                }
                catch (RemotingException)
                {
                    // nothing to do with the lost connection
                }
            }

            PluginContext.Log.Info("YagodaPlugin stopped");
        }
    }
}