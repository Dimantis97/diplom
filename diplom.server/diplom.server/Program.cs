using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading;
using diplom.library;

namespace diplom.server
{
    public class TServer
    {
        private static TConfig fpConfig;                          //конфиг
        private static TLog fpLog;                                //объект лога
        private static THTTPServer fpHttpServer;                  //http-сервер
        private static Dictionary<string,TMathModel> fpModelList; //список математических моделей
        private static TDBProvider fpProvider;                    //провайдер к БД
               

        static void Main(string[] args)
        {
            int iRes;
            string sError = "";
            TMathModel pCurrModel;
            try
            {
                //создание объекта лога
                fpLog = new TLog(TConsts.S_LOG_FILEPATH);
                //чтение конфига
                fpConfig = new TConfig();
                iRes = fpConfig.LoadFromXml(ref sError);
                if (iRes != TConsts.I_OK)
                {
                    Console.WriteLine("Error while reading config. Error code: "+iRes.ToString()+" Error string: "+sError);
                    if (fpLog.WriteLog(iRes, sError) != TConsts.I_OK)
                        Console.WriteLine("Crititcal error. Failed to write log"); 
                    Console.WriteLine("Press any key to end the program.");                     
                    Console.ReadKey();
                    return;
                }
                Console.WriteLine("Reading config successful.");
                //создание провайдера к БД
                fpProvider = new TDBProvider(fpConfig.sConnStr);
                //создание объектов математических моделей
                fpModelList = new Dictionary<string, TMathModel>() { };
                foreach(Tuple<string, string, string,string> pModelData in fpConfig.pModelsList)
                {
                    if (pModelData.Item3 == TConsts.S_MODEL_TYPE_LOG_REG)
                    {
                        pCurrModel = new TRegressionModel();
                        pCurrModel.sModelName = pModelData.Item4;
                        iRes = ((TRegressionModel)pCurrModel).ReadDataFromCsv(pModelData.Item1, pModelData.Item2, TConsts.C_DELIM, ref sError);
                        if (iRes != TConsts.I_OK)
                        {
                            Console.WriteLine("Error while creating mathmodels. Error code: " + iRes.ToString() + " Error string: " + sError);
                            if (fpLog.WriteLog(iRes, sError) != TConsts.I_OK)
                                Console.WriteLine("Crititcal error. Failed to write log.");
                            Console.WriteLine("Press any key to end the program.");
                            Console.ReadKey();
                            return;
                        }
                        fpModelList.Add(pModelData.Item4,pCurrModel);
                    }  
                }
                Console.WriteLine("Creating mathmodels successful.");    
                //запуск http-сервера 
                Console.WriteLine("Running http-server...");
                fpHttpServer = new THTTPServer(fpConfig.pHttpPrefixes,fpModelList);
                iRes = fpHttpServer.Exec(fpProvider, fpLog, ref sError);
                if(iRes!=TConsts.I_OK)
                    Console.WriteLine("Error while running http-server. Error code: "+iRes.ToString()+" Error string: "+sError);                 
            }   
            catch(Exception E)
            {
                Console.WriteLine("Unexpected error occured. Error code: "+E.HResult+". Error string: "+E.Message);
                if (fpLog.WriteLog(E.HResult, E.Message) != TConsts.I_OK)
                                Console.WriteLine("Crititcal error. Failed to write log.");
                Console.WriteLine("Press any key to end the program.");
                Console.ReadKey();
                return;
            }
        }

    }
}
