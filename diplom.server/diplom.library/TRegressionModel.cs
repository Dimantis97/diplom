using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace diplom.library
{
    /**<summary>Модель, основанная на логистической регрессии.</summary>**/
    public class TRegressionModel : TMathModel
    { 
        private float ffTreshValue;  //пороговое значение регрессионного уравнения
        private double ffConstant;    //значение константы       

        #region Constructors
        /**<summary>Конструктор.</summary>**/
        public TRegressionModel()
        {
            fpModelCoef = new Dictionary<string, double>();
            ffTreshValue = (float)0.5;
            ffModelAccur = 0;
            ffModelSens = 0;
            ffModelSpec = 0;
            ffConstant = -1;
        }
        /**<summary>Конструктор.</summary>
         * <param name="_fTreshValue">Пороговое значение уравнения.</param>
         * <param name="_pParams">Список параметров уравнения.</param>
         * <param name="_fModelAccur">Точность модели.</param>
         * <param name="_fModelSens">Чувствительность модели.</param>
         * <param name="_fModelSpec">Специфичность модели.</param>**/
        public TRegressionModel(Dictionary<string,double> _pParams, float _fTreshValue, double _fModelAccur, double _fModelSpec, double _fModelSens, double _fConstant)
        {
            fpModelCoef = new Dictionary<string, double>();
            foreach (KeyValuePair<string,double> pPair in _pParams)
                fpModelCoef.Add(pPair.Key,pPair.Value);
            ffTreshValue = _fTreshValue;
            ffModelSpec  = _fModelSpec;
            ffModelSens  = _fModelSens;
            ffModelAccur = _fModelAccur;
            ffConstant   = _fConstant;
        }
        #endregion

        #region Methods
        /**<summary>Чтение коэффициентов и характеристик модели из csv-файла.</summary>
         * <param name="_cDelim">Символ-делитель для чтения из csv-файла.</param>
         * <param name="_sError">Строка ошибки.</param>
         * <param name="_sCoefFilePath">Путь к csv-файлу коэффициентов уравнения.</param>**/
        public int ReadDataFromCsv(string _sCoefFilePath, string _sCharactFilePath, char _cDelim, ref string _sError)
        {
            _sError = "";
            string sCurrLine;         //текущая строка из csv-файла
            string[] arSubstrings;    //массив подстрок строки из csv-файла
            double fCurrParamValue;   //значение текущего параметра
            try
            {
                //чтение коэффициентов 
                using (StreamReader pReader = new StreamReader(_sCoefFilePath))
                {
                    while ((sCurrLine = pReader.ReadLine())!=null)
                    {
                        arSubstrings = sCurrLine.Split(_cDelim);
                        if(!Double.TryParse(arSubstrings[1], out fCurrParamValue))
                        {
                            _sError = TConsts.S_ERR_READ_CSV;
                            return TConsts.I_ERR_READ_CSV;
                        }
                        if(arSubstrings[0]==TConsts.S_CONST)
                            ffConstant = fCurrParamValue;
                        else
                            fpModelCoef.Add(arSubstrings[0],fCurrParamValue);
                    }
                }
                using (StreamReader pReader = new StreamReader(_sCharactFilePath))
                {
                   while ((sCurrLine = pReader.ReadLine())!=null)
                    {
                        arSubstrings = sCurrLine.Split(_cDelim);
                        if(!Double.TryParse(arSubstrings[1], out fCurrParamValue))
                        {
                            _sError = TConsts.S_ERR_READ_CSV;
                            return TConsts.I_ERR_READ_CSV;
                        }
                        switch (arSubstrings[0])
                        {
                            case TConsts.S_MODEL_ACCUR: ffModelAccur = fCurrParamValue; break;
                            case TConsts.S_MODEL_SENS: ffModelSens = fCurrParamValue; break;
                            case TConsts.S_MODEL_SPEC: ffModelSpec = fCurrParamValue; break;
                            default: break;
                        }

                    }
                }
                    return TConsts.I_OK;
            }
            catch (Exception E)
            {
                _sError = "Ошибка при чтении параметров модели из csv-файла. " + E.Message;
                return E.HResult;
            }
        }
        /**<summary>Вычисление модели.</summary>
         * <param name="_pData">Параметры, которые нужно подставить в модель.</param>
         * <param name="_fResult">Результат работы модели.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public override int CalculateModel(Dictionary<string,double> _pData, out int _iResult, ref string _sError)
        {
            _sError = "";
            double fRegressionEq; //вычисляемое значение логистического уравнения
            double fCurrParam;    //текущий параметр логистического уравнения
            double fRes;
            try
            {   //переносим константу
                fRegressionEq = fConstant;
                foreach(KeyValuePair<string,double> pEntry in fpModelCoef)
                {   //если среди полученных значений экг отсутствует параметр, то ошибка
                    if(!_pData.TryGetValue(pEntry.Key, out fCurrParam))
                    {
                        _sError = TConsts.S_ERR_CALC_MODEL_NO_PARAM;
                        _iResult = -1;
                        return TConsts.I_ERR_CALC_MODEL_NO_PARAM;
                    }
                    //вычисляем новое значение регрессионного уравнения
                    fRegressionEq += pEntry.Value*fCurrParam;
                }
                //вычисляем итоговую вероятность
                fRes  = 1/(1+Math.Pow((Math.E),(-1)*fRegressionEq));
                if(fRes < ffTreshValue)
                    _iResult = 0;
                else 
                    _iResult = 1;
                return TConsts.I_OK;
            }
            catch(Exception E)
            {
                _sError = "Ошибка при вычислении модели. Строка ошибки" + E.Message;
                _iResult = -1;
                return E.HResult;
            }
        }
        /**<summary>Создание копии объекта данной модели.</summary>**/
        public override TMathModel Copy()
        {
            return new TRegressionModel(this.fpModelCoef,this.ffTreshValue,this.ffModelAccur,this.ffModelSpec,this.fModelSens,this.fConstant);        
        }
        /**<summary>Преобразование словаря параметров в JSON-строку.</summary>**/
        public string DictToJsonString()
        {
            return JsonConvert.SerializeObject(fpModelCoef);
        }
        #endregion

        #region Properties
        /**<summary>Точность модели.</summary>**/
        public override double fModelAccur
        {
            get
            {
                return ffModelAccur;
            }

            set
            {
                ffModelAccur = value;
            }
        }
        /**<summary>Чувствительность модели.</summary>**/
        public override double fModelSens
        {
            get
            {
                return ffModelSens;
            }

            set
            {
                ffModelSens = value;
            }
        }
        /**<summary>Специфичность модели.</summary>**/
        public override double fModelSpec
        {
            get
            {
                return ffModelSpec;
            }

            set
            {
                ffModelSpec = value;
            }
        }
        /**<summary>Имя модели.</summary>**/
        public override string sModelName
        {
            get
            {
                return fsModelName;
            }

            set
            {
                fsModelName = value;
            }
        }
        /**<summary>Константа.</summary>**/
        public double fConstant
        {
            get { return ffConstant;}
            set { ffConstant = value;}
        }
        #endregion

    }
}
