using NLog;
using Resto.Front.Api.V6;
using Resto.Front.Api.V6.Extensions;
using Resto.Front.Api.V6.UI;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Resto.Front.Api.YagodaPlugin
{
    internal sealed class PluginCore : IDisposable
    {
        private readonly CompositeDisposable subscriptions;
        private static Logger logger;

        public PluginCore()
        {
            logger = LogManager.GetCurrentClassLogger();
            subscriptions = new CompositeDisposable
            {
             //   PluginContext.Integration.AddButton(new Button("SamplePlugin: Message Button",  (v, p, _) => MessageBox.Show("Message shown from Sample plugin."))),
                PluginContext.Integration.AddButton("Yagoda'", ShowListPopup),
               // PluginContext.Integration.AddButton("SamplePlugin: Print 'Test Keyboard View'", ShowKeyboardPopup)
            };
        }

        public void Dispose()
        {
            subscriptions.Dispose();
        }

        private void ShowListPopup(IViewManager viewManager, IReceiptPrinter receiptPrinter, IProgressBar progressBar)
        {
            var list = new List<string> { "Узнать баланс.", "Начислить бонусы.", "Списать бонусы" };

            var selectedItem = list[2];
            var inputResult = viewManager.ShowChooserPopup("Yagoda", list, i => i, selectedItem, ButtonWidth.Narrower);
            //PluginContext.Operations.AddNotificationMessage(
            //    inputResult == null
            //        ? "Nothing"
            //        : string.Format("Selected : {0}", inputResult),
            //    "SamplePlugin",
            //    TimeSpan.FromSeconds(15));

            ShowKeyboardPopup(viewManager, receiptPrinter, progressBar);
        }

        private void ShowKeyboardPopup(IViewManager viewManager, IReceiptPrinter receiptPrinter, IProgressBar progressBar)
        {
            var inputResult = viewManager.ShowKeyboard("Введите номер телефона:", isMultiline: false, capitalize: true);
            //PluginContext.Operations.AddNotificationMessage(
            //    inputResult == null
            //        ? "Nothing"
            //        : string.Format("Entered : '{0}'", inputResult),
            //    "SamplePlugin",
            //    TimeSpan.FromSeconds(15));
            YagodaPlug.Entity entity;

            var yagodaCore = new YagodaPlug.CoreYagoda(logger);
            try
            {
                logger.Info("Открываем подключение к базе...");
                yagodaCore.Connect();
            }
            catch (Exception exp)
            {
                logger.Error("Ошибка подключения к базе." + exp.Message);
            }
            finally
            {
                logger.Info("Подключение к базе успешно.");
            }

            entity = yagodaCore.GetInfo("79625020828");

            string notificationString = string.Format("Имя:{0}, баланс:{1}", entity.profile.name, entity.info.balance);
            notificationString = "11111";
            PluginContext.Operations.AddNotificationMessage(notificationString, "Yagoda", TimeSpan.FromSeconds(15));
            yagodaCore.Dispose();
        }
    }
}