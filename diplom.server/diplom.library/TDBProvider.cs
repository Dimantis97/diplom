using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace diplom.library
{
    public class TDBProvider
    {
        private string fsConnString;

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public TDBProvider()
        {
            fsConnString = "";
        }
        /**<summary>Конструктор.</summary>**/
        public TDBProvider(string _sConnStr)
        {
            sConnString = _sConnStr;
        }

        #endregion

        #region Methods
        /**<summary>Выполнить команду (без параметров).</summary>
         * <param name="_sCommand">Строка комманды.</param>
         * <param name="_sErr">Строка ошибки.</param>**/
        public int ExecCmd(string _sCommand, ref string _sError)
        {
            _sError = "";
            try
            {
                using(SqlConnection pConnection = new SqlConnection(sConnString))
                {
                    SqlCommand pCommand = new SqlCommand(_sCommand,pConnection);
                    pCommand.Connection.Open();
                    pCommand.ExecuteNonQuery();
                }

                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Выполнить команду (с параметрами) без возврата таблицы.</summary>
         * <param name="_arParams">Набор пар параметров команды.</param>
         * <param name="_sCommand">Строка команды c параметрами типа @DATA0, @DATA1 и т.д.</param>
         * <param name="_sError">Строка ошибки.</param>
         * <remarks>Для вставки параметров в строку команды строка должна содержать @DATA0, @DATA1... на месте значений.</remarks>**/
        public int ExecCmd(string _sCommand, object[] _arParams, ref string _sError)
        {
            SqlParameter pCurrParam;    //текущий параметр sql-комманды
            SqlDbType pCurrParamType;   //тип текущего параметра в формате sql
            _sError = "";
            try
            {
                using(SqlConnection pConnection = new SqlConnection(sConnString))
                {
                    SqlCommand pCommand = new SqlCommand(_sCommand,pConnection);
                    //добавление параметров в комманду
                    for(int i = 0; i < _arParams.Length; i++)
                    {          
                        if(!TTypeConverter.ConvertType(_arParams[i].GetType(), out pCurrParamType))
                        {
                            _sError = TConsts.S_ERR_SQL_PARAM_TYPE_CONV;
                            return TConsts.I_ERR_SQL_PARAM_TYPE_CONV;
                        }                   
                        pCurrParam = new SqlParameter(TConsts.S_DATA_PATT+i.ToString(),pCurrParamType);
                        pCurrParam.Value = _arParams[i];
                        pCommand.Parameters.Add(pCurrParam);
                    }
                    pConnection.Open();
                    pCommand.ExecuteNonQuery();                       
                }             
                return TConsts.I_OK;  
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Выполнить команду (с параметрами) без возврата таблицы.</summary>
         * <param name="_pParamsDict">Словарь параметров, ключ - имя параметра.</param>
         * <param name="_sCommand">Строка команды.</param>
         * <param name="_sError">Строка ошибки.</param>
         * <remarks>Для вставки параметров в строку команды строка должна содержать @DATA0, @DATA1... на месте значений.</remarks>**/
        public int ExecCmd(string _sCommand, Dictionary<string,double> _pParamsDict, ref string _sError)
        {
            _sError = "";
            int i = 0;
            int iRes;
            string sParamsNamesString = "";                      //строка имен параметров для вставки в sql-запрос insert
            string sParamsValuesString = "";
            List<object> pParams = new List<object>();           //список значений параметров
            try
            {
                foreach (KeyValuePair<string, double> pPair in _pParamsDict)
                {
                    pParams.Add(pPair.Value);
                    sParamsNamesString += pPair.Key.ToString();
                    sParamsValuesString += TConsts.S_DATA_PATT+i.ToString();
                    if (i < _pParamsDict.Count - 1)
                    {
                        sParamsNamesString += ",";
                        sParamsValuesString += ",";
                    }

                    i++;
                }
                iRes = this.ExecCmd(string.Format(_sCommand,new object[] {sParamsNamesString,sParamsValuesString }),
                             pParams.ToArray(),
                             ref _sError);
                if(iRes!=TConsts.I_OK) return iRes;
                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary> Получить таблицу по sql-запросу (с параметрами).</summary>
         * <param name="_arParams">Набор параметров.</param>
         * <param name="_ResTable">Результирующая таблица.</param>
         * <param name="_sCommand">Строка sql-</param>
         * <remarks>Для вставки параметров в строку команды строка должна содержать @DATA0, @DATA1... на месте значений.</remarks>**/
        public int Open(string _sCommand, object[] _arParams, out DataTable _ResTable, ref string _sError)
        {
            SqlParameter pCurrParam;
            SqlDbType pCurrParamType;
            SqlDataAdapter pAdapter;
            DataSet pDataSet;
            _sError = "";
            try
            {
                using (SqlConnection pConnection = new SqlConnection(sConnString))
                {
                    SqlCommand pCommand = new SqlCommand(_sCommand,pConnection);
                    //добавление параметров в комманду
                    for(int i = 0; i < _arParams.Length; i++)
                    {          
                        if(!TTypeConverter.ConvertType(_arParams[i].GetType(), out pCurrParamType))
                        {
                            _ResTable = null;
                            _sError = TConsts.S_ERR_SQL_PARAM_TYPE_CONV;
                            return TConsts.I_ERR_SQL_PARAM_TYPE_CONV;
                        }                   
                        pCurrParam = new SqlParameter(TConsts.S_DATA_PATT+i.ToString(),pCurrParamType);
                        pCurrParam.Value = _arParams[i];
                        pCommand.Parameters.Add(pCurrParam);                         
                    }
                    pAdapter = new SqlDataAdapter(pCommand);
                    pConnection.Open();
                    pDataSet = new DataSet();
                    pAdapter.Fill(pDataSet);
                    _ResTable = pDataSet.Tables[0];
                }
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _ResTable = null;
                _sError = E.Message;
                return E.HResult;
            }
        }
        /**<summary>Получить таблицу по sql-запросу (без параметров).</summary>
         * <param name="_pResTable">Результирующая таблица.</param>
         * <param name="_sCommand">Строка sql-команды.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public int Open(string _sCommand, out DataTable _pResTable, ref string _sError)
        {
            _sError = "";
            SqlDataAdapter pAdater;
            DataSet pSet;
            try
            {
                using (SqlConnection pConn = new SqlConnection(sConnString))
                {
                    SqlCommand pCommand = new SqlCommand(_sCommand,pConn);   
                    pAdater = new SqlDataAdapter(pCommand); 
                    pSet = new DataSet();
                    pAdater.Fill(pSet);
                    _pResTable = pSet.Tables[0];
                }
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _pResTable = null;
                _sError = E.Message;
                return E.HResult;
            }
        }          
        #endregion          

        #region Properties
        /**<summary>Строка соединения.</summary>**/
        public string sConnString
        {
            get { return fsConnString;}
            set { fsConnString = value;}
        }
        #endregion

    }
}
