using Resto.Front.Api.V6;
using Resto.Front.Api.V6.Data.View;
using Resto.Front.Api.V6.Extensions;
using Resto.Front.Api.V6.UI;
using Resto.Front.Api.YagodaPlugCore;
using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace Resto.Front.Api.YagodaPlugin
{
    internal sealed class PluginCore : IDisposable
    {
        private readonly CompositeDisposable subscriptions;
        private ILog logger;

        public PluginCore(ILog logger)
        {
            this.logger = logger;
            subscriptions = new CompositeDisposable
            {
             //   PluginContext.Integration.AddButton(new Button("SamplePlugin: Message Button",  (v, p, _) => MessageBox.Show("Message shown from Sample plugin."))),
                PluginContext.Integration.AddButton("Yagoda", ShowListPopup),
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
            IPhoneInputResult inputResult = (IPhoneInputResult)viewManager.ShowExtendedNumericInputPopup("Введите номер телефона:", "Номер телефона",
                new ExtendedInputSettings() { EnablePhone = true });

            logger.Info("После клавиатуры.");
            Entity entity;
            CoreYagoda yagodaCore = null;
            try
            {
                yagodaCore = new CoreYagoda(PluginContext.Log);
            }
            catch (Exception exc)
            {
                logger.Error(exc.Message);
            }

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

            if (yagodaCore == null) { logger.Error("yagodaCore=Null"); }

            entity = yagodaCore.GetInfo(inputResult.PhoneNumber);
            logger.Info("Entity - " + entity);
            string notificationString = string.Format("Имя:{0}, баланс:{1}", entity.profile.name, entity.info.balance);
            PluginContext.Operations.AddNotificationMessage(notificationString, "Yagoda", TimeSpan.FromSeconds(15));
            yagodaCore.Dispose();
        }
    }
}