using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL.StockBL;
using MISA.CUKCUK.Common.Entities;

namespace MISA.CUKCUK.VTHYEN.Controller.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StocksController : BasesController<Stock>
    {
        #region Field

        private IStockBL _stockBL;

        #endregion

        #region Constructor
        public StocksController(IStockBL stockBL) : base(stockBL)
        {
            _stockBL = stockBL;
        }
        #endregion
    }
}
