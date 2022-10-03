﻿using MISA.CUKCUK.Common.Entities;
using MISA.CUKCUK.DL.BaseDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.BL.BaseBL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        protected BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
      
        #endregion

        #region Method
        public int DeleteOneRecord(Guid recordID)
        {
            return _baseDL.DeleteOneRecord(recordID);
        }
        public virtual IEnumerable<T> GetAllRecords()
        {
            return _baseDL.getAllRecords();
        }

        public T GetRecordByID(Guid ID)
        {
            return _baseDL.GetRecordByID(ID);
        }

        public int InsertOneRecord(T record)
        {
            return _baseDL.InsertOneRecord(record);
        }

        public int UpdateOneRecord(T record, Guid recordID)
        {
            return _baseDL.UpdateOneRecord(record, recordID);
        }
        #endregion


    }
}
