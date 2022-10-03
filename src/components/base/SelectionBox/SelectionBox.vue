<template>
  <button class="btn" @click="showDropDown($event)" v-if="isButtonShow">{{fillterText}}</button>
  <div class="selectionbox" :class="div_class" v-if="isSelectShow">
    <input
      type="text"
      :value="optionValue"
      readonly
      :class="input_class"
    />
    <div class="icon icon-16 icon__dropdown" :class="icon_class" @click="showDropDown($event)"></div>
    <i v-show="isAdd" class="fa-solid fa-plus icon__add position__plus"></i>
  </div>
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
</template>
<style scoped>
@import url(selectionbox.css);
</style>
<script>
import { MISAEnum } from "../../../js/Enum.js";
export default {
    emits:['PageSize','Status','fillter'],
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
    "isButtonShow"
  ],
  data() {
    return {
      isShow: this.isDropDownShow, // ẩn hiện option của selectbox
      optionId: this.optionID, //id của option
      optionValue: this.value,
      fillterText:MISAEnum.FillterOption.Contain.value,
    };
  },
  methods: {
    showDropDown(){ 
        this.isShow = !this.isShow;
    },
   
    onClickOption(data) {
      try {
        this.optionId = data.id;
        this.isShow = false;
       
        switch (this.optionType) {
          case MISAEnum.OptionType.Paging:
             this.optionValue = data.value;
            this.$emit("PageSize", data.value);
            break;
            case MISAEnum.OptionType.Status:
            this.optionValue = data.text;
            this.$emit("Status",data.value);
            break;
            case MISAEnum.OptionType.Fillter:
           this.fillterText = data.value;
           this.$emit("fillter",data.value);
            break;
          default:
            break;
        }
      } catch (error) {
        console.log(error);
      }
    },
  },
};
</script>