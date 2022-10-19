using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace diplom.library
{
    /// <summary>
    /// Запрос с данными для модели, основанной на логистической регрессии.
    /// </summary>
    public class TRegModelRequest : TRequest
    {
        #region Methods
        /**<summary>Получение данных из JSON.</summary>
         * <param name="_pJson">JSON http-запроса.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public int GetDataFromJson(JObject _pJson, ref string _sError)
        {
            _sError = "";
            int iRes;
            JToken pJMedData;
            JToken pJUserData;
            JToken pJEkgData;
            try
            {             
                
                //получение id пользователя
                JProperty pUserIdProp = _pJson.Property(TConsts.S_JSON_ALIAS_USER_ID);
                fsUserId = pUserIdProp.Value.ToObject<string>();
                //получение объекта медицинских данных
                if(!_pJson.TryGetValue(TConsts.S_JSON_ALIAS_MED_DATA, out pJMedData))
                {
                    _sError = TConsts.S_ERR_READING_REQ_DATA_MED;
                    return TConsts.I_ERR_READING_REQ_DATA_MED;
                }


                //получение объекта личных данных пользователя
                if(!((JObject)pJMedData).TryGetValue(TConsts.S_JSON_ALIAS_USER_DATA, out pJUserData))
                {
                    _sError = TConsts.S_ERR_READING_REQ_DATA_USER;
                    return TConsts.I_ERR_READING_REQ_DATA_USER;
                }
                iRes = GetUserData((JObject)pJUserData,ref _sError);
                if(iRes!=TConsts.I_OK) return iRes;
                
                
                //получение данных ЭКГ
                if(!((JObject)pJMedData).TryGetValue(TConsts.S_JSON_ALIAS_EKG, out pJEkgData))
                {
                    _sError = TConsts.S_ERR_READING_REQ_DATA_EKG;
                    return TConsts.I_ERR_READING_REQ_DATA_EKG;
                }
                iRes = GetEkgData((JObject)pJEkgData, ref _sError);
                if(iRes!=TConsts.I_OK) return iRes;

                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Обработка личных данных пользователя.</summary>
         * <param name="_pJUserData">Личные данные ползователя в виде JSON.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        private int GetUserData(JObject _pJUserData, ref string _sError)
        {
            _sError = "";
            try
            {
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_STIMULATOR,                                                                           //cardiostimulator
                                       TTypeConverter.ConvertToDouble( _pJUserData.Property(TConsts.S_JSON_ALIAS_STIMULATOR).Value.ToObject<bool>()));
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_SMOKING,                                                                             //smoking
                                       TTypeConverter.ConvertToDouble(_pJUserData.Property(TConsts.S_JSON_ALIAS_SMOKING).Value.ToObject<bool>()));
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_DIABETE,                                                                             //diseasediabetes
                                       TTypeConverter.ConvertToDouble(_pJUserData.Property(TConsts.S_JSON_ALIAS_DIABETE).Value.ToObject<bool>()));
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_HYPERTONIA,                                                                          //diseasehypertonia
                                       TTypeConverter.ConvertToDouble(_pJUserData.Property(TConsts.S_JSON_ALIAS_HYPERTONIA).Value.ToObject<bool>()));

                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_AGE,                                                                             //age
                                       _pJUserData.Property(TConsts.S_JSON_ALIAS_AGE).Value.ToObject<double>());
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_GENDER,                                                                          //gender
                                       _pJUserData.Property(TConsts.S_JSON_ALIAS_GENDER).Value.ToObject<double>());
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_WEIGHT_,                                                                          //weight
                                       _pJUserData.Property(TConsts.S_JSON_ALIAS_WEIGHT).Value.ToObject<double>());
                fpModelParamValues.Add(TConsts.S_JSON_ALIAS_HEIGHT,                                                                          //height
                                       _pJUserData.Property(TConsts.S_JSON_ALIAS_HEIGHT).Value.ToObject<double>());

                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Обработка данных ЭКГ пользователя.</summary>
         * <param name="_pJEkgData">Объект данных ЭКГ.</param>
         * <param name="_sError">Строка ошибка.</param>**/
        private int GetEkgData(JObject _pJEkgData, ref string _sError)
        {
            _sError = "";
            List<JProperty> pEkgPropList;
            try
            {
                pEkgPropList = _pJEkgData.Children<JProperty>().ToList<JProperty>();
                foreach(JProperty pCurrProp in pEkgPropList)
                    fpModelParamValues.Add(pCurrProp.Name,pCurrProp.Value.ToObject<double>());

                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }                
        #endregion

        #region Properties
        /**<summary>Id пользователя.</summary>**/
        public override string sUserId
        {
            get { return fsUserId;}
            set { fsUserId = value;}
        }
        /**<summary>Список параметров модели.</summary>**/
        public override Dictionary<string, double> pModelParamValues
        {
            get { return fpModelParamValues; }
            set
            {
                fpModelParamValues = new Dictionary<string, double>();
                foreach(KeyValuePair<string,double> pPair in value)
                    fpModelParamValues.Add(pPair.Key,pPair.Value);
            }

        }   
        #endregion
    }
}
