using System;
using System.Reactive.Disposables;
using Resto.Front.Api.V6;

namespace Resto.Front.Api.YagodaPlugin
{
    class YagodaPlugin : IFrontPlugin
    {
        private readonly CompositeDisposable subscriptions;
        
        public void Dispose()
        {
            subscriptions?.Dispose();
        }
    }
}