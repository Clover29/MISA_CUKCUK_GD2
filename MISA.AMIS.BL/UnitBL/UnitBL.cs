using MISA.AMIS.BL.BaseBL;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.DL.UnitDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.UnitBL
{
    public class UnitBL : BaseBL<Unit>, IUnitBL
    {
        #region Feild
        private IUnitDL _unitDL;
        #endregion

        #region Constructor
        public UnitBL(IUnitDL unitDL) : base(unitDL)
        {
            _unitDL = unitDL;
        }
        #endregion
    }
}
