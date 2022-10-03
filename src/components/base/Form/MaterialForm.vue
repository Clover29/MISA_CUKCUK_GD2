<template>
     <div class="dialog">
    <div class="dialog__content">
      <div class="dialog__header">
        <div class="form__title">Thêm nguyên vật liệu</div>
        <div class="dialog__header--rigt">
          <i
            class="fa-solid fa-up-right-and-down-left-from-center icon__resize"
          ></i>
          <i class="fa-solid fa-circle-xmark icon__close" @click="btnCloseOnClick"></i>
        </div>
      </div>
      <div class="dialog__body">
        <div class="dialog__body--data">
          <div class="data__left">
            <div class="row">
              <label class="form__label"
                >Tên
                <span class="input--required">(*)</span>
              </label>
              <input
                class="form__text"
                type="text"
                name="MaterialName"
                value=""
              />
            </div>
            <div class="row">
              <label class="form__label"
                >ĐVT
                <span class="input--required">(*)</span>
              </label>
              <SelectionBox
              :div_class="'select__form--height'"
              :name="'Unit'"
              :value="'Chai'"
              :icon_class="'position__dropdown--21'"
              :input_class="'form__text col_width_100'"
              :isAdd="true"/>
            </div>
            <div class="row">
              <label class="form__label">Hạn sử dụng</label>
              <input class="form__text col__width_34" type="text" name="Expiry" value="" />
              <SelectionBox
              :div_class="'select__form--height col__width_34'"
              :name="'ExpiryDate'"
              :value="''"
              :icon_class="'position__dropdown--9'"
              :input_class="'form__text col_width_100'"
              />
            </div>
          </div>
          <div class="data__right">
            <div class="row">
              <label class="form__label"
                >Mã
                <span class="input--required">(*)</span>
              </label>
              <input
                class="form__text"
                type="text"
                name="MaterialName"
                value=""
              />
            </div>
            <div class="row">
              <label class="form__label">Kho ngầm định</label>
              <SelectionBox
              :div_class="'select__form--height col_width_70'"
              :name="'Stock'"
              :icon_class="'position__dropdown--21'"
              :input_class="'form__text col_width_100'"
              :isAdd="true"
              />
            </div>
            <div class="row">
              <label class="form__label">SL tồn tối thiểu</label>
              <input class="form__text col__width_34" type="text" name="Number" value="" />
            </div>
          </div>
        </div>
        <div class="row row-60">
          <label class="form__label" style="width: 15%">Mô tả</label>
          <textarea
            class="text_description"
            name="Description"
            value=""
          ></textarea>
        </div>
        <div class="body__line">
          <div class="line__title">Đơn vị chuyển đổi</div>
        </div>
        <div class="body__table">
          <table class="table__data">
            <tr>
              <th>STT</th>
              <th>Đơn vị chuyển đổi</th>
              <th>Tỷ lệ chuyển đổi</th>
              <th>Phép tính</th>
              <th>Mô tả</th>
            </tr>
            <tr v-for="item in listUnitChange" :key="item.numerical">
              <td class="col-1">{{item.numerical}}</td>
              <td>{{item.unitChange}}</td>
              <td class="col-3">{{item.rate}}</td>
              <td>{{item.rate}}</td>
              <td class="col-5">{{item.description}}</td>
            </tr>
          </table>
        </div>
        <div class="table_button">
          <button class="btn" style="margin-right: 8px" @click="btnAddRowOnClick">
            <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm dòng
          </button>
          <button class="btn" @click="btnDeleteRowOnClick" :disabled="listUnitChange.length == 0">
            <i class="fa-solid fa-xmark icon__delete"></i>Xóa dòng
          </button>
        </div>
        <div class="dialog__footer">
          <button class="btn">
            <i class="fa-solid fa-circle-question icon__add"></i>Giúp
          </button>
          <div class="footer__right">
            <button class="btn" style="margin-right: 8px">
              <i class="fa-regular fa-floppy-disk icon__add"></i>Cất
            </button>
            <button class="btn" style="margin-right: 8px">
              <i class="fa-solid fa-floppy-disk icon__add"></i>Cất và thêm
            </button>
            <button class="btn">
              <i class="fa-solid fa-ban icon__delete"></i>Hủy
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
  @import url(dialog.css);
  </style>
 <script>
  import SelectionBox from "../SelectionBox/SelectionBox.vue";
 export default {
  name:"MaterialForm",
  components:{ SelectionBox },
  props: ["showForm"],
  data(){
    return {
      listUnitChange:[], // danh sách đơn vị chuyển đổi
      numerical:1, // số thự tự trong bảng đơn vị chuyển đổi
    }
  },
  methods: {
    /**
     *Hàm đóng form
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnCloseOnClick(){
      this.showForm(false);
    },
    /**
     *Hàm Thêm dòng mới trong bảng đơn vị chuyển đổi
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnAddRowOnClick(){
      this.listUnitChange.push({numerical:this.numerical++})
    },
    /**
     *Hàm xóa dòng trong bảng đơn vị chuyển đổi
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnDeleteRowOnClick(){
      this.listUnitChange.pop();
      this.numerical--;
    }
  },
 }
 </script>