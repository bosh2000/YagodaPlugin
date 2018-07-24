using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using NLog;

namespace Resto.Front.Api.YagodaPlugCore
{
    /// <summary>
    /// Класс инициализации YagodaCore , чтение настроек подключения.
    /// </summary>
    internal class InitYagodaCore
    {
        private SettingYagodaCore setting;
        private Logger logger;

        public InitYagodaCore(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Десерилизация xml файла с настройками в класс setting/
        /// </summary>
        /// <returns></returns>
        public SettingYagodaCore GetSetting()
        {
            setting=new SettingYagodaCore();
            FileStream fileStream=null;
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SettingYagodaCore));
                fileStream = new FileStream("setting.xml", FileMode.Open);
                setting = (SettingYagodaCore)xmlSerializer.Deserialize(fileStream);
            } catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            finally {
                if (fileStream != null) { fileStream.Close(); };
            }

            return setting;
        }
    }
}