using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;

namespace diplom.library
{
    /**<summary>Конфигурационные данные сервера.</summary>**/
    public class TConfig
    {
        private string fsConnStr;
        private List<string> fpHttpPrefixes;
        private List<Tuple<string, string, string, string>> fpModelsList;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public TConfig()
        {
            fsConnStr = "";
            fpHttpPrefixes = new List<string>() { };
            fpModelsList = new List<Tuple<string, string, string,string>>() { };
        }
        /**<summary>Конструктор.</summary>
         * <param name="_sConnstr">Строка соединения с БД.</param>
         * <param name="_pHttpPrefixes">Список http-префиксов.</param>
         * <param name="_pModelsList">Список моделей.</param>**/
        public TConfig(string _sConnstr, List<string> _pHttpPrefixes, List<Tuple<string, string, string, string>> _pModelsList)
        {
            fsConnStr = _sConnstr;
            fpHttpPrefixes = _pHttpPrefixes.ToList();
            fpModelsList = _pModelsList.ToList();
        }
        #endregion
                                                                                
        #region Methods
        /**<summary>Чтение файла конфига из XML.</summary>
         * <param name="_sError">Строка ошибки.</param>**/
        public int LoadFromXml(ref string _sError)
        {
            _sError = "";
            try
            {   //0 - получение документа конфига
                XmlDocument pConfig = new XmlDocument();
                pConfig.Load(TConsts.S_CONF_FILEPATH);
                //1 - получение строки подключения к БД
                XmlNode pDbNode = pConfig.SelectSingleNode(TConsts.S_CONF_DB_XPATH);
                fsConnStr = pDbNode.Attributes[TConsts.S_CONN_STR_ATTR_NAME].Value;
                //2 - получение http-префиксов
                XmlNode pHttpParamNode = pConfig.SelectSingleNode(TConsts.S_CONF_HTTP_PARAMS_XPATH);
                foreach(XmlNode pNode in pHttpParamNode.ChildNodes)
                    fpHttpPrefixes.Add(pNode.Attributes[TConsts.S_PREF_ATTR_NAME].Value);
                //3 - получение данных для создания модели
                XmlNode pModelsNode = pConfig.SelectSingleNode(TConsts.S_MODEL_XPATH);
                foreach(XmlNode pNode in pModelsNode.ChildNodes)
                    fpModelsList.Add(new Tuple<string, string, string, string>(pNode.Attributes[TConsts.S_FILEPATH_ATTR_NAME].Value,
                                                                               pNode.Attributes[TConsts.S_CHARACT_FILEPATH_ATTR_NAME].Value,
                                                                               pNode.Attributes[TConsts.S_MODEL_TYPE_ATTR_NAME].Value,
                                                                               pNode.Attributes[TConsts.S_MODEL_MODEL_NAME_ATTR_NAME].Value));
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        #endregion   

        #region Properties
        /**<summary>Строка подключения к БД.</summary>**/
        public string sConnStr
        {
            get { return fsConnStr;}
        }
        /**<summary>Список префиксов для прослушивания http-соединения.</summary>**/
        public List<string> pHttpPrefixes
        {
            get { return fpHttpPrefixes;}
        }
        /**<summary>Список моделей.</summary>**/
        public List<Tuple<string, string, string, string>> pModelsList
        {
            get { return fpModelsList;}
        }
        #endregion

    }
}
