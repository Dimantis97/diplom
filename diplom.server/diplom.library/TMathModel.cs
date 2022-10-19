using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom.library
{
    /**<summary>Абстрактный родительский класс математической модели.</summary>**/
    public abstract class TMathModel
    {
        protected Dictionary<string,double> fpModelCoef; //коэффициенты логистического уравнения, ключ - название
        protected double ffModelAccur; //точность модели
        protected double ffModelSpec;  //специфичность модели
        protected double ffModelSens;  //чувствительность модели
        protected string fsModelName;  //имя модели

        #region Methods
        /**<summary>Вычисления с помощью математической.</summary>
         * <param name="_pData">Параметры, которые нужно подставить в модель.</param>
         * <param name="_fResult">Результат вычислений.</param>
         * <param name="_sError">Строка ошибки.</param>**/
        public abstract int CalculateModel(Dictionary<string,double> _pData, out int _iResult, ref string _sError);   
        /**<summary>Создание копии данного объекта математической модели.</summary>**/
        public abstract TMathModel Copy();
        #endregion

        #region Properties
        /**<summary>Точность модели.</summary>**/
        public abstract double fModelAccur
        {
            get;
            set;
        }
        /**<summary>Специфичность модели модели.</summary>**/
        public abstract double fModelSpec
        {
            get;
            set;
        }
        /**<summary>Чувствительность модели.</summary>**/
        public abstract double fModelSens
        {
            get;
            set;
        }
        /**<summary>Имя модели.</summary>**/
        public abstract string sModelName
        {
            get;
            set;
        }
        #endregion
    }
}
