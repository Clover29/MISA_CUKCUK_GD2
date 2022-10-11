<template>
  <div class="dialog">
    <div class="dialog__content">
      <div class="dialog__header">
        <div class="form__title">{{Title}} nguyên vật liệu</div>
        <div class="dialog__header--rigt">
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
                v-model="material.materialName"
              />
            </div>
            <div class="row">
              <label class="form__label"
                >ĐVT
                <span class="input--required">(*)</span>
              </label>
              <ComboboxVue
                ref="Unit"
                :width="'col_width_70'"
                :dropdown__icon="'position__dropdown--72'"
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
                @blur="formatDate"
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
                ref="MaterialCode"
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
              <label class="form__label">SL tồn tối thiểu</label>
              <input
                class="form__text col__width_34 text-right"
                type="text"
                name="Number"
                v-model="material.inventoryNumber"
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
              <th class="col-1">STT</th>
              <th class="col-5">Đơn vị chuyển đổi</th>
              <th class="col-3">Tỷ lệ chuyển đổi</th>
              <th>Phép tính</th>
              <th class="col-5">Mô tả</th>
            </tr>
            <tr
              v-for="(item, index) in material.conversionUnits"
              :key="item.numerical"
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
                  v-model="item.conversionRate"
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
            class="btn"
            style="margin-right: 8px"
            @click="btnAddRowOnClick"
          >
            <i class="fa-solid fa-file-circle-plus icon__add"></i>Thêm dòng
          </button>
          <button
            class="btn"
            @click="btnDeleteRowOnClick"
            :disabled="material.conversionUnits.length == 0"
          >
            <i class="fa-solid fa-xmark icon__delete"></i>Xóa dòng
          </button>
        </div>
        <div class="dialog__footer">
          <button class="btn">
            <i class="fa-solid fa-circle-question icon__add"></i>Giúp
          </button>
          <div class="footer__right">
            <button
              class="btn"
              style="margin-right: 8px"
              @click="saveOnClick(false)"
            >
              <i class="fa-regular fa-floppy-disk icon__add"></i>Cất
            </button>
            <button
              class="btn"
              style="margin-right: 8px"
              @click="saveOnClick(true)"
            >
              <i class="fa-solid fa-floppy-disk icon__add"></i>Cất và thêm
            </button>
            <button class="btn" @click="showPopUp(true)">
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
  />
</template>
<style scoped>
@import url(dialog.css);
</style>
 <script>
import axios from "axios";
import { Resources } from "../../../js/Resources.js";
import SelectionBox from "../SelectionBox/SelectionBox.vue";
import ComboboxVue from "../Combobox/ComboBoxVue.vue";
import { MISAEnum } from "@/js/Enum.js";
import ThePopUp from "../PopUp/ThePopUp.vue";
import LoadData from "../Loading/LoadData.vue";
import AddValue from "./AddValue.vue";

