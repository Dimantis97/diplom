using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;


namespace diplom.library
{
    public class THandler
    {
        private HttpListenerRequest fpRequest;
        private Dictionary<string,TMathModel> fpModelsDict;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public THandler(HttpListenerRequest _pReq, Dictionary<string,TMathModel> _pModelsDict)
        {
            fpRequest = _pReq;
            fpModelsDict = new Dictionary<string, TMathModel>();
            foreach(KeyValuePair<string,TMathModel> pPair in _pModelsDict)
                fpModelsDict.Add(pPair.Key,pPair.Value.Copy());   
        }
        #endregion

        #region Methods
        /**<summary>Обработка вычислений по запросу.</summary>
         * <param name="_sError">Строка ошибки.</param>
         * <param name="_pResp">Ответ на запрос.</param>**/
        public int Exec(TDBProvider _pProvider, out JObject _pJRespObj, ref string _sError)
        {               
            int iRes;
            string sReqText;
            string sModelName;
            
            _sError = "";

            JObject pJReqData;
            TMathModel pCurrModel;
            TRegressionModel pCurrRegModel;
            TRegModelRequest pModelReqData;
            try
            {                       
                using (StreamReader pReqStream = new StreamReader( fpRequest.InputStream, fpRequest.ContentEncoding))
                    sReqText = pReqStream.ReadToEnd();     
                pJReqData = JObject.Parse(sReqText);                                                        //получение json запроса
                sModelName = pJReqData.Property(TConsts.S_JSON_ALIAS_MODEL).Value.ToObject<string>();       //получение имени модели, которую предполагается использовать  
                switch(sModelName)                                                                          //проверка имени запрошенной модели
                {
                    case TConsts.S_JSON_VALUE_MODEL_LOG_REG:                                                //если логистическа регрессия
                        {
                            //получение данных запроса
                            pModelReqData = new TRegModelRequest();
                            iRes = pModelReqData.GetDataFromJson(pJReqData, ref _sError);
                            if (iRes != TConsts.I_OK)
                            {
                                _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR, _sError), new JProperty(TConsts.S_I_RES, iRes));
                                return iRes;
                            }
                            //сохранение данных в БД
                            iRes = SaveLogRegReqDataToDB(pModelReqData, _pProvider, ref _sError);
                            if (iRes != TConsts.I_OK)
                            {
                                _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR,_sError),
                                                         new JProperty(TConsts.S_I_RES,iRes));
                                return iRes;
                            }
                            //получение модели
                            if (!fpModelsDict.TryGetValue(sModelName, out pCurrModel))
                            {
                                _sError = TConsts.S_ERR_HANDL_RETRIEV_MODEL_BY_NAME;
                                _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR,TConsts.S_ERR_HANDL_RETRIEV_MODEL_BY_NAME),
                                                         new JProperty(TConsts.S_I_RES,TConsts.I_ERR_HANDL_RETRIEV_MODEL_BY_NAME));
                                return TConsts.I_ERR_HANDL_RETRIEV_MODEL_BY_NAME;
                            }
                            pCurrRegModel = (TRegressionModel)pCurrModel;
                            iRes = ExecLogicalRegression(pCurrRegModel,pModelReqData,out _pJRespObj, ref _sError);
                            return iRes;
                        }
                    default: _pJRespObj = new JObject(); return TConsts.I_OK;;
                }         
            }
            catch(Exception E)
            {
                _sError = E.Message;
                _pJRespObj = new JObject(new JProperty("sError",E.Message),new JProperty("iRes",E.HResult));
                return E.HResult;
            }
        }
        /**<summary>Обработка логистической регрессии.</summary>
         * <param name="_pJReqData">Данные http-запроса в формате json.</param>
         * <param name="_pJRespObj">Ответ на https-запрос в офрмате json.</param>**/
        private int ExecLogicalRegression(TRegressionModel _pModel, TRegModelRequest _pReqData, out JObject _pJRespObj, ref string _sError)
        {
            _sError = "";
            int iRes;
            int iModelResult;

            try
            {
                //осуществление вычисления на основе модели
                iRes = _pModel.CalculateModel(_pReqData.pModelParamValues, out iModelResult,ref _sError);
                if(iRes != TConsts.I_OK)
                {
                    _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR,_sError),new JProperty(TConsts.S_I_RES,iRes));
                    return iRes;
                }
                //получение ответа 
                iRes = FormLogRegResponse(_pModel,iModelResult,out _pJRespObj, ref _sError);
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _sError = E.Message;
                _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR,E.Message),new JProperty(TConsts.S_I_RES,E.HResult));
                return E.HResult;    
            }
        }
        /**<summary>Формирование объекта ответа в формате JSON (логистическая регрессия).</summary>
         * <param name="_iModelResult">Резултат вычисления модели.</param>
         * <param name="_pJRespObj">Объект ответа в формате JSON.</param>
         * <param name="_pModel">Объект модели.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        private int FormLogRegResponse(TRegressionModel _pModel, int _iModelResult, out JObject _pJRespObj, ref string _sError)
        {
            _sError = "";
            try
            {
                //характеристики модели
                JObject pModelCharact = new JObject(new JProperty(TConsts.S_SENSITIVITY,_pModel.fModelSens),
                                                    new JProperty(TConsts.S_SPECIFICITY,_pModel.fModelSpec),
                                                    new JProperty(TConsts.S_ACCURACY,_pModel.fModelAccur));
                JProperty pCalcId     = new JProperty(TConsts.S_CALCULATE_ID,-1);
                JProperty pResOfModel = new JProperty(TConsts.S_RES_OF_MODEL, _iModelResult);
                _pJRespObj = new JObject(new JProperty(TConsts.S_MODEL_CHARACT,pModelCharact),pCalcId,pResOfModel);
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _pJRespObj = new JObject(new JProperty(TConsts.S_ERROR,_sError),new JProperty(TConsts.S_I_RES,E.HResult));
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Сохранение данных запроса к модели логистической регрессии в БД.</summary>
         * <param name="_pProvider">Провайдер к БД.</param>
         * <param name="_pReq">Данные запроса к модели.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public int SaveLogRegReqDataToDB(TRegModelRequest _pReq, TDBProvider _pProvider, ref string _sError)
        {
            _sError = "";
            int iRes;
            int i;
            string sInternalId;
            string sParamsNamesString = "";                      //строка имен параметров для вставки в sql-запрос insert
            string sParamsValuesString = "";                     //строка паттернов значений параметров параметров для вставки в sql-запрос insert
            string sCommand;

            List<object> pParams = new List<object>();           //список значений параметров
            DataTable pTable;

            try
            {
                iRes = _pProvider.Open(TConsts.S_SQL_SELECT_USERS, new object[] { _pReq.sUserId }, out pTable, ref _sError);
                if (iRes != TConsts.I_OK) return iRes;
                if (pTable.Rows.Count == 0)
                {    //если по данному внешнему id не найдено совпадений
                    sInternalId = TTypeConverter.GetHash(_pReq.sUserId);
                    iRes = _pProvider.ExecCmd(TConsts.S_SQL_INSERT_INTO_USERS, new object[] { sInternalId, _pReq.sUserId }, ref _sError);
                    if (iRes != TConsts.I_OK) return iRes;
                }
                else
                    sInternalId = pTable.Rows[0].Field<string>(TConsts.S_SQL_INTERNAL_ID);
                sCommand = string.Format(TConsts.S_SQL_INSERT_SAMPLES, new object[] {TConsts.S_SQL_PATT_PATIENT_ID, "'"+sInternalId+"'"+TConsts.S_SQL_PATT_FIRST_PLACE_TO_FRM});
                iRes = _pProvider.ExecCmd(sCommand,_pReq.pModelParamValues,ref _sError);
                return iRes;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }

        #endregion
    }
}
