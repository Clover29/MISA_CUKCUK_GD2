using MISA.AMIS.BL.BaseBL;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.DL.BaseDL;
using MISA.CUKCUK.DL.StockDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.StockBL
{
    public class StockBL : BaseBL<Stock>, IStockBL
    {
        #region Field

        private IStockDL _stockDL;

        #endregion

        #region Constructor

        public StockBL(IStockDL stockDL) : base(stockDL)
        {
            _stockDL = stockDL;
        }

        #endregion
    }
}
