using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.BL.BaseBL;
using MISA.AMIS.BL.UnitBL;
using MISA.CUKCUK.Common.Entities;

namespace MISA.CUKCUK.VTHYEN.Controller.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UnitsController : BasesController<Unit>
    {
        #region Field

        private IUnitBL _UnitBL;

        #endregion

        #region Constructor
        public UnitsController(IUnitBL unitBL) : base(unitBL)
        {
            _UnitBL = unitBL;
        }

        #endregion
    }
}
