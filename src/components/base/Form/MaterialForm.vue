<template>
  <div
    class="dialog"
    @keydown.esc="showPopUp(true)"
    @keydown.ctrl.s.prevent.exact="saveOnClick(false)"
    @keydown.ctrl.shift.s.prevent="saveOnClick(true)"
  >
    <div class="dialog__content">
      <div class="dialog__header">
        <div class="form__title">{{ Title }} nguyên vật liệu</div>
        <div class="dialog__header--rigt" title="Đóng (Esc)">
          <i
            class="fa-solid fa-circle-xmark icon__close"
            @click="showPopUp(true)"
          ></i>
        </div>
      </div>
      <div class="dialog__body">
        <LoadData v-show="isLoad" />
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
                ref="MaterialName"
                @blur="getCode"
                @input="getValue('MaterialName')"
                v-model="material.materialName"
                @keydown.shift.tab.prevent="repeatTabIndex('cancel')"
              />
            </div>
            <div class="row">
              <label class="form__label" title="Đơn vị tính"
                >ĐVT
                <span class="input--required">(*)</span>
              </label>
              <ComboboxVue
                ref="Unit"
                :width="'col_width_70'"
                :dropdown__icon="'position__dropdown--72'"
                :div_class="'col_width_45'"
                :plus__icon="'position__plus--50'"
                :UnitList="unitList"
                v-model:ID="material.unitID"
                v-model:Name="material.unitName"
                @addValue="addValue(true)"
                @validateUnit="checkDuplicateUnit"
              />
            </div>
            <div class="row">
              <label class="form__label">Hạn sử dụng</label>
              <input
                class="form__text col__width_34 text-right"
                type="text"
                maxlength="5"
                @blur="formatDate"
                @keypress="isNumber($event)"
                v-model="expiryDate"
              />
              <SelectionBox
                :isSelectShow="true"
                :div_class="'select__form--height col__width_34'"
                :option="expiry"
                :optionID="expiry[0].id"
                :input_class="'form__text col_width_100 col_height_30'"
                v-model:value="expiryTime"
                @formatDate="getExpiryDate(this.material.expiryDate)"
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
                maxlength="25"
                ref="MaterialCode"
                @input="getValue('MaterialCode')"
                v-model="material.materialCode"
              />
            </div>
            <div class="row">
              <label class="form__label">Kho ngầm định</label>
              <ComboboxVue
                ref="Stock"
                :dropdown__icon="'position__dropdown--21'"
                :text_class="'col_width_100'"
                :div_class="'col_width_70'"
                :StockList="stockList"
                v-model:ID="material.stockID"
                v-model:Name="material.stockName"
                @addValue="addValue(false)"
              />
            </div>
            <div class="row">
              <label class="form__label" title="Số lượng tồn tối thiểu"
                >SL tồn tối thiểu</label
              >
              <input
                class="form__text col__width_34 text-right"
                type="text"
                name="Number"
                maxlength="10"
                v-model="material.inventoryNumber"
                @blur="formatRate(material.inventoryNumber)"
              />
            </div>
          </div>
        </div>
        <div class="row row-60">
          <label class="form__label" style="width: 15%">Mô tả</label>
          <textarea
            class="text_description row-60"
            name="Description"
            v-model="material.description"
          ></textarea>
        </div>
        <div class="body__line">
          <div class="line__title">Đơn vị chuyển đổi</div>
        </div>
        <div class="body__table">
          <table class="table__data">
            <tr class="table__header">
              <th class="col-1" title="Số thứ tự">STT</th>
              <th class="col-5">Đơn vị chuyển đổi</th>
              <th class="col-3">Tỷ lệ chuyển đổi</th>
              <th>Phép tính</th>
              <th class="col-5">Mô tả</th>
            </tr>
            <tr
              v-for="(item, index) in material.conversionUnits"
              :key="item.numerical"
              v-show="material.conversionUnits[index].action != 3"
            >
              <td class="col-1">{{ index + 1 }}</td>
              <td class="col-5">
                <ComboboxVue
                  ref="UnitChange"
                  :dropdown__icon="'position__dropdown--21'"
                  :text_class="'col_width_100'"
                  :UnitList="unitList"
                  v-model:ID="item.conversionUnitID"
                  v-model:Name="item.conversionUnitName"
                  @addValue="addValue(true)"
                  @validateUnit="checkDuplicateUnit(index)"
                />
              </td>
              <td class="col-3">
                <input
                  class="row__input"
                  type="text"
                  maxlength="10"
                  v-model="item.conversionRate"
                  @blur="formatRate(item.conversionRate, index)"
                />
              </td>
              <td>
                <SelectionBox
                  :isSelectShow="true"
                  :div_class="'select__form--height'"
                  :option="calculation"
                  v-model:optionID="item.calculation"
                  :value="
                    item.calculation
                      ? calculation[item.calculation - 1]?.value
                      : calculation[0]
                  "
                  :input_class="'form__text col_width_100 col_height_30'"
                  :optionType="rateType"
                />
              </td>
              <td class="col-5">
                <div
                  style="height: fit-content"
                  v-if="
                    item.calculation == 2 &&
                    material.unitName &&
                    item.conversionUnitName
                  "
                >
                  1 {{ material.unitName }} = 1 / {{ item.conversionRate }}
                  {{ item.conversionUnitName }}
                </div>
                <div
                  style="height: fit-content"
                  v-if="
                    item.calculation == 1 &&
                    material.unitName &&
                    item.conversionUnitName
                  "
                >
                  1 {{ material.unitName }} ={{ item.conversionRate }} *
                  {{ item.conversionUnitName }}
                </div>
              </td>
            </tr>
          </table>
        </div>
        <div class="table_button">
          <button
            class="btn col__height--28"
            style="margin-right: 8px"
            @click="btnAddRowOnClick"
          >
            <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm dòng
          </button>
          <button
            class="btn col__height--28"
            @click="btnDeleteRowOnClick"
            :disabled="
              (material.conversionUnits &&
                material.conversionUnits.length == 0) ||
              !material.conversionUnits
            "
          >
            <i class="fa-solid fa-xmark icon__delete"></i>Xóa dòng
          </button>
        </div>
        <div class="dialog__footer">
          <button class="btn col__height--28">
            <i class="fa-solid fa-circle-question icon__add"></i>Giúp
          </button>
          <div class="footer__right">
            <button
              class="btn col__height--28"
              style="margin-right: 8px"
              @click="saveOnClick(false)"
              title="Cất (Ctrl + S)"
            >
              <i class="fa-regular fa-floppy-disk icon__add"></i>Cất
            </button>
            <button
              ref="save"
              title="Cất và thêm (Ctrl + Shift + S)"
              class="btn col__height--28"
              style="margin-right: 8px"
              @click="saveOnClick(true)"
            >
              <i class="fa-solid fa-floppy-disk icon__add"></i>Cất và thêm
            </button>
            <button
              ref="cancel"
              class="btn col__height--28"
              @click="showPopUp(true)"
              title="Đóng (Esc)"
              @keydown.tab.prevent="repeatTabIndex('MaterialName')"
              @keydown.shift.tab.prevent.exact="repeatTabIndex('save')"
            >
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
    @onYesClick="saveOnClick(false)"
    @onNoClick="btnCloseOnClick"
    :PopUpType="popUpType"
    :closePopUp="showPopUp"
    @focus="setFocus"
  />
  <AddValue
    :title="title"
    :label="label"
    :type="addingType"
    v-if="isShowAdding"
    v-model:isShow="isShowAdding"
    @loadUnit="getAllUnit"
    @loadStock="getAllStock"
    @getEmitShow="setEmitAlert"
  />
