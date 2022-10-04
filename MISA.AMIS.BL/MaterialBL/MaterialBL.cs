using MISA.AMIS.BL.BaseBL;
using MISA.CUKCUK.Common.DTO;
using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.DL.BaseDL;
using MISA.CUKCUK.DL.MaterialDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.MaterialBL
{
    public class MaterialBL : BaseBL<Material>, IMaterialBL
    {
        #region Feild

        private IMaterialDL _materialDL;

        #endregion

        #region Constructor
        public MaterialBL(IMaterialDL materialDL) : base(materialDL)
        {
            _materialDL = materialDL;
        }

        #endregion
        #region Method
        public string getFillterOption(string keyword)
        {
            if (keyword != null)
            {
                string value = keyword.Substring(1);
                switch (keyword[0])
                {
                    case '*':
                        return $"LIKE '%{value}%'";
                    case '=':
                        return $"='{value}'";
                    case '+':
                        return $"LIKE '{value}%'";
                    case '-':
                        return $"LIKE '%{value}'";
                    case '!':
                        return $"NOT LIKE '%{value}%'";
                    default:
                        return $"LIKE '%{value}%'";
                }
            }
            return null;
        }
        public MaterialFillter setFillterValue(MaterialFillter fillter)
        {
            if (fillter != null)
            {
                fillter.MaterialCode = getFillterOption(fillter.MaterialCode);
                fillter.MaterialName = getFillterOption(fillter.MaterialName);
                fillter.Feature = getFillterOption(fillter.Feature);
                fillter.UnitName = getFillterOption(fillter.UnitName);
                fillter.CategoryName = getFillterOption(fillter.CategoryName);
                fillter.Description = getFillterOption(fillter.Description);
            }
            return fillter;
        }
        public PagingData<Material> FillterMaterial(MaterialFillter? keyword, int pageSize, int pageNumber)
        {
            if (keyword != null)
                keyword = setFillterValue(keyword);
            return _materialDL.FillterMaterial(keyword, pageSize, pageNumber);
        }

        public string GetNewCode(string MaterialName)
        {
          return _materialDL.GetNewCode(MaterialName);
        }
        #endregion
    }
}
