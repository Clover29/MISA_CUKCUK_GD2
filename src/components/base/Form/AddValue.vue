<template>
  <div class="dialog">
    <div class="dialog__content" style="width: 30%">
      <div class="dialog__header">
        <div class="form__title">{{ title }}</div>
        <div class="dialog__header--rigt">
          <i
            class="fa-solid fa-circle-xmark icon__close"
            @click="btnCloseOnClick"
          ></i>
        </div>
      </div>
      <div class="dialog__body">
        <LoadData v-show="isLoad" />
        <div class="dialog__body--data" style="flex-direction: column">
          <div class="row">
            <label class="form__label"
              >{{ label }}
              <span class="input--required">(*)</span>
            </label>
            <input ref="FirstInput" class="form__text" type="text" v-model="firstValue" />
          </div>
          <div class="row" v-if="type == stockType">
            <label class="form__label"
              >Tên kho
              <span class="input--required">(*)</span>
            </label>
            <input ref="StockName" class="form__text" type="text" v-model="secondValue" />
          </div>
        </div>
        <div class="dialog__footer">
          <button class="btn col__height--28">
            <i class="fa-solid fa-circle-question icon__add"></i>Giúp
          </button>
          <div class="footer__right">
            <button class="btn col__height--28" style="margin-right: 8px" @click="saveOnClick">
              <i class="fa-regular fa-floppy-disk icon__add"></i>Cất
            </button>
            <button class="btn col__height--28" @click="btnCloseOnClick">
              <i class="fa-solid fa-ban icon__delete"></i>Hủy
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <ThePopUp
    v-if="isShowPopUp"
    :message="message"
    @onNoClick="closeNotice"
    :PopUpType="popUpType"
    :closePopUp="closeNotice"
    @focus="setFocus"
  />
</template>
  <script>
import axios from "axios";
import { MISAEnum } from "@/js/Enum";
import { Resources } from "@/js/Resources";
import { Constant } from "@/js/Constant";
import ThePopUp from "../PopUp/ThePopUp.vue";
import LoadData from "../Loading/LoadData.vue";
export default {
  name: "AddValue",
  components: { ThePopUp, LoadData },
  emits: ["loadStock", "loadUnit"],
  props: ["title", "label", "type", "isShow"],
  data() {
    return {
      isLoad: false, //ẩn hiện loading
      unitType: MISAEnum.AddingType.Unit, //loại form add là unit
      stockType: MISAEnum.AddingType.Stock, //loại form add là stock
      firstValue: "", //giá trị hàng đầu tiên trong form
      secondValue: "", //giá trị hàng thứ hai trong form
      unit: {},
      stock: {},
      message: [
        {
          msg: "",
          tittle: "",
        },
      ], // nội dung pop up thông báo
      isShowPopUp: false,
    };
  },
  methods: {
     /**
     * Hàm focus vào input bị lỗi đầu tiên
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    setFocus(){
      try {
        var refName = this.message[0].tittle;
        if (this.$refs[refName]) this.$refs[refName].focus();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *sự kiện cho nút đóng
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    btnCloseOnClick() {
      this.$emit(Constant.EMIT_UPDATE_ISSHOW, false);
    },

    /**
     * Đóng thông báo
     * @param { * } isShow 
     */
    closeNotice(isShow) {
      this.isShowPopUp = isShow;
    },
    /**
     *Hàm validate dữ liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    validate() {
      try {
        this.message = [];
        if (this.firstValue == "") {
          this.message.push({
            msg:
              this.type == this.unitType
                ? Resources.UNIT_ERROR
                : Resources.STOCK_CODE_ERROR,
            tittle: Constant.REF_FISRT_INPUT,
          });
        }
        if (this.type == this.stockType && this.secondValue == "") {
          this.message.push({
            msg: Resources.STOCK_NAME_ERROR,
            tittle: Constant.REF_STOCK_NAME,
          });
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *sự kiện cho nút cất
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    saveOnClick() {
      this.validate();
      if (this.message) {
        if (this.message.length > 0) {
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
        } else {
          if (this.type == this.unitType) {
            this.insertUnit();
          } else {
            this.insertStock();
          }
        }
      }
    },
    /**
     *Thực hiện chức năng thêm đơn vị
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    insertUnit() {
      this.unit = {
        unitName: this.firstValue,
        modifiedDate: new Date(),
      };
      this.isLoad = true;
      axios
        .post(
          Constant.DOMAIN + Constant.API_VER + Constant.UNIT_PATH,
          this.unit
        )
        .then((response) => {
          if (response.data) {
            this.$emit("getEmitShow", true,
              Resources.INSERT_UNIT_SUCCESS,
              MISAEnum.Status.Success);
            this.$emit(Constant.EMIT_UPDATE_ISSHOW, false);
            this.$emit(Constant.EMIT_LOAD_UNIT,true);
          }
        })
        .catch((error) => {
          console.log(error);
          if (error.response.data.errorCode == 3) {
            this.isLoad = false;
            this.isShowPopUp = true;
            this.popUpType = MISAEnum.PopUpType.Error;
            this.message = [];
            this.message.push({
              msg: Resources.DUPLICATE_UNIT,
            });
          } else {
            this.isLoad = false;
            this.isShowPopUp = true;
            this.popUpType = MISAEnum.PopUpType.Error;
            this.message = [];
            this.message.push({
              msg: error.response.data.userMsg,
            });
          }
        });
    },
    /**
     *thực hiện chức năng thêm kho
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    insertStock() {
      this.stock = {
        stockCode: this.firstValue,
        stockName: this.secondValue,
        modifiedDate: new Date(),
      };
      this.isLoad = true;
      axios
        .post(
          Constant.DOMAIN + Constant.API_VER + Constant.STOCK_PATH,
          this.stock
        )
        .then((response) => {
          if (response.data) { 
            this.$emit("getEmitShow", true,
              Resources.INSERT_STOCK_SUCCESS,
              MISAEnum.Status.Success);
            this.$emit(Constant.EMIT_UPDATE_ISSHOW, false);
            this.$emit(Constant.EMIT_LOAD_STOCK,true);
           
          }
        })
        .catch((error) => {
          console.log(error);
          if (error.response.data.errorCode == 3) {
            this.isLoad = false;
            this.isShowPopUp = true;
            this.popUpType = MISAEnum.PopUpType.Error;
            this.message = [];
            this.message.push({
              msg: Resources.DUPLICATE_STOCK,
            });
          } else {
            this.isLoad = false;
            this.isShowPopUp = true;
            this.popUpType = MISAEnum.PopUpType.Error;
            this.message = [];
            this.message.push({
              msg: error.response.data.userMsg,
            });
          }
        });
    },

  },
};
</script>