using System;          
using System.Collections.Generic;                                                 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace diplom.library
{
    /**<summary>HTTP-сервер.</summary>**/
    public class THTTPServer
    {
        private List<string> fpPrefixesList;                 //список префиксов
        private Dictionary<string,TMathModel> fpModelsDict;  //список моделей
        private HttpListener fpListener;                     //объект для прослушивания http-соединений

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public THTTPServer()
        {
            fpPrefixesList = new List<string>() { };
            fpModelsDict = new Dictionary<string, TMathModel>();
            fpListener = new HttpListener();
        }
        /**<summary>Конструктор.</summary>
         * <param name="_pModelsDict">Список моделей с типами в виде ключей.</param>
         * <param name="_pPrefixesList">Список префиксов.</param>**/
        public THTTPServer(List<string> _pPrefixesList, Dictionary<string,TMathModel> _pModelsDict)
        {
            fpModelsDict = new Dictionary<string, TMathModel>();
            foreach(KeyValuePair<string,TMathModel> pEntry in _pModelsDict)
                fpModelsDict.Add(pEntry.Key,pEntry.Value.Copy());
            fpPrefixesList = _pPrefixesList.ToList();   
            fpListener = new HttpListener(); 
        }
        #endregion

        #region Methods
        /**<summary>Исполнение работы http-сервера (в параллельном процессе).</summary>
         * <param name="_oLog">Объект лога.</param>**/
        public void Exec(object _oLog)
        {
            TLog pLog = new TLog();
            pLog.sMsg = "";
            pLog.iRes = TConsts.I_OK;
            try
            {
                //занесение всех префиксов
                foreach(string sPref in fpPrefixesList)
                    fpListener.Prefixes.Add(sPref);
            }
            catch(Exception E)
            {
                pLog = new TLog();
                pLog.sMsg = E.Message;
                pLog.iRes = E.HResult;
                _oLog = (object)pLog;
            }
            finally
            {
                if(fpListener!=null)
                    if(fpListener.IsListening)
                        fpListener.Stop();
                _oLog = (object)pLog;
            }
        }
        /**<summary>Исполнение работы http-сервера (в главном процессе).</summary>
         * <param name="_pProvider">Провайдер к БД.</param>
         * <param name="_pLog">Объект лога.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public int Exec(TDBProvider _pProvider, TLog _pLog, ref string _sError)
        {
            _sError = "";
            int iRes = TConsts.I_OK;
            bool fIsListening = true;
            byte[] arBuff;

            HttpListenerContext pContext;
            HttpListenerRequest pReq;
            THandler pHandler;

            JObject pJRespObj;

            try
            {
                foreach(string sPref in fpPrefixesList)
                    fpListener.Prefixes.Add(sPref);
                Console.WriteLine("Waiting for connections...");
                fpListener.Start();
                while(fIsListening)
                {
                    pContext = fpListener.GetContext();
                    pReq = pContext.Request;
                    Console.WriteLine("Incomming connection.");
                    pHandler = new THandler(pReq, fpModelsDict);
                    iRes = pHandler.Exec(_pProvider,out pJRespObj,ref _sError);
                    if (iRes != TConsts.I_OK) 
                        pContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    else
                        pContext.Response.StatusCode = (int)HttpStatusCode.OK;
                    arBuff = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pJRespObj));
                    pContext.Response.ContentLength64 = arBuff.Length;
                    Stream pStream = pContext.Response.OutputStream;
                    pStream.Write(arBuff,0,arBuff.Length);
                    pStream.Close();
                    Console.WriteLine("Connection successfully closed.");
                    //запись лога, если ошибка в работе хэндлера
                    if (iRes != TConsts.I_OK)
                        _pLog.WriteLog(iRes,_sError);

                }
                return iRes;
            }
            catch (Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
            finally
            {
                if(fpListener!=null)
                    if(fpListener.IsListening)
                        fpListener.Stop();
            }
        }
        /**<summary>Остановка http-сервера.</summary>
         * <param name="_sEttor">Строка ошибки.</param>**/
        public int Stop(ref string _sError)
        {
            _sError = "";
            try
            {
                if(fpListener!=null)
                    if(fpListener.IsListening)
                        fpListener.Stop();
                return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _sError = E.Message;
                return E.HResult;
            }
        }
        #endregion
    }
}