</template>
<style scoped>
@import url(dialog.css);
</style>
 <script>
import axios from "axios";
import { Resources } from "../../../js/Resources.js";
import { Constant } from "../../../js/Constant.js";
import SelectionBox from "../SelectionBox/SelectionBox.vue";
import ComboboxVue from "../Combobox/ComboBoxVue.vue";
import { MISAEnum } from "@/js/Enum.js";
import ThePopUp from "../PopUp/ThePopUp.vue";
import LoadData from "../Loading/LoadData.vue";
import AddValue from "./AddValue.vue";

export default {
  name: "MaterialForm",
  components: { SelectionBox, ComboboxVue, ThePopUp, LoadData, AddValue },
  emits: ["LoadData", "showAlert", "AddEvent"],
  props: ["showForm", "MaterialID", "ActionType", "Title"],
  data() {
    return {
      isSave: false, //kiểm tra button save đã đc click hay chưa
      isShowAdding: false, // hiển thị form thêm mới
      addingType: MISAEnum.AddingType.Unit, // xác định form thêm mới cho đơn vị hay kho
      title: Resources.UNIT_TITLE, // tittle cho form thêm mới
      label: Resources.UNIT_LABEL, // nhãn cho form thêm mới
      isLoad: false, // ẩn hiện component loading
      popUpType: 0, // kiểu thông báo hiện lên
      isShowPopUp: false, // ẩn hiển pop up
      message: [
        {
          msg: "",
          tittle: "",
        },
      ], // nội dung pop up thông báo
      unitList: [], // danh sách đơn vị
      stockList: [], //danh sách kho ngầm định
      expiry: [
        { id: 1, value: MISAEnum.Date.Day, text: MISAEnum.Date.Day },
        { id: 2, value: MISAEnum.Date.Week, text: MISAEnum.Date.Week },
        { id: 3, value: MISAEnum.Date.Month, text: MISAEnum.Date.Month },
        { id: 4, value: MISAEnum.Date.Year, text: MISAEnum.Date.Year },
      ], // list hạn sử dụng
      expiryDate: null, // số thời gian của hạn sử dụng
      expiryTime: MISAEnum.Date.Day, //"giá trị hạn sử dụng"
      calculation: [
        {
          id: 1,
          value: MISAEnum.Calculation.Multiple,
          text: MISAEnum.Calculation.MultipleText,
        },
        {
          id: 2,
          value: MISAEnum.Calculation.Divide,
          text: MISAEnum.Calculation.DivideText,
        },
      ], // list phép tính cần thực hiện
      rateType: MISAEnum.OptionType.Calculation, // loại phép tính
      material: {
        materialCode: "",
        materialName: "",
        expiryDate: null,
        unitID: "",
        unitName: "",
        stockID: null,
        stockName: null,
        inventoryNumber: "0,00",
        description: "",
        status: 1,
        createdBy: "admin",
        createdDate: new Date(),
        modifiedBy: "admin",
        modifiedDate: new Date(),
        conversionUnits: [],
      }, //thông tin nguyên vật liệu
      materialFillter: {
        MaterialCode: "",
        MaterialName: "",
        Feature: "",
        UnitName: "",
        CategoryName: "",
        Description: "",
        Status: 1,
      }, //danh sách các thuộc tính lọc nguyên vật liệu
      isChange: false, // xác định người dùng đã sửa form hay chưa
      pageSize: 50, //số bản ghi muốn hiển thị trên một trang
      deleteIndex: 0, // index dòng cần xóa
    };
  },
  computed: {
    minimumStock: {
      get() {
        let formatNumber = new Intl.NumberFormat("de-DE", {
          minimumFractionDigits: 2,
        });
        // let formatNumber = new Intl.NumberFormat('de-DE')
        if (this.material?.inventoryNumber) {
          return formatNumber.format(this.material?.inventoryNumber);
        }
        return "";
      },
      set(newVal) {
        newVal = newVal.replaceAll(".", "");
        if (!this.material) {
          this.material = {};
        }
        this.material.inventoryNumber = parseInt(newVal);
      },
    },
  },
  watch: {
    material: {
      handler() {
        this.isChange = true;
      },
      deep: true,
    },
  },
  methods: {
    /**
     * format số sang kiểu 0,00
     * @param value: giá trị cần format
     * @param index: chỉ số của tỷ lệ cần format
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    formatRate(value, index) {
      if (value < 0 || value == "" || !value.toString().match(/[0-9]+/g)) {
        if (index || index >= 0)
          this.material.conversionUnits[index].conversionRate = "1,00";
        this.material.inventoryNumber = "0,00";
      }
      let formatNumber = new Intl.NumberFormat("de-DE", {
        minimumFractionDigits: 2,
      });
      if ( this.material.inventoryNumber.toString().search(',') < 0) {
        this.material.inventoryNumber = parseFloat(
          this.material.inventoryNumber
            .toString()
            .replace(".", ",")
        );
        this.material.inventoryNumber = formatNumber.format(
          this.material.inventoryNumber
        );
      }

      if (index >= 0 && this.material.conversionUnits[index] &&value.toString().search(',') < 0) {
        this.material.conversionUnits[index].conversionRate = parseFloat(
          this.material.conversionUnits[index].conversionRate
            .toString()
            .replace(".", ",")
        );
        this.material.conversionUnits[index].conversionRate =
          formatNumber.format(
            this.material.conversionUnits[index].conversionRate
          );
      }
    },
    /**
     * quay lại tab index
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    repeatTabIndex(value) {
      this.$refs[value].focus();
    },
    /**
     * Hàm focus vào input bị lỗi đầu tiên
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    setFocus() {
      try {
        if (this.message) {
          var refName = this.message[0].tittle;
          if (
            refName == Constant.REF_UNIT &&
            this.$refs.Unit &&
            this.$refs.Unit.$refs.input
          ) {
            this.$refs.Unit.$refs.input.focus();
          } else {
            if (this.$refs[refName]) this.$refs[refName].focus();
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm hiển thị pop up
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    showPopUp(isShow) {
      try {
        this.message = [];
        if (this.isChange && isShow) {
          this.isShowPopUp = true;
          this.message.push({
            msg: Resources.CLOSE_CONFIRM,
          });
          this.popUpType = MISAEnum.PopUpType.Confirm;
        } else if (!this.isChange && this.popUpType == 0) {
          this.btnCloseOnClick();
        } else {
          this.isShowPopUp = isShow;
          this.popUpType = 0;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm hiển thị form thêm mới
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    addValue(isUnit) {
      try {
        this.isShowAdding = true;
        this.title = isUnit ? Resources.UNIT_TITLE : Resources.STOCK_TITLE;
        this.addingType = isUnit
          ? MISAEnum.AddingType.Unit
          : MISAEnum.AddingType.Stock;
        this.label = isUnit ? Resources.UNIT_LABEL : Resources.STOCK_LABEL;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm đóng form
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnCloseOnClick() {
      this.showForm(false);
      this.$emit(Constant.EMIT_ADD_EVENT);
    },
    /**
     *Hàm lấy ra giá trị hạn sử dụng
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    getExpiryDate(value) {
      try {
        if (this.ActionType != MISAEnum.ActionType.Insert && value) {
          var now = new Date().getTime();
          switch (this.expiryTime) {
            case MISAEnum.Date.Day:
              value = new Date(value).getTime();
              this.expiryDate = Math.ceil(
                (value - now) / (24 * 60 * 60 * 1000)
              );
              break;
            case MISAEnum.Date.Week:
              value = new Date(value).getTime();
              this.expiryDate = Math.ceil(
                (value - now) / (7 * 24 * 60 * 60 * 1000)
              );
              break;
            case MISAEnum.Date.Month:
              value = new Date(value).getTime();
              this.expiryDate = Math.ceil(
                (value - now) / (30 * 24 * 60 * 60 * 1000)
              );
              break;
            case MISAEnum.Date.Year:
              value = new Date(value).getTime();
              this.expiryDate = Math.ceil(
                (value - now) / (365 * 24 * 60 * 60 * 1000)
              );
              break;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm format hạn sử dụng thành date time
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    formatDate() {
      try {
        if (this.material) {
          this.material.expiryDate = new Date();
          switch (this.expiryTime) {
            case MISAEnum.Date.Day:
              this.material.expiryDate = new Date(
                this.material.expiryDate.setDate(
                  this.material.expiryDate.getDate() + parseInt(this.expiryDate)
                )
              );
              break;
            case MISAEnum.Date.Week:
              this.material.expiryDate = new Date(
                this.material.expiryDate.setDate(
                  this.material.expiryDate.getDate() +
                    parseInt(this.expiryDate * 7)
                )
              );
              break;
            case MISAEnum.Date.Month:
              this.material.expiryDate = new Date(
                this.material.expiryDate.setMonth(
                  this.material.expiryDate.getMonth() +
                    parseInt(this.expiryDate)
                )
              );
              break;
            case MISAEnum.Date.Year:
              this.material.expiryDate = new Date(
                this.material.expiryDate.setFullYear(
                  this.material.expiryDate.getFullYear() +
                    parseInt(this.expiryDate)
                )
              );

              break;
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm kiểm tra trùng mã
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    checkDuplicateUnit(i) {
      try {
        this.isDuplicateUnit = false;
        this.isDuplicateUnitChange = false;
        if (this.material) {
          var item = this.material.conversionUnits;
          if (item.length > 0) {
            for (let index = 0; index < item.length; index++) {
              if (
                item[index].conversionUnitID != 0 &&
                item[index].conversionUnitID == this.material.unitID &&
                item[index].action != MISAEnum.ActionType.Delete
              ) {
                this.isDuplicateUnit = true;
                this.isShowPopUp = true;
                this.popUpType = MISAEnum.PopUpType.Error;
                this.message = [];
                this.message.push({
                  msg: Resources.UNIT_CHANGE_ERROR,
                });
              }
              if (i) {
                if (index == i) continue;
                if (
                  item[index].conversionUnitID != 0 &&
                  item[index].conversionUnitID == item[i].conversionUnitID &&
                  item[index].action != MISAEnum.ActionType.Delete
                ) {
                  this.isDuplicateUnitChange = true;
                  this.isShowPopUp = true;
                  this.popUpType = MISAEnum.PopUpType.Error;
                  this.message = [];
                  this.message.push({
                    msg: Resources.UNIT_CHANGE_DUPLICATE,
                  });
                }
              }
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm Thêm dòng mới trong bảng đơn vị chuyển đổi
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnAddRowOnClick() {
      try {
        if (!this.material.conversionUnits) this.material.conversionUnits = [];
        this.material.conversionUnits.push({
          conversionUnitID: 0,
          conversionUnitName: "",
          calculation: 1,
          conversionRate: "1,00",
          action: MISAEnum.ActionType.Insert,
          description: "",
          createdBy: "admin",
          createdDate: new Date(),
          modifiedBy: "admin",
          modifiedDate: new Date(),
        });
        // var length = this.material.conversionUnits.length;
        // this.$refs.UnitChange[length-1].$refs.input.focus();
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm xóa dòng trong bảng đơn vị chuyển đổi
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    btnDeleteRowOnClick() {
      try {
        var length = this.material.conversionUnits.length;
        if (this.material.conversionUnits[length - 1].oldUnitID) {
          this.deleteIndex++;
          this.material.conversionUnits[length - this.deleteIndex].action =
            MISAEnum.ActionType.Delete;
        } else {
          this.material.conversionUnits.pop();
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     * Hàm validate giá trị
     * AUTHOR: YENVTH
     * CreatedDate:04/10/2022
     */
    getValue(value) {
      try {
        if (this.isSave) {
          var input = this.$refs[value];
          if (!input.value) {
            input.classList.add("border-red");
          } else {
            input.classList.remove("border-red");
          }
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**
     *
     */
    getCode() {
      if (
        this.ActionType == MISAEnum.ActionType.Insert ||
        this.ActionType == MISAEnum.ActionType.Duplicate
      ) {
        this.getNewCode();
      }
    },
    /**
     *Hàm lấy mã nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:04/10/2022
     */
    getNewCode() {
      try {
        if (this.material && this.material.materialName) {
          if (this.$refs.MaterialName)
            axios
              .get(
                Constant.DOMAIN +
                Constant.API_VER +
                Constant.MATERIAL_PATH +
                  `/new-code?MatterialName=${this.material.materialName}`
              )
              .then((response) => {
                if (response) {
                  this.material.materialCode = response.data;
                  this.$refs.MaterialCode.classList.remove("border-red");
                }
              })
              .catch((error) => {
                console.log(error.response);
                this.isLoad = false;
                this.isShowPopUp = true;
                this.popUpType = MISAEnum.PopUpType.Error;
                this.message = [];
                this.message.push({
                  msg: Resources.UNDEFINED_ERROR,
                });
              });
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm lấy danh sách đơn vị
     * AUTHOR: YENVTH
     * CreatedDate:04/10/2022
     */
    getAllUnit(isAdd) {
      axios
        .get(Constant.DOMAIN + Constant.API_VER + Constant.UNIT_PATH)
        .then((response) => {
          if (response && response.data) {
            this.unitList = response.data;
            if (isAdd) {
              this.material.unitID = response.data[0].unitID;
              this.material.unitName = response.data[0].unitName;
              if (
                this.$refs &&
                this.$refs.Unit &&
                this.$refs.Unit.$refs &&
                this.$refs.Unit.$refs.input
              )
                this.$refs.Unit.$refs.input.focus();
            }
          }
        })
        .catch((error) => {
          console.log(error.response);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          this.message.push({
            msg: Resources.UNDEFINED_ERROR,
          });
        });
    },
    /**
     *Hàm lấy danh sách kho ngầm định
     * AUTHOR: YENVTH
     * CreatedDate:04/10/2022
     */
    getAllStock(isAdd) {
      axios
        .get(Constant.DOMAIN + Constant.API_VER + Constant.STOCK_PATH)
        .then((response) => {
          if (response && response.data) {
            this.stockList = response.data;
            if (isAdd) {
              this.material.stockID = response.data[0].stockID;
              this.material.stockName = response.data[0].stockName;
              if (
                this.$refs &&
                this.$refs.Stock &&
                this.$refs.Stock.$refs &&
                this.$refs.Stock.$refs.input
              )
                this.$refs.Stock.$refs.input.focus();
            }
          }
        })
        .catch((error) => {
          console.log(error.response);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          this.message.push({
            msg: Resources.UNDEFINED_ERROR,
          });
        });
    },
    /**
     *Hàm lấy thông tin nguyên vật liệu theo id
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    getMaterialById() {
      this.isLoad = true;
      axios
        .get(
          Constant.DOMAIN +
          Constant.API_VER +
          Constant.MATERIAL_PATH +
            `/${this.MaterialID}`
        )
        .then((response) => {
          if (response && response.data) {
            this.isLoad = false;
          
            this.material = response.data;
            this.formatRate(response.data.inventoryNumber);
            if (
              this.material &&
              this.material.conversionUnits &&
              response.data.conversionUnits
            ) {
              if (response.data.conversionUnits.length > 0) {
                for (
                  let index = 0;
                  index < response.data.conversionUnits.length;
                  index++
                ) {
                  this.formatRate(
                    response.data.conversionUnits[index].conversionRate,
                    index
                  );
                }
              }
            }
            this.getExpiryDate(response.data.expiryDate);
            if (this.ActionType == MISAEnum.ActionType.Duplicate) {
              this.getNewCode();
            }
          }
        }).then(() => { this.isChange = false;}

        )
        .catch((error) => {
          console.log(error);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          this.message.push({
            msg: Resources.UNDEFINED_ERROR,
          });
        });
    },
    /**
     *Hàm reset lại nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    resetMaterial() {
      try {
        this.material = {
          materialCode: "",
          materialName: "",
          expiryDate: null,
          unitID: "",
          unitName: "",
          stockID: null,
          stockName: null,
          inventoryNumber: "0,00",
          description: "",
          status: 1,
          createdBy: "admin",
          createdDate: new Date(),
          modifiedBy: "admin",
          modifiedDate: new Date(),
          conversionUnits: [],
        };
        this.expiryDate = null;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm validate dữ liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    validate() {
      try {
        this.message = [];
        if (this.material) {
          if (this.material.materialName == "") {
            this.$refs.MaterialName.classList.add("border-red");
            this.message.push({
              msg: Resources.MATERIAL_NAME_ERROR,
              tittle: Constant.REF_MATERIAL_NAME,
            });
          }
          if (!this.material.materialCode) {
            this.$refs.MaterialCode.classList.add("border-red");
            this.message.push({
              msg: Resources.MATERIALE_CODE_ERROR,
              tittle: Constant.REF_MATERIAL_CODE,
            });
          }
          if (!this.material.unitName) {
            this.$refs.Unit.validate(true);
            this.message.push({
              msg: Resources.UNIT_ERROR,
              tittle: Constant.REF_UNIT,
            });
          }
          if (this.isDuplicateUnit) {
            this.message.push({
              msg: Resources.UNIT_CHANGE_ERROR,
            });
          }
          if (this.isDuplicateUnitChange) {
            this.message.push({
              msg: Resources.UNIT_CHANGE_DUPLICATE,
            });
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm thực hiện function tương ứng khi click button cất hoặc cất và thêm
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    saveOnClick(isContinue) {
      try {
        this.isSave = true;
        this.validate();
        this.material.inventoryNumber = 
          this.material.inventoryNumber
            .toString()
            .replaceAll(".", "").replace(",",".")
        ;
        if (this.material && this.material.conversionUnits) {
          if (this.material.conversionUnits.length > 0) {
            for (
              let index = 0;
              index < this.material.conversionUnits.length;
              index++
            ) {
              this.material.conversionUnits[index].conversionRate = parseFloat(
                this.material.conversionUnits[index].conversionRate
                  .toString()
                  .replaceAll(".", "")
                  .replace(",", ".")
              );
              if (this.material.conversionUnits[index].conversionUnitID == 0) {
                this.material.conversionUnits.splice(index, 1);
                index--;
              }
            }
          }
        }
        if (this.message) {
          if (this.message.length > 0) {
            this.isShowPopUp = true;
            this.popUpType = MISAEnum.PopUpType.Error;
          } else {
            if (this.ActionType == MISAEnum.ActionType.Update) {
              this.updateMaterial(isContinue);
            } else {
              this.insertMaterial(isContinue);
            }
          }
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm Thêm nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    insertMaterial(isContinue) {
      this.isLoad = true;
      this.material.modifiedDate = new Date();
      axios
        .post(
          Constant.DOMAIN + Constant.API_VER + Constant.MATERIAL_PATH,
          this.material
        )
        .then((response) => {
          if (response.data) {
            if (isContinue) {
              this.isLoad = false;
              this.resetMaterial();
              this.$emit(
                Constant.EMIT_LOAD_DATA,
                this.pageSize,
                1,
                this.materialFillter
              );
            } else {
              this.showForm(false);
              this.$emit(
                Constant.EMIT_LOAD_DATA,
                this.pageSize,
                1,
                this.materialFillter
              );
            }
            this.$emit(
              Constant.EMIT_SHOW_ALERT,
              true,
              Resources.INSERT_SUCCESS,
              MISAEnum.Status.Success
            );
          }
        })
        .catch((error) => {
          console.log(error);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          if (error.response.data.errorCode == 3) {
            if (this.$refs.MaterialCode)
              this.$refs.MaterialCode.classList.add("border-red");
            this.message.push({
              msg: ` Mã nhân viên << ${this.material.materialCode} >> đã tồn tại trong hệ thống!`,
              tittle: Constant.REF_MATERIAL_CODE,
            });
          } else {
            this.message.push({
              msg: "Có lỗi xảy ra! Vui lòng liên hệ MISA!",
              tittle: Constant.REF_MATERIAL_CODE,
            });
          }
        });
    },
    /**
     *Hàm sửa nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    updateMaterial(isContinue) {
      this.isLoad = true;
      this.material.modifiedDate = new Date();
      axios
        .put(
          Constant.DOMAIN +
          Constant.API_VER +
          Constant.MATERIAL_PATH +
            `/${this.MaterialID}`,
          this.material
        )
        .then((response) => {
          if (response.data) {
            this.isLoad = false;
            if (isContinue) {
              this.$emit(Constant.EMIT_UPDATE_ACTION_TYPE, MISAEnum.ActionType.Insert);
              this.$emit(Constant.EMIT_UPDATE_TITLE, Resources.ADD_TITLE);
              this.resetMaterial();
              this.$emit(
                Constant.EMIT_LOAD_DATA,
                this.pageSize,
                1,
                this.materialFillter
              );
            } else {
              this.showForm(false);
              this.$emit(
                Constant.EMIT_LOAD_DATA,
                this.pageSize,
                1,
                this.materialFillter
              );
            }
            this.$emit(
              Constant.EMIT_SHOW_ALERT,
              true,
              Resources.UPDATE_SUCCESS,
              MISAEnum.Status.Success
            );
          }
        })
        .catch((error) => {
          console.log(error);
          this.isLoad = false;
          this.isShowPopUp = true;
          this.popUpType = MISAEnum.PopUpType.Error;
          this.message = [];
          if (error.response.data.errorCode == 3) {
            if (this.$refs.MaterialCode)
              this.$refs.MaterialCode.classList.add("border-red");
            this.message.push({
              msg: ` Mã nhân viên << ${this.material.materialCode} >> đã tồn tại trong hệ thống!`,
              tittle: Constant.REF_MATERIAL_CODE,
            });
          } else {
            this.message.push({
              msg: Resources.UNDEFINED_ERROR,
              tittle: Constant.REF_MATERIAL_CODE,
            });
          }
        });
    },

    /**
     *truyền giá trị cho alert
     * @param {*} isShow: ẩn hiển alert
     * @param {*} msg: thông báo
     * @param {*} type:loại alert cần hiển thị
     *  CreatedDate:28/09/2022
     */
    setEmitAlert(isShow, msg, type) {
      this.$emit(Constant.EMIT_SHOW_ALERT, isShow, msg, type);
    },

    /**
     *kiểm tra input đầu vào là số
     @param evt: giá trị event
     CreatedDate:28/09/2022
     */
    isNumber(evt) {
      try {
         evt = evt ? evt : window.event;
      var charCode = evt.which ? evt.which : evt.keyCode;
      if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        evt.preventDefault();
      } else {
        return true;
      }
      } catch (error) {
        console.log(error);
      }
     
    },
  },
  created() {
    this.getAllUnit();
    this.getAllStock();
    if (
      this.ActionType == MISAEnum.ActionType.Update ||
      this.ActionType == MISAEnum.ActionType.Duplicate
    ) {
      this.getMaterialById();
    }
  },
  mounted() {
    this.$refs.MaterialName.focus();
  },
};
</script>