using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Text;


namespace Resto.Front.Api.YagodaPlug
{
    internal class CoreYagoda : System.IDisposable
    {
        private WebClient webClient;
        private Logger logger;

        ///// <summary>
        ///// Параметры подключения к серверу Ягоды.
        ///// </summary>
        private SettingYagodaCore setting;

        public CoreYagoda(Logger logger)
        {
            this.logger = logger;
            var init = new InitYagodaCore(logger);
            setting = init.GetSetting();
        }

        /// <summary>
        /// Подготовка и инициализация WebClient
        /// </summary>
        /// <returns>False при неудачной попытки создать объект.</returns>
        public bool Connect()
        {
            webClient = new WebClient()
            {
                Encoding = Encoding.UTF8
            };

            return webClient == null ? false : true;
        }

        /// <summary>
        /// Получение информации и статуса по номеру телефона.
        /// </summary>
        /// <param name="NumberTel">Номер телефона.</param>
        /// <returns>Класс Entity, десерелизованный Json ответ </returns>
        public Entity GetInfo(string NumberTel)
        {
            string urlRequest = string.Format("{0}:{1}{2}/{3}/getJsonInfo/{4}",
                setting.Url,
                setting.Port,
                setting.PrefixDataBase,
                setting.IdSale,
                NumberTel);

            string responceJson = string.Empty;

            try
            {
                NetworkCredential networkCredential = new NetworkCredential(setting.Login, setting.Password);
                webClient.Credentials = networkCredential;
                responceJson = webClient.DownloadString(new Uri(urlRequest));
            }
            catch (WebException exp)
            {
                var errorString = "Ошибка получения информации по номеру телефона -" + NumberTel;
                errorString += ";" + exp.Message;
                logger.Error(errorString);
            }

            Entity result = JsonConvert.DeserializeObject<Entity>(responceJson);

            return result;
        }

        /// <summary>
        /// Отправка  на сервер покупки.
        /// </summary>
        /// <param name="purchase"></param>
        /// <returns></returns>
        public bool WritePurchase(Purchase purchase)
        {
            var json = JsonConvert.SerializeObject(purchase);
            var response = string.Empty;

            try
            {
                NetworkCredential networkCredential = new NetworkCredential(setting.Login, setting.Password);
                var httpRequest = (HttpWebRequest)WebRequest.Create(new Uri(setting.Url + ":" + setting.Port + setting.PrefixDataBase + "/" + setting.IdSale + "/postdata"));
                httpRequest.Method = "POST";
                httpRequest.Credentials = networkCredential;
                httpRequest.ContentType = "application/json";

                using (var requestStream = httpRequest.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(json);
                }
                using (var httpResponse = httpRequest.GetResponse())
                using (var responseStream = httpResponse.GetResponseStream())
                using (var reader = new StreamReader(responseStream))
                {
                    response = reader.ReadToEnd();
                }
            }
            catch (WebException exp)
            {
                var errorString = "Ошибка записи бонусов для клиента - " + purchase.buyerTel;
                errorString += ";" + exp.Message;
                logger.Error(errorString);
            }
            logger.Info("Ответ сервера при записи покупки - " + response);
            return true;
        }

        /// <summary>
        /// Списание бонусов при оплате бонусами.
        /// </summary>
        /// <param name="purchase">Покупка</param>
        /// <returns>Если списание прошло успешно то true/</returns>
        public bool WriteOffPurchase(Purchase purchase)
        {
            return true;
        }

        /// <summary>
        /// Реализация интерфейся IDisposable, для закрытия рессурсов.
        /// </summary>
        public void Dispose()
        {
            logger.Info("Закрытие рессурсов webClient");
            webClient.Dispose();
        }
    }
}