export default {
  name: "MaterialForm",
  components: { SelectionBox, ComboboxVue, ThePopUp, LoadData, AddValue },
  emits: ["LoadData", "showAlert"],
  props: ["showForm", "MaterialID", "ActionType","Title"],
  data() {
    return {
      oldInput: "", // giấ trị tên nguyên vật liệu trước khi sửa
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
      expiryTime:  MISAEnum.Date.Day, //"giá trị hạn sử dụng"
      calculation: [
        { id: 1, value:MISAEnum.Calculation.Multiple, text: MISAEnum.Calculation.MultipleText },
        { id: 2, value: MISAEnum.Calculation.Divide, text:MISAEnum.Calculation.DivideText },
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
        inventoryNumber: 0,
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
      pageSize: 10, //số bản ghi muốn hiển thị trên một trang
    };
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
     * Hàm focus vào input bị lỗi đầu tiên
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    setFocus() {
      try {
        if (this.message) {
          var refName = this.message[0].tittle;
          if (
            refName == Resources.REF_UNIT &&
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
        this.isShowPopUp = isShow;
        if (isShow) {
          this.message.push({
            msg: Resources.CLOSE_CONFIRM,
          });
          this.popUpType = MISAEnum.PopUpType.Confirm;
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
    },
    /**
     *Hàm lấy ra giá trị hạn sử dụng
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    getExpiryDate(value) {
      try {
        var now = new Date().getTime();
        switch (this.expiryTime) {
          case "Ngày":
            value = new Date(value).getTime();
            this.expiryDate = Math.ceil((value - now) / (24 * 60 * 60 * 1000));
            break;
          case "Tuần":
            value = new Date(value).getTime();
            this.expiryDate = Math.ceil(
              (value - now) / (7 * 24 * 60 * 60 * 1000)
            );
            break;
          case "Tháng":
            value = new Date(value).getTime();
            this.expiryDate = Math.ceil(
              (value - now) / (30 * 24 * 60 * 60 * 1000)
            );
            break;
          case "Năm":
            value = new Date(value).getTime();
            this.expiryDate = Math.ceil(
              (value - now) / (365 * 24 * 60 * 60 * 1000)
            );
            break;
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
            case "Ngày":
              this.material.expiryDate = new Date(
                this.material.expiryDate.setDate(
                  this.material.expiryDate.getDate() + parseInt(this.expiryDate)
                )
              );
              break;
            case "Tuần":
              this.material.expiryDate = new Date(
                this.material.expiryDate.setDate(
                  this.material.expiryDate.getDate() +
                    parseInt(this.expiryDate * 7)
                )
              );
              break;
            case "Tháng":
              this.material.expiryDate = new Date(
                this.material.expiryDate.setMonth(
                  this.material.expiryDate.getMonth() +
                    parseInt(this.expiryDate)
                )
              );
              break;
            case "Năm":
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
        if (this.material) {
          var item = this.material.conversionUnits;
          if (item.length > 0) {
            for (let index = 0; index < item.length; index++) {
              if (
                item[index].conversionUnitID != 0 &&
                item[index].conversionUnitID == this.material.unitID
              ) {
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
                  item[index].conversionUnitID == item[i].conversionUnitID
                ) {
                  this.isShowPopUp = true;
                  this.popUpType = MISAEnum.PopUpType.Error;
                  this.message = [];
                  this.message.push({
                    msg:Resources.UNIT_CHANGE_DUPLICATE,
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
        this.material.conversionUnits.push({
          conversionUnitID: 0,
          conversionUnitName: "",
          calculation: 1,
          conversionRate: 1,
          description: "",
          createdBy: "admin",
          createdDate: new Date(),
          modifiedBy: "admin",
          modifiedDate: new Date(),
        });
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
        if (
          this.material &&
          this.material.conversionUnits &&
          this.material.conversionUnits[
            this.material.conversionUnits.length - 1
          ].conversionUnitID != 0 &&
          this.ActionType == MISAEnum.ActionType.Update
        ) {
          this.isLoad = true;
          axios
            .delete(
              Resources.DOMAIN +
                Resources.API_VER +
                Resources.CONVERSION_UNIT_PATH +
                `/${this.MaterialID}&${
                  this.material.conversionUnits[
                    this.material.conversionUnits.length - 1
                  ].conversionUnitID
                }`
            )
            .then((response) => {
              if (response.data) {
                this.isLoad = false;
              }
            })
            .catch((error) => {
              console.log(error.response);
            });
        }
        this.material.conversionUnits.pop();
      } catch (error) {
        console.log(error);
      }
    },
    getCode() {
      if (
        this.ActionType == MISAEnum.ActionType.Insert ||
        this.material.materialName != this.oldInput
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
            this.$refs.MaterialName.classList.remove(Resources.BORDER_CLASS);
          this.isLoad = true;
          axios
            .get(
              Resources.DOMAIN +
                Resources.API_VER +
                Resources.MATERIAL_PATH +
                `/new-code?MatterialName=${this.material.materialName}`
            )
            .then((response) => {
              if (response) {
                this.material.materialCode = response.data;
                if (this.$refs.MaterialCode)
                  this.$refs.MaterialCode.classList.remove(Resources.BORDER_CLASS);
                this.isLoad = false;
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
        } else {
          if (this.$refs.MaterialName && this.$refs.MaterialCode) {
            if (this.material.materialName) {
              this.$refs.MaterialName.classList.remove(Resources.BORDER_CLASS);
              this.$refs.MaterialCode.classList.remove(Resources.BORDER_CLASS);
            } else {
              this.$refs.MaterialName.classList.add(Resources.BORDER_CLASS);
              this.$refs.MaterialCode.classList.add(Resources.BORDER_CLASS);
            }
          }
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
        .get(Resources.DOMAIN + Resources.API_VER + Resources.UNIT_PATH)
        .then((response) => {
          if (response && response.data) {
            this.unitList = response.data;
            this.material.unitID = response.data[0].unitID;
            if (isAdd) {
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
        .get(Resources.DOMAIN + Resources.API_VER + Resources.STOCK_PATH)
        .then((response) => {
          if (response && response.data) {
            this.stockList = response.data;
            this.material.stockID = response.data[0].stockID;
            if (isAdd) {
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
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/${this.MaterialID}`
        )
        .then((response) => {
          if (response && response.data) {
            this.isLoad = false;
            this.material = response.data;
            this.oldInput = response.data.materialName;
            this.getExpiryDate(response.data.expiryDate);
            if (this.ActionType == MISAEnum.ActionType.Duplicate) {
              this.getNewCode();
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
     *Hàm reset lại nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    resetMaterial() {
      try {
        this.material = {
          materialCode: "",
          materialName: "",
          expiryDate: "",
          unitID: "",
          unitName: "",
          stockID: null,
          stockName: null,
          inventoryNumber: 0,
          description: "",
          status: 1,
          createdBy: "admin",
          createdDate: new Date(),
          modifiedBy: "admin",
          modifiedDate: new Date(),
          conversionUnits: [],
        };
        this.expiryDate = 0;
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
            this.message.push({
              msg: Resources.MATERIAL_NAME_ERROR,
              tittle: Resources.REF_MATERIAL_NAME,
            });
          }
          if (!this.material.materialCode) {
            this.message.push({
              msg: Resources.MATERIALE_CODE_ERROR,
              tittle: Resources.REF_MATERIAL_CODE,
            });
          }
          if (!this.material.unitName) {
            this.$refs.Unit.validate(true);
            this.message.push({
              msg: Resources.UNIT_ERROR,
              tittle: Resources.REF_UNIT,
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
        this.validate();
        if (this.material && this.material.conversionUnits) {
          if (this.material.conversionUnits.length > 0) {
            for (
              let index = 0;
              index < this.material.conversionUnits.length;
              index++
            ) {
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
          Resources.DOMAIN + Resources.API_VER + Resources.MATERIAL_PATH,
          this.material
        )
        .then((response) => {
          if (response.data) {
            if (isContinue) {
              this.isLoad = false;
              this.resetMaterial();
              this.$emit(Resources.EMIT_LOAD_DATA, this.pageSize, 1, this.materialFillter);
            } else {
              this.showForm(false);
              this.$emit(Resources.EMIT_LOAD_DATA, this.pageSize, 1, this.materialFillter);
            }
            this.$emit(
              Resources.EMIT_SHOW_ALERT,
              true,
              Resources.INSERT_SUCCESS,
              MISAEnum.Status.Success
            );
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
              msg: ` Mã nhân viên << ${this.material.materialCode} >> đã tồn tại trong hệ thống!`,
              tittle:Resources.REF_MATERIAL_CODE
            });
           if(this.$refs.MaterialCode) this.$refs.MaterialCode.classList.add("border-red");
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
     *Hàm sửa nguyên vật liệu
     * AUTHOR: YENVTH
     * CreatedDate:28/09/2022
     */
    updateMaterial(isContinue) {
      this.isLoad = true;
      this.material.modifiedDate = new Date();
      axios
        .put(
          Resources.DOMAIN +
            Resources.API_VER +
            Resources.MATERIAL_PATH +
            `/${this.MaterialID}`,
          this.material
        )
        .then((response) => {
          if (response.data) {
            this.isLoad = false;
            if (isContinue) {
              this.resetMaterial();
              this.$emit(Resources.EMIT_LOAD_DATA, this.pageSize, 1, this.materialFillter);
            } else {
              this.showForm(false);
              this.$emit(Resources.EMIT_LOAD_DATA, this.pageSize, 1, this.materialFillter);
            }
            this.$emit(
             Resources.EMIT_SHOW_ALERT,
              true,
              Resources.UPDATE_SUCCESS,
              MISAEnum.Status.Success
            );
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
              msg: ` Mã nhân viên << ${this.material.materialCode} >> đã tồn tại trong hệ thống!`,
            });
          }
        });
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