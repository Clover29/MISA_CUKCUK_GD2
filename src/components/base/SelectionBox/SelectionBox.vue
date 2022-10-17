<template>
  <div class="selectionbox" :class="div_class">
    <OnClickOutside @trigger="onClickOutside">
      <button class="btn" @click="showDropDown()" v-if="isButtonShow">
        {{ fillterText }}
      </button>
    </OnClickOutside>
    <div
      style="display: flex"
      v-if="isSelectShow"
      @keydown="keyMonitor($event)"
    >
      <input type="text" :value="optionValue" readonly :class="input_class" />
      <OnClickOutside @trigger="onClickOutside">
        <div
          class="icon icon-16 icon__dropdown"
          :class="icon_class"
          @click="showDropDown()"
        ></div>
      </OnClickOutside>
    </div>

    <i v-show="isAdd" class="fa-solid fa-plus icon__add position__plus"></i>
    <div class="combobox__option" v-show="isShow" :class="option_class">
      <div
        v-for="item in option"
        :key="item.id"
        class="combobox__option--row"
        :class="{ combobox__highlight: optionId == item.id }"
        @click="onClickOption(item)"
      >
        {{ item.text }}
      </div>
    </div>
  </div>
</template>
<style scoped>
@import url(selectionbox.css);
</style>
<script>
import { MISAEnum } from "../../../js/Enum.js";
import { OnClickOutside } from "@vueuse/components";
import { Constant } from "@/js/Constant";
export default {
  emits: ["PageSize", "Status", "fillter"],
  components: { OnClickOutside },
  props: [
    "div_class",
    "input_class",
    "icon_class",
    "option_class",
    "names",
    "value",
    "isAdd",
    "option",
    "optionType",
    "isSelectShow",
    "isDropDownShow",
    "optionID",
    "isButtonShow",
  ],
  data() {
    return {
      isShow: this.isDropDownShow, // ẩn hiện option của selectbox
      optionId: this.optionID, //id của option
      optionValue: this.value, //giá trị khi đc chọn
      fillterText: MISAEnum.FillterOption.Contain.value, // loại fillter muốn thực hiện
      index:0, // chỉ số của item được lựa chọn
    };
  },
  watch:{
    index(){
      this.optionId = this.option[this.index].id;
    }
  },
  methods: {
    /**
     * tắt dropdown khi click ra các vùng bên ngoài
     * AUTHOR: YENVTH
     * CreatedDate: 08/10/2022
     */
    onClickOutside() {
      try {
        this.isShow = false;
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *Hàm ẩn hiện dropdown
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    showDropDown() {
      this.isShow = !this.isShow;
    },
    /**
     *Hàm gán dữ liệu khi chọn option
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    onClickOption(data) {
      try {
        this.optionId = data.id;
        this.isShow = false;

        switch (this.optionType) {
          case MISAEnum.OptionType.Paging:
            this.optionValue = data.value;
            this.$emit(Constant.EMIT_PAGESIZE, data.value);
            break;
          case MISAEnum.OptionType.Status:
            this.optionValue = data.text;
            this.$emit(Constant.EMIT_STATUS, data.value);
            break;
          case MISAEnum.OptionType.Fillter:
            this.fillterText = data.value;
            this.$emit(Constant.EMIT_FILLTER, data.value);
            break;
          case MISAEnum.OptionType.Calculation:
            this.optionValue = data.value;
            this.$emit(Constant.EMIT_UPDATE_OPTIONID, data.id);
            break;
          default:
            this.$emit(Constant.EMIT_UPDATE_VALUE, data.text);
            this.$emit(Constant.EMIT_FORMATDATE);
            this.optionValue = data.text;
            break;
        }
      } catch (error) {
        console.log(error);
      }
    },
    /**
     *set index tương ứng với các phím tương ứng
     * AUTHOR: YENVTH
     * CreatedDate:03/10/2022
     */
    keyMonitor(event) {
      try {
          switch (event.key) {
            case MISAEnum.KeyBoard.Enter:
                 this.onClickOption(this.option[this.index]);
              break;
            case MISAEnum.KeyBoard.ArrowDown:
              if(this.isShow) this.index =this.index >= this.option.length - 1
                ? 0
                : this.index + 1;
              this.isShow = true;
              break;
            case MISAEnum.KeyBoard.ArrowUp: 
            if(this.isShow) this.index =
              this.index <= 0
                ? this.option.length - 1
                : this.index - 1;
              this.isShow = true;
              break;
          }
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>