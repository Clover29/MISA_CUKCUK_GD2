<template>
  <div class="dialog">
    <div class="dialog__content" style="width: 30%">
      <div class="dialog__header">
        <div class="form__title">{{ title }}</div>
        <div class="dialog__header--rigt">
          <i
            class="fa-solid fa-up-right-and-down-left-from-center icon__resize"
          ></i>
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
            <input class="form__text" type="text" v-model="firstValue" />
          </div>
          <div class="row" v-if="type == stockType">
            <label class="form__label"
              >Tên kho
              <span class="input--required">(*)</span>
            </label>
            <input class="form__text" type="text" v-model="secondValue" />
          </div>
        </div>
        <div class="dialog__footer">
          <button class="btn">
            <i class="fa-solid fa-circle-question icon__add"></i>Giúp
          </button>
          <div class="footer__right">
            <button class="btn" style="margin-right: 8px" @click="saveOnClick">
              <i class="fa-regular fa-floppy-disk icon__add"></i>Cất
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
  <script>
import axios from "axios";
import { MISAEnum } from "@/js/Enum";
import { Resources } from "@/js/Resources";
export default {
  name: "AddValue",
  props: ["title", "label", "type", "isShow"],
  data() {
    return {
      isLoad: false,
      unitType: MISAEnum.AddingType.Unit,
      stockType: MISAEnum.AddingType.Stock,
      firstValue: "",
      secondValue: "",
      unit: {},
      stock: {},
    };
  },
  methods: {
    btnCloseOnClick() {
      this.$emit("update:isShow", false);
    },
    saveOnClick() {
      if (this.type == this.unitType) {
        this.insertUnit();
      } else {
        this.insertStock();
      }
    },
    insertUnit() {
      this.unit = {
        unitName: this.firstValue
      };
      this.isLoad = true;
      axios
        .post(
          Resources.DOMAIN + Resources.API_VER + Resources.UNIT_PATH,
          this.unit
        )
        .then((response) => {
          if (response.data) {
            this.$emit("update:isShow", false);
            this.$emit("loadUnit");
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
    insertStock() {
      this.stock = {
        stockCode: this.firstValue,
        stockName: this.secondValue,
      };
      this.isLoad = true;
      axios
        .post(
          Resources.DOMAIN + Resources.API_VER + Resources.STOCK_PATH,
          this.stock
        )
        .then((response) => {
          if (response.data) {
            this.$emit("update:isShow", false);
            this.$emit("loadStock");
          }
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